// Project: XnaShooter, File: Unit.cs
// Namespace: XnaShooter.Game, Class: Unit
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace XnaShooter.Game
{
	/// <summary>
	/// Unit values, used for both the GameAsteroidManager to keep all
	/// enemy units in the current level and in UnitManager for all active
	/// units we are currently rendering.
	/// </summary>
	public class Unit
	{
		#region Variables
		/// <summary>
		/// Unit types
		/// </summary>
		public enum UnitTypes
		{
			Corvette,
			SmallTransporter,
			Asteroid,
			Firebird,
			RocketFrigate,
		} // enum UnitTypes

		static readonly float[] DefaultUnitHitpoints = new float[]
			{
				// Corvette,
				400,
				// SmallTransporter,
				300,
				// Asteroid,
				4500,
				// Firebird,
				650,
				// RocketFrigate,
				1500,
			};
		static readonly float[] DefaultCooldownTime = new float[]
			{
				// Corvette,
				275,
				// SmallTransporter,
				0,
				// Asteroid,
				0,
				// Firebird,
				1150,
				// RocketFrigate,
				1850,
			};
		//*unused
		static readonly float[] DefaultUnitDamage = new float[]
			{
				// Corvette,
				10,
				// SmallTransporter,
				0,
				// Asteroid,
				0,
				// Firebird,
				55,
				// RocketFrigate,
				20,
			};
		static readonly float[] DefaultExplosionDamage = new float[]
			{
				// Corvette,
				225,
				// SmallTransporter,
				125,
				// Asteroid,
				250,
				// Firebird,
				275,
				// RocketFrigate,
				500,
			};
		//*/
		static readonly float[] DefaultMaxSpeed = new float[]
			{
				// Corvette,
				13,//20,//50,
				// SmallTransporter,
				16,//25,//60,
				// Asteroid,
				0,//3,
				// Firebird,
				1.5f,//9,//30,
				// RocketFrigate,
				-10,//18,
			};

		/// <summary>
		/// Unit type, for enemy units this is 3-7 (Kamikaze - Mine).
		/// Links to the unit settings list in UnitManager.
		/// </summary>
		public UnitTypes unitType;

		/// <summary>
		/// Life time of this unit in ms, just used for movement pattern.
		/// </summary>
		public float lifeTimeMs;

		/// <summary>
		/// Current hitpoints and max hitpoints, which are copied from
		/// unit settings, but are also dependend on the unit level.
		/// </summary>
		public float hitpoints, maxHitpoints;
		/// <summary>
		/// Shoot time to determinate when we can shoot again.
		/// </summary>
		public float shootTime, cooldownTime;

		/// <summary>
		/// Damage this unit currently does. Copied from unit settings,
		/// but increase as the level advances.
		/// </summary>
		public float damage;

		/// <summary>
		/// Explosion damage, used when we hit something. Used for projectiles,
		/// but also for all units in case we collide with them.
		/// </summary>
		public float explosionDamage;

		/// <summary>
		/// Current speed of this ship. Limited to maxSpeed.
		/// For our own ship this value isn't really used because
		/// we use the shipMovement vector in SpaceCamera, but for enemy ships
		/// this value is very important to speed up/slow down in the unit ai.
		/// </summary>
		public float speed;

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
		public Vector2 position;

		/// <summary>
		/// Movement pattern for simple AI
		/// </summary>
		public enum MovementPattern
		{
			StraightDown,
			GetFasterAndMoveDown,
			SinWave1,
			SinWave2,
			SinWave3,
			CosWave1,
			CosWave2,
			CosWave3,
			SweepLeft,
			SweepRight,
		} // enum MovementPattern

		/// <summary>
		/// Number of possible movement patterns we can choose from.
		/// </summary>
		public const int NumOfMovementPatterns = 10;

		/// <summary>
		/// Movement pattern that is currently used by this unit (handles AI movement).
		/// </summary>
		public MovementPattern movementPattern = MovementPattern.StraightDown;
		#endregion

		#region Constructor
		/// <summary>
		/// Create unit of specific type at specific location.
		/// All other parameters (hitpoints, etc.) are set from UnitManager.
		/// </summary>
		/// <param name="setType">Set type</param>
		/// <param name="setPosition">Set position</param>
		/// <param name="setMovementPattern">Movement pattern</param>
		public Unit(UnitTypes setType, Vector2 setPosition,
			MovementPattern setMovementPattern)
		{
			unitType =
				setType;
				//(Unit.UnitTypes)RandomHelper.GetRandomInt(5);
			position = setPosition;
			movementPattern = setMovementPattern;
			// Don't allow swing left/right for asteroid
			if (unitType == UnitTypes.Asteroid)
				movementPattern = MovementPattern.StraightDown;

			// Recalculate our unit values and reset hitpoints to 100%.
			maxHitpoints = hitpoints = DefaultUnitHitpoints[(int)unitType];
			cooldownTime = DefaultCooldownTime[(int)unitType];
			damage = DefaultUnitDamage[(int)unitType];
			explosionDamage = DefaultExplosionDamage[(int)unitType];
			maxSpeed = DefaultMaxSpeed[(int)unitType];
			shootTime = 0;
			lifeTimeMs = 0;
		} // Unit(setType, setPosition, setLevel)
		#endregion

		#region Render
		/// <summary>
		/// Render unit, returns false if we are done with it.
		/// Has to be removed then. Else it updates just position and AI.
		/// </summary>
		/// <returns>True if done, false otherwise</returns>
		public bool Render(Mission mission)
		{
			#region Update movement with AI
			lifeTimeMs += BaseGame.ElapsedTimeThisFrameInMs;
			float moveSpeed = BaseGame.MoveFactorPerSecond;
			if (Player.GameOver)
				moveSpeed = 0;
			switch (movementPattern)
			{
				case MovementPattern.StraightDown:
					position += new Vector2(0, -1) * maxSpeed * moveSpeed;
					break;
				case MovementPattern.GetFasterAndMoveDown:
					// Out of visible area? Then keep speed slow and wait.
					if (position.Y - Mission.LookAtPosition.Y > 30)
						lifeTimeMs = 300;
					if (lifeTimeMs < 3000)
						speed = lifeTimeMs / 3000;
					position += new Vector2(0, -1) * speed * 1.5f * maxSpeed * moveSpeed;
					break;
				case MovementPattern.SinWave1:
					position += new Vector2(
						(float)Math.Sin(lifeTimeMs / 759.0f),
						-1) * maxSpeed * moveSpeed;
					break;
				case MovementPattern.SinWave2:
					position += new Vector2(
						-(float)Math.Sin(lifeTimeMs / 759.0f),
						-1) * maxSpeed * moveSpeed;
					break;
				case MovementPattern.SinWave3:
					position += new Vector2(
						(float)Math.Sin(lifeTimeMs / 359.0f),
						-1) * maxSpeed * moveSpeed;
					break;
				case MovementPattern.CosWave1:
					position += new Vector2(
						(float)Math.Cos(lifeTimeMs / 759.0f),
						-1) * maxSpeed * moveSpeed;
					break;
				case MovementPattern.CosWave2:
					position += new Vector2(
						-(float)Math.Cos(lifeTimeMs / 759.0f),
						-1) * maxSpeed * moveSpeed;
					break;
				case MovementPattern.CosWave3:
					position += new Vector2(
						(float)Math.Cos(lifeTimeMs / 1759.0f),
						-1) * maxSpeed * moveSpeed;
					break;
				case MovementPattern.SweepLeft:
					position += new Vector2(0, -1) * maxSpeed * moveSpeed;
					if (lifeTimeMs > 1750)
						position += new Vector2(-0.7f, 0) * maxSpeed * moveSpeed;
					break;
				case MovementPattern.SweepRight:
					position += new Vector2(0, -1) * maxSpeed * moveSpeed;
					if (lifeTimeMs > 1750)
						position += new Vector2(+0.7f, 0) * maxSpeed * moveSpeed;
					break;
			} // switch(movementPattern)

			// Keep in bounds
			if (position.X < -Player.MaxXPosition)
				position.X = -Player.MaxXPosition;
			if (position.X > Player.MaxXPosition)
				position.X = Player.MaxXPosition;
			#endregion

			#region Skip if out of visible range
			float distance = Mission.LookAtPosition.Y - position.Y;
			const float MaxUnitDistance = 60;

			// Remove unit if it is out of visible range!
			if (distance > MaxUnitDistance)
				return true;
			bool visible = distance > -35;
			#endregion

			#region Render
			Vector3 shipPos = new Vector3(position, Mission.AllShipsZHeight);
			Mission.ShipModelTypes shipModelType =
				unitType == UnitTypes.Corvette ? Mission.ShipModelTypes.Corvette :
				unitType == UnitTypes.SmallTransporter ? Mission.ShipModelTypes.SmallTransporter :
				unitType == UnitTypes.Firebird ? Mission.ShipModelTypes.Firebird :
				unitType == UnitTypes.RocketFrigate ? Mission.ShipModelTypes.RocketFrigate :
				Mission.ShipModelTypes.Asteroid;
			float shipSize = Mission.ShipModelSize[(int)shipModelType];
			//Note: rotation could be implemented here, but game is fine without
			float shipRotation = 0;
			Matrix rotationMatrix = Matrix.CreateRotationZ(shipRotation);
			if (unitType == UnitTypes.Asteroid)
				rotationMatrix *=
					Matrix.CreateRotationX(position.X / 10 + Player.gameTimeMs / 1539.0f) *
					Matrix.CreateRotationY(position.Y / 20 + Player.gameTimeMs / 1839.0f);
			mission.AddModelToRender(
				mission.shipModels[(int)shipModelType],
				Matrix.CreateScale(shipSize) *
				rotationMatrix *
				Matrix.CreateTranslation(shipPos));
			// Add rocket smoke
			if (unitType != UnitTypes.Asteroid)
				EffectManager.AddRocketOrShipFlareAndSmoke(
					shipPos + shipSize *
					new Vector3(0, 0.6789f + ((int)unitType > 3 ? 0.25f : 0.0f), 0),
					shipSize / 4.0f, 12 * maxSpeed);
			#endregion

			#region Shooting?
			if (Player.GameOver)
				return false;

			// Peng peng?
			bool fireProjectile = false;
			if (cooldownTime > 0 &&
				visible)
			{
				// Ready to shoot again?
				if (shootTime <= 0)
				{
					fireProjectile = true;
					shootTime += cooldownTime;
				} // if (shootTime)
			} // if (GameForm.Mouse.LeftButtonPressed)

			// Weapon needs cooldown time?
			if (shootTime > 0)
				shootTime -= BaseGame.ElapsedTimeThisFrameInMs;

			if (fireProjectile)
			{
				Vector3 shootPos = Vector3.Zero;
				Vector3 enemyUnitPos = Player.shipPos;
				switch (unitType)
				{
					case UnitTypes.Corvette:
						if ((int)(lifeTimeMs / 700) % 3 == 0)
						{
							// Just shoot straight ahead.
							bool hitUnit = Math.Abs(Player.position.X - shipPos.X) < 4.5f &&
								Player.shipPos.Y < shipPos.Y;
							shootPos = shipPos +
							new Vector3(-2.35f, -3.5f, +0.2f);
							EffectManager.AddMgEffect(shootPos,
								hitUnit ? enemyUnitPos : shootPos + new Vector3(0, -40, 0),
								10, 12, hitUnit, false);//shootNum % 2 == 0);
							shootPos = shipPos +
							new Vector3(+2.35f, -3.5f, +0.2f);
							EffectManager.AddMgEffect(shootPos,
								hitUnit ? enemyUnitPos : shootPos + new Vector3(0, -40, 0),
								10, 13, hitUnit, false);
							EffectManager.PlaySoundEffect(
								EffectManager.EffectSoundType.MgShootEnemy);
							if (hitUnit)
							{
								Player.health -= damage / 1000.0f;
							} // if
						} // if
						break;

					case UnitTypes.Firebird:
						shootPos = shipPos + new Vector3(0, -1.5f, 3);
						Mission.AddWeaponProjectile(
							Projectile.WeaponTypes.Fireball, shootPos, false);
						EffectManager.AddFireFlash(shootPos);
						EffectManager.PlaySoundEffect(
							EffectManager.EffectSoundType.EnemyShoot);
						break;

					case UnitTypes.RocketFrigate:
						shootPos = shipPos + new Vector3(
							RandomHelper.GetRandomInt(2) == 0 ? -2.64f : +2.64f, 0, 0);
						Mission.AddWeaponProjectile(
							Projectile.WeaponTypes.Rocket, shootPos, false);
						EffectManager.AddFireFlash(shootPos);
						EffectManager.PlaySoundEffect(
							EffectManager.EffectSoundType.RocketShoot);
						break;
				} // switch
			} // if
			#endregion

			#region Explode if out of hitpoints
			// Destroy ship and damage player if colliding!
			// Near enough to our ship?
			Vector2 distVec =
				new Vector2(Player.shipPos.X, Player.shipPos.Y) -
				new Vector2(position.X, position.Y);
			if (distVec.Length() < 4.75f)//5.5f)
			{
				// Explode and do damage!
				EffectManager.AddFlameExplosion(Player.shipPos);
				Player.health -= explosionDamage / 1000.0f;
				hitpoints = 0;
			} // else

			if (maxHitpoints > 0 &&
				hitpoints <= 0)
			{
				// Explode and kill unit!
				float size = 10 + (int)unitType * 3;
				if (unitType == UnitTypes.Asteroid)
					size = 12;
				EffectManager.AddExplosion(
					new Vector3(position, Mission.AllShipsZHeight),
					size * 0.425f);
					//size * 0.55f);
				return true;
			} // if (hitpoints.X)
			#endregion

			// Don't remove unit from units list
			return false;
		} // Render()
		#endregion

		#region Unit testing
#if DEBUG
		delegate void ResetUnitDelegate(MovementPattern setPattern);
		/// <summary>
		/// Test unit AI movement
		/// </summary>
		public static void TestUnitAI()
		{
			Unit testUnit = null;
			Mission dummyMission = null;

			TestGame.Start("TestUnitAI",
				delegate
				{
					dummyMission = new Mission();
					testUnit = new Unit(UnitTypes.Corvette, Vector2.Zero,
						MovementPattern.StraightDown);
					// Call dummyMission.RenderLandscape once to initialize everything
					dummyMission.RenderLevelBackground(0);
					// Remove the all enemy units (the start enemies)+ all neutral objects
					dummyMission.numOfModelsToRender = 2;
				},
				delegate
				{
					BaseGame.Device.Clear(
						ClearOptions.DepthBuffer | ClearOptions.Target,
						Color.Black, 1.0f, 0);
					TextureFont.WriteText(2, 30,
						"Press 1-0 to test all available movement patterns");
					TextureFont.WriteText(2, 60,
						"Press C, S, F, R, A to switch unit type");
					TextureFont.WriteText(2, 90,
						"Space restarts the current unit");
					TextureFont.WriteText(2, 150,
						"Unit: "+testUnit.unitType);
					TextureFont.WriteText(2, 180,
						"Movement AI: " + testUnit.movementPattern);
					TextureFont.WriteText(2, 210,
						"Position: " + testUnit.position);
					TextureFont.WriteText(2, 240,
						"Speed: " + testUnit.speed);
					TextureFont.WriteText(2, 270,
						"LifeTime: " + (int)(testUnit.lifeTimeMs / 10) / 100.0f);
					ResetUnitDelegate ResetUnit = delegate(MovementPattern setPattern)
						{
							testUnit.movementPattern = setPattern;
							testUnit.position = new Vector2(
								RandomHelper.GetRandomFloat(-20, +20),
								Mission.SegmentLength/2);
							testUnit.hitpoints = testUnit.maxHitpoints;
							testUnit.speed = 0;
							testUnit.lifeTimeMs = 0;
						};
					if (Input.KeyboardKeyJustPressed(Keys.D1))
						ResetUnit(MovementPattern.StraightDown);
					if (Input.KeyboardKeyJustPressed(Keys.D2))
						ResetUnit(MovementPattern.GetFasterAndMoveDown);
					if (Input.KeyboardKeyJustPressed(Keys.D3))
						ResetUnit(MovementPattern.SinWave1);
					if (Input.KeyboardKeyJustPressed(Keys.D4))
						ResetUnit(MovementPattern.SinWave2);
					if (Input.KeyboardKeyJustPressed(Keys.D5))
						ResetUnit(MovementPattern.SinWave3);
					if (Input.KeyboardKeyJustPressed(Keys.D6))
						ResetUnit(MovementPattern.CosWave1);
					if (Input.KeyboardKeyJustPressed(Keys.D7))
						ResetUnit(MovementPattern.CosWave2);
					if (Input.KeyboardKeyJustPressed(Keys.D8))
						ResetUnit(MovementPattern.CosWave3);
					if (Input.KeyboardKeyJustPressed(Keys.D9))
						ResetUnit(MovementPattern.SweepLeft);
					if (Input.KeyboardKeyJustPressed(Keys.D0))
						ResetUnit(MovementPattern.SweepRight);
					if (Input.KeyboardKeyJustPressed(Keys.Space))
						ResetUnit(testUnit.movementPattern);

					if (Input.KeyboardKeyJustPressed(Keys.C))
						testUnit.unitType = UnitTypes.Corvette;
					if (Input.KeyboardKeyJustPressed(Keys.S))
						testUnit.unitType = UnitTypes.SmallTransporter;
					if (Input.KeyboardKeyJustPressed(Keys.F))
						testUnit.unitType = UnitTypes.Firebird;
					if (Input.KeyboardKeyJustPressed(Keys.R))
						testUnit.unitType = UnitTypes.RocketFrigate;
					if (Input.KeyboardKeyJustPressed(Keys.A))
						testUnit.unitType = UnitTypes.Asteroid;

					// Update and render unit
					if (testUnit.Render(dummyMission))
						// Restart unit if it was removed because it was too far down
						ResetUnit(testUnit.movementPattern);

					// Render all models the normal way
					for (int num = 0; num < dummyMission.numOfModelsToRender; num++)
						dummyMission.modelsToRender[num].model.Render(
							dummyMission.modelsToRender[num].matrix);
					BaseGame.MeshRenderManager.Render();
					// Restore number of units as before.
					// Our test unit will be added next frame again.
					dummyMission.numOfModelsToRender = 2;

					// Show all effects (unit smoke, etc.)
					BaseGame.effectManager.HandleAllEffects();
				});
		} // TestUnitAI()
#endif
		#endregion
	} // class Unit
} // namespace XnaShooter.Game
