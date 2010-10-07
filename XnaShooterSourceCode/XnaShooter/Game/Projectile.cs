// Project: XnaShooter, File: EnemyUnit.cs
// Namespace: XnaShooter.Game, Class: EnemyUnit
// Path: C:\code\XnaShooter\Game, Author: Abi
// Code lines: 167, Size of file: 4,73 KB
// Creation date: 27.12.2006 06:10
// Last modified: 27.12.2006 15:14
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using XnaShooter.Shaders;
using XnaShooter.Graphics;
using XnaShooter.Helpers;
using XnaShooter.GameScreens;
using XnaShooter.Game;
using Material = XnaShooter.Graphics.Material;
using Microsoft.Xna.Framework;
#endregion

namespace XnaShooter.Game
{
	/// <summary>
	/// Unit values, used for both the GameAsteroidManager to keep all
	/// enemy units in the current level and in UnitManager for all active
	/// units we are currently rendering.
	/// </summary>
	public class Projectile
	{
		#region Variables
		/// <summary>
		/// Weapon types
		/// </summary>
		public enum WeaponTypes
		{
			Rocket,
			Plasma,
			Fireball,
			/*obs
			Health,
			MgItem,
			PlasmaItem,
			GattlingItem,
			RocketItem,
			EmpItem,
			 */
		} // enum WeaponTypes

		/// <summary>
		/// Unit type, for enemy units this is 3-7 (Kamikaze - Mine).
		/// Links to the unit settings list in UnitManager.
		/// </summary>
		public WeaponTypes weaponType;

		/// <summary>
		/// Damage this unit currently does. Copied from unit settings,
		/// but increase as the level advances.
		/// </summary>
		public float damage;

		/// <summary>
		/// Helper for plasma and fireball effects, rotation is calculated
		/// once in constructor and used for the effects.
		/// </summary>
		public float effectRotation;

		/// <summary>
		/// Max speed for this unit (calculated from unit settings, increased
		/// a little for each level). Any movement is limited by this value.
		/// Units don't have to fly as fast as this value, this is just the limit.
		/// </summary>
		public float maxSpeed;
		/// <summary>
		/// Current position of this unit, will be updated each frame.
		/// Absolute position in 3d space.
		/// </summary>
		public Vector3 position;

		/// <summary>
		/// Move direction, not used for plasma projectiles, but fireballs will
		/// be moving towards the player (but not change direction during flight).
		/// Rockets on the other hand will slowly adjust to the player position.
		/// </summary>
		public Vector3 moveDirection;

		/// <summary>
		/// Own projectile? Then it flys up and damages enemies, else it
		/// is an enemy projectile!
		/// </summary>
		public bool ownProjectile = false;

		/// <summary>
		/// Life time of this projectile, will die after 6 seconds!
		/// </summary>
		private float lifeTimeMs = 0;
		#endregion

		#region Constructor
		/// <summary>
		/// Create unit of specific type at specific location.
		/// </summary>
		/// <param name="setType">Set type</param>
		/// <param name="setPosition">Set position</param>
		public Projectile(WeaponTypes setType, Vector3 setPosition,
			bool setOwnProjectile)
		{
			weaponType = setType;
			position = setPosition;
			ownProjectile = setOwnProjectile;

			// 300 dmg for plasma, 1000 for rockets
			damage = weaponType == WeaponTypes.Plasma ? 300 : 1000;

			if (ownProjectile == false)
			{
				if (weaponType == WeaponTypes.Fireball)
				{
					Vector3 distVec = Player.shipPos - position;
					moveDirection = Vector3.Normalize(
						Player.shipPos + new Vector3(0, distVec.Length() / 3, 0) -
						position);
					damage = 70;// 75;
				} // if
				else if (weaponType == WeaponTypes.Rocket)
				{
					moveDirection = new Vector3(0, -1, 0);
					damage = 110;// 125;
				} // else if
			} // if

			if (weaponType == WeaponTypes.Rocket)
				maxSpeed = 30;//80;
			else if (weaponType == WeaponTypes.Plasma)
				maxSpeed = 75;//150;
			else if (weaponType == WeaponTypes.Fireball)
				maxSpeed = 20;//45;

			if (weaponType == WeaponTypes.Plasma ||
				weaponType == WeaponTypes.Fireball)
				effectRotation = RandomHelper.GetRandomFloat(
					0, (float)Math.PI * 2.0f);
		} // Unit(setType, setPosition, setLevel)
		#endregion

		#region Get rotation angle
		private float GetRotationAngle(Vector3 pos1, Vector3 pos2)
		{
			// See http://en.wikipedia.org/wiki/Vector_(spatial)
			// for help and check out the Dot Product section ^^
			// Both vectors are normalized so we can save deviding through the
			// lengths.
			Vector3 vec1 = new Vector3(0, 1, 0);
			Vector3 vec2 = pos1 - pos2;
			vec2.Normalize();
			return (float)Math.Acos(Vector3.Dot(vec1, vec2));
		} // GetRotationAngle(pos1, pos2)

