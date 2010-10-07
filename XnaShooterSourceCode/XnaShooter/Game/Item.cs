// Project: XnaShooter, File: Item.cs
// Namespace: XnaShooter.Game, Class: Item
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
using XnaShooter.Sounds;
#endregion

namespace XnaShooter.Game
{
	public class Item
	{
		#region Variables
		/// <summary>
		/// Item types
		/// </summary>
		public enum ItemTypes
		{
			Health,
			Mg,
			Gattling,
			Plasma,
			Rockets,
			Emp,
		} // enum ItemTypes

		/// <summary>
		/// Item type, either health or a weapon.
		/// </summary>
		public ItemTypes itemType;

		/// <summary>
		/// Position for this item. Will not move or anything, just collect them.
		/// </summary>
		public Vector2 position;
		#endregion
		
		#region Constructor
		/// <summary>
		/// Create item of specific type at specific location.
		/// </summary>
		/// <param name="setType">Set type</param>
		/// <param name="setPosition">Set position</param>
		public Item(ItemTypes setType, Vector2 setPosition)
		{
			itemType = setType;
			position = setPosition;
		} // Unit(setType, setPosition)
		#endregion

		#region Render
		/// <summary>
		/// Render unit, returns false if we are done with it.
		/// Has to be removed then. Else it updates just position and AI.
		/// </summary>
		/// <returns>True if done, false otherwise</returns>
		public bool Render(Mission mission)
		{
			#region Skip if out of visible range
			float distance = Mission.LookAtPosition.Y - position.Y;
			const float MaxUnitDistance = 60;

			// Remove unit if it is out of visible range!
			if (distance > MaxUnitDistance)
				return true;
			#endregion

			#region Render
			float itemSize = Mission.ItemModelSize;
			float itemRotation = 0;
			Vector3 itemPos = new Vector3(position, Mission.AllShipsZHeight);
			mission.AddModelToRender(
				mission.itemModels[(int)itemType],
				Matrix.CreateScale(itemSize) *
				Matrix.CreateRotationZ(itemRotation) *
				Matrix.CreateTranslation(itemPos));
			// Add glow effect
			EffectManager.AddEffect(itemPos + new Vector3(0, 0, 1.01f),
				EffectManager.EffectType.LightInstant,
				7.5f, 0, 0);
			EffectManager.AddEffect(itemPos + new Vector3(0, 0, 1.02f),
				EffectManager.EffectType.LightInstant,
				5.0f, 0, 0);
			#endregion

			#region Collect
			// Collect item and give to player if colliding!
			Vector2 distVec =
				new Vector2(Player.shipPos.X, Player.shipPos.Y) -
				new Vector2(position.X, position.Y);
			if (distVec.Length() < 5.0f)
			{
				if (itemType == ItemTypes.Health)
				{
					// Refresh health
					Sound.Play(Sound.Sounds.Health);
					Player.health = 1.0f;
				} // if
				else
				{
					Sound.Play(Sound.Sounds.NewWeapon);
					if (itemType == ItemTypes.Mg)
						Player.currentWeapon = Player.WeaponTypes.MG;
					else if (itemType == ItemTypes.Plasma)
						Player.currentWeapon = Player.WeaponTypes.Plasma;
					else if (itemType == ItemTypes.Gattling)
						Player.currentWeapon = Player.WeaponTypes.Gattling;
					else if (itemType == ItemTypes.Rockets)
						Player.currentWeapon = Player.WeaponTypes.Rockets;
					else if (itemType == ItemTypes.Emp &&
						Player.empBombs < 5)
						Player.empBombs++;
				} // else
				Player.score += 500;

				return true;
			} // else
			#endregion

			// Don't remove unit from items list
			return false;
		} // Render()
		#endregion
	} // class Item
} // namespace XnaShooter.Game