		private float GetRotationAngle(Vector3 vec)
		{
			return (float)Math.Atan2(vec.Y, vec.X);
		} // GetRotationAngle(pos1, pos2)
		#endregion

		#region Render
		/// <summary>
		/// Render projectile, returns false if we are done with it.
		/// Has to be removed then. Else it updates just position.
		/// </summary>
		/// <returns>True if done, false otherwise</returns>
		public bool Render(Mission mission)
		{
			#region Update movement
			lifeTimeMs += BaseGame.ElapsedTimeThisFrameInMs;
			float moveSpeed = BaseGame.MoveFactorPerSecond;
			if (Player.GameOver)
				moveSpeed = 0;
			switch (weaponType)
			{
				case WeaponTypes.Fireball:
					position += moveDirection * maxSpeed * moveSpeed;
					break;
				case WeaponTypes.Rocket:
					if (ownProjectile)
						position += new Vector3(0, +1, 0) * maxSpeed * moveSpeed * 1.1f;
					else
					{
						// Fly to player
						Vector3 targetMovement = Player.shipPos - position;
						targetMovement.Normalize();
						moveDirection = moveDirection * 0.95f + targetMovement * 0.05f;
						moveDirection.Normalize();
						position += moveDirection * maxSpeed * moveSpeed;
					} // else
					break;
				case WeaponTypes.Plasma:
					if (ownProjectile)
						position += new Vector3(0, +1, 0) * maxSpeed * moveSpeed * 1.25f;
					else
						position += new Vector3(0, -1, 0) * maxSpeed * moveSpeed;
					break;
				// Rest are items, they just stay around
			} // switch(movementPattern)
			#endregion

			#region Skip if out of visible range
			float distance = Mission.LookAtPosition.Y - position.Y;
			const float MaxUnitDistance = 35;

			// Remove unit if it is out of visible range!
			if (distance > MaxUnitDistance ||
				distance < -MaxUnitDistance * 2 ||
				position.Z < 0 ||
				lifeTimeMs > 6000)
				return true;
			#endregion

			#region Render
			switch (weaponType)
			{
				case WeaponTypes.Rocket:
					float rocketRotation = MathHelper.Pi;
					if (ownProjectile == false)
						rocketRotation = MathHelper.PiOver2 + GetRotationAngle(moveDirection);
					mission.AddModelToRender(
						mission.shipModels[(int)Mission.ShipModelTypes.Rocket],
						Matrix.CreateScale(
						(ownProjectile ? 1.25f : 0.75f) *
						Mission.ShipModelSize[(int)Mission.ShipModelTypes.Rocket]) *
						Matrix.CreateRotationZ(rocketRotation) *
						Matrix.CreateTranslation(position));
					// Add rocket smoke
					EffectManager.AddRocketOrShipFlareAndSmoke(position, 1.5f, 6 * maxSpeed);
					break;
				case WeaponTypes.Plasma:
					EffectManager.AddPlasmaEffect(position, effectRotation, 1.25f);
					break;
				case WeaponTypes.Fireball:
					EffectManager.AddFireBallEffect(position, effectRotation, 1.25f);
					break;
			} // switch
			#endregion

			#region Explode if hitting unit
			// Own projectile?
			if (ownProjectile)
			{
				// Hit enemy units, check all of them
				for (int num = 0; num < Mission.units.Count; num++)
				{
					Unit enemyUnit = Mission.units[num];
					// Near enough to enemy ship?
					Vector2 distVec =
						new Vector2(enemyUnit.position.X, enemyUnit.position.Y) -
						new Vector2(position.X, position.Y);
					if (distVec.Length() < 7 &&
						(enemyUnit.position.Y - Player.shipPos.Y) < 60)
					{
						// Explode and do damage!
						EffectManager.AddFlameExplosion(position);
						Player.score += (int)enemyUnit.hitpoints / 10;
						enemyUnit.hitpoints -= damage;
						return true;
					} // if
				} // for
			} // if
			// Else this is an enemy projectile?
			else
			{
				// Near enough to our ship?
				Vector2 distVec =
					new Vector2(Player.shipPos.X, Player.shipPos.Y) -
					new Vector2(position.X, position.Y);
				if (distVec.Length() < 2.75f)//3)
				{
					// Explode and do damage!
					EffectManager.AddFlameExplosion(position);
					Player.health -= damage / 1000.0f;
					return true;
				} // if
			} // else
			#endregion

			// Don't remove unit from units list
			return false;
		} // Render()
		#endregion
	} // class Unit
} // namespace XnaShooter.Game
