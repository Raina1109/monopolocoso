// Project: XnaShooter, File: Player.cs
// Namespace: XnaShooter.Game, Class: Player
// Path: C:\code\XnaShooter\Game, Author: Abi
// Code lines: 182, Size of file: 5,41 KB
// Creation date: 02.10.2006 01:33
// Last modified: 27.10.2006 02:40
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using XnaShooter.GameScreens;
using Microsoft.Xna.Framework;
using XnaShooter.Helpers;
using Microsoft.Xna.Framework.Input;
using XnaShooter.Properties;
using XnaShooter.Graphics;
using XnaShooter.Sounds;
#endregion

namespace XnaShooter.Game
{
	/// <summary>
	/// Player helper class, holds all the current game properties:
	/// Health, Weapon and Score.
	/// Note: This is a static class and holds always all player entries
	/// for the current game. If we would have more than 1 player (e.g.
	/// in multiplayer mode) this should not be a static class!
	/// </summary>
	static class Player
	{
		#region Variables
		/// <summary>
		/// Current game time in ms. Used for time display in game. Also used to
		/// update the level position.
		/// </summary>
		public static float gameTimeMs = 0;

		/// <summary>
		/// Won or lost?
		/// </summary>
		public static bool victory = false;

		/// <summary>
		/// Game over?
		/// </summary>
		private static bool gameOver = false;

		/// <summary>
		/// Is game over?
		/// </summary>
		/// <returns>Bool</returns>
		public static bool GameOver
		{
			get
			{
				return gameOver;
			} // get
		} // GameOver

		/// <summary>
		/// Remember if we already uploaded our highscore for this game.
		/// Don't do this twice (e.g. when pressing esc).
		/// </summary>
		static bool alreadyUploadedHighscore = false;

		/// <summary>
		/// Set game over and upload highscore
		/// </summary>
		public static void SetGameOverAndUploadHighscore()
		{
			// Set lifes to 0 and set gameOver to true to mark this game as ended.
			gameOver = true;

			// Upload highscore
			if (alreadyUploadedHighscore == false)
			{
				alreadyUploadedHighscore = true;
				Highscores.SubmitHighscore(score, Highscores.DefaultLevelName);
			} // if (alreadyUploadedHighscore)
		} // SetGameOverAndUploadHighscore()
		#endregion

		#region Current player values (health, weapon, etc.)
		/// <summary>
		/// Health, 1 means we have 100% health, everything below means we
		/// are damaged. If we reach 0, we die!
		/// </summary>
		public static float health = 1.0f;

		/// <summary>
		/// Weapon types we can carry with our ship
		/// </summary>
		public enum WeaponTypes
		{
			MG,
			Plasma,
			Gattling,
			Rockets,
		} // enum WeaponTypes

		/// <summary>
		/// Weapon we currently have, each weapon is replaced by the
		/// last collected one. No ammunition is used.
		/// </summary>
		public static WeaponTypes currentWeapon = WeaponTypes.MG;

		/// <summary>
		/// Do we have the EMP bomb? Press space to fire them.
		/// </summary>
		public static int empBombs = 0;

		/// <summary>
		/// Position where EMP was fired!
		/// </summary>
		public static Vector3 empPosition = Vector3.Zero;
		/// <summary>
		/// After firing the EMP bomb will deploy over the screen in about 1 sec.
		/// </summary>
		public static float empBombTimeout = 0;
		/// <summary>
		/// Time for an EMP bomb to explode everything on screen.
		/// </summary>
		const int MaxEmpTimeout = 1000;

		/// <summary>
		/// Current score. Used as highscore if game is over.
		/// </summary>
		public static int score = 0;

		/*unused
		/// <summary>
		/// Max. value for camera wobbel timeout.
		/// </summary>
		public const int MaxCameraWobbelTimeoutMs = 700;

		/*unused
		/// <summary>
		/// Camera wobbel timeout.
		/// Used to shake camera after colliding or nearly hitting asteroids.
		/// </summary>
		public static float cameraWobbelTimeoutMs = 0;

		/// <summary>
		/// Camera wobbel factor.
		/// </summary>
		public static float cameraWobbelFactor = 1.0f;

		/// <summary>
		/// Set camera wobbel
		/// </summary>
		/// <param name="factor">Factor</param>
		public static void SetCameraWobbel(float wobbelFactor)
		{
			cameraWobbelTimeoutMs = (int)
				//((0.75f + 0.5f * wobbelFactor) *
				(MaxCameraWobbelTimeoutMs);
			cameraWobbelFactor = wobbelFactor;
		} // SetCameraWobbel(wobbelFactor)
		 */

		/// <summary>
		/// Movement speed per second, used in SpaceCamera class.
		/// 500 means we will move up to 5 sectors a second, which is really fast.
		/// </summary>
		public const float MovementSpeedPerSecond = 88;//111;//1234;//500.0f;// 500.0f;

		/// <summary>
		/// Max values for x,y position (positive or negative)
		/// </summary>
		public const float MaxXPosition = 28.25f,
			MaxYPosition = 26.75f;
		/// <summary>
		/// Our current ship position.
		/// </summary>
		public static Vector2 position = new Vector2(0, -MaxYPosition);

		/// <summary>
		/// Ship rotation in x and y directions to make it look cooler.
		/// </summary>
		public static Vector2 shipRotation = new Vector2(0, 0);

		/// <summary>
		/// Ship position in 3d, updated each frame in Mission.RenderLandscapeObjects
		/// </summary>
		public static Vector3 shipPos = new Vector3(0, 0, 0);

		/// <summary>
		/// Shoot number, will be increased each time we shoot.
		/// </summary>
		public static int shootNum = 0;

		/// <summary>
		/// Remember the last time we shoot to allow instant shooting when clicking.
		/// </summary>
		private static float lastShootTimeMs = -500;
		#endregion

		#region Reset everything for starting a new game
		/// <summary>
		/// Keys for moving around. Assigned from settings!
		/// </summary>
		static Keys moveLeftKey,
			moveRightKey,
			moveUpKey,
			moveDownKey;

		/// <summary>
		/// Default to normal mouse sensibility, can be changed
		/// from 0.5 to 2.0.
		/// </summary>
		static float mouseSensibility = 1.0f;

		/// <summary>
		/// Reset all player entries for restarting a game.
		/// </summary>
		public static void Reset()
		{
			gameOver = false;
			alreadyUploadedHighscore = false;
			gameTimeMs = 0;
			health = 1.0f;
			score = 0;
			//unused: cameraWobbelTimeoutMs = 0;
			lastShootTimeMs = -500;
			position = new Vector2(0, -MaxYPosition);
			shipRotation = new Vector2(0, 0);
			shipPos = new Vector3(0, 0, 0);
			shootNum = 0;
			currentWeapon = WeaponTypes.MG;
			empBombs = 0;

			// Assign keys. Warning: This is VERY slow, never use it
			// inside any render loop (getting Settings, etc.)!
			moveLeftKey = GameSettings.Default.MoveLeftKey;
			moveRightKey = GameSettings.Default.MoveRightKey;
			moveUpKey = GameSettings.Default.MoveForwardKey;
			moveDownKey = GameSettings.Default.MoveBackwardKey;

			// Also assign mouse sensibility
			mouseSensibility = 2.5f -
				2.0f * GameSettings.Default.ControllerSensibility;
			if (mouseSensibility < 0.5f)
				mouseSensibility = 0.5f;

			// Reset light position (make sure it still comes from the same direction)
			BaseGame.LightDirection = new Vector3(2, -7, 5);
		} // Reset(setLevelName)
		#endregion

		#region Handle game logic
		/// <summary>
		/// Handle game logic
		/// </summary>
		public static void HandleGameLogic(Mission mission)
		{
			//tst: if (Input.KeyboardF1JustPressed)
			//	gameOver = true;

			// Don't handle any more game logic if game is over.
			if (Player.GameOver)
			{
				if (victory)
				{
					// Display Victory message
					TextureFont.WriteTextCentered(
						BaseGame.Width / 2, BaseGame.Height / 3,
						"Victory! You won.");
					TextureFont.WriteTextCentered(
						BaseGame.Width / 2, BaseGame.Height / 3 + 40,
						"Your Highscore: " + Player.score +
						" (#" + Highscores.GetRankFromCurrentScore(Player.score) + ")");
				} // if
				else
				{
					// Display game over message
					TextureFont.WriteTextCentered(
						BaseGame.Width / 2, BaseGame.Height / 3,
						"Game Over! You lost.");
					TextureFont.WriteTextCentered(
						BaseGame.Width / 2, BaseGame.Height / 3 + 40,
						"Your Highscore: " + Player.score +
						" (#" + Highscores.GetRankFromCurrentScore(Player.score) + ")");
				} // else

				return;
			} // if

			// Increase game time
			gameTimeMs += BaseGame.ElapsedTimeThisFrameInMs;
			
			// Control our ship position with the keyboard or gamepad.
			// Use keyboard cursor keys and the left thumb stick. The
			// right hand is used for fireing (ctrl, space, a, b).
			Vector2 lastPosition = position;
			Vector2 lastRotation = shipRotation;
			float moveFactor = mouseSensibility *
				MovementSpeedPerSecond * BaseGame.MoveFactorPerSecond;
			// Left/Right
			if (Input.Keyboard.IsKeyDown(moveLeftKey) ||
				Input.Keyboard.IsKeyDown(Keys.Left) ||
				Input.Keyboard.IsKeyDown(Keys.NumPad4) ||
				Input.GamePad.DPad.Left == ButtonState.Pressed)
			{
				position.X -= moveFactor;
			} // if
			if (Input.Keyboard.IsKeyDown(moveRightKey) ||
				Input.Keyboard.IsKeyDown(Keys.Right) ||
				Input.Keyboard.IsKeyDown(Keys.NumPad6) ||
				Input.GamePad.DPad.Right == ButtonState.Pressed)
			{
				position.X += moveFactor;
			} // if
			if (Input.GamePad.ThumbSticks.Left.X != 0.0f)
			{
				position.X += Input.GamePad.ThumbSticks.Left.X;// *0.75f;
			} // if

			// Down/Up
			if (Input.Keyboard.IsKeyDown(moveDownKey) ||
				Input.Keyboard.IsKeyDown(Keys.Down) ||
				Input.Keyboard.IsKeyDown(Keys.NumPad2) ||
				Input.GamePad.DPad.Down == ButtonState.Pressed)
			{
				position.Y -= moveFactor;
			} // if
			if (Input.Keyboard.IsKeyDown(moveUpKey) ||
				Input.Keyboard.IsKeyDown(Keys.Up) ||
				Input.Keyboard.IsKeyDown(Keys.NumPad8) ||
				Input.GamePad.DPad.Up == ButtonState.Pressed)
			{
				position.Y += moveFactor;
			} // if
			if (Input.GamePad.ThumbSticks.Left.Y != 0.0f)
			{
				position.Y += Input.GamePad.ThumbSticks.Left.Y;// *0.75f;
			} // if

			// Keep position in bounds
			if (position.X < -MaxXPosition)
				position.X = -MaxXPosition;
			if (position.X > MaxXPosition)
				position.X = MaxXPosition;
			if (position.Y < -MaxYPosition)
				position.Y = -MaxYPosition;
			if (position.Y > MaxYPosition)
				position.Y = MaxYPosition;

			// Calculate ship rotation based on the current movement
			if (lastPosition.X > position.X)
				shipRotation.X = -0.5f;
			else if (lastPosition.X < position.X)
				shipRotation.X = +0.5f;
			else
				shipRotation.X = 0;

			if (lastPosition.Y > position.Y)
				shipRotation.Y = +0.53f;
			else if (lastPosition.Y < position.Y)
				shipRotation.Y = -0.33f;
			else
				shipRotation.Y = 0;
			shipRotation = lastRotation * 0.95f + shipRotation * 0.05f;

			// Fire?
			if (Input.GamePadAPressed ||
				Input.GamePad.Triggers.Right > 0.5f ||
				Input.Keyboard.IsKeyDown(Keys.LeftControl) ||
				Input.Keyboard.IsKeyDown(Keys.RightControl))
			{
				switch (currentWeapon)
				{
					case WeaponTypes.MG:
						if (gameTimeMs - lastShootTimeMs >= 150)
						{
							bool hitUnit = false;
							Vector3 enemyUnitPos = new Vector3(0, 100000, 0);
							// Hit enemy units, check all of them
							for (int num = 0; num < Mission.units.Count; num++)
							{
								Unit enemyUnit = Mission.units[num];
								// Near enough to enemy ship?
								if (Math.Abs(enemyUnit.position.X - shipPos.X) < 5.0f &&
									enemyUnit.position.Y > shipPos.Y &&
									(enemyUnit.position.Y - shipPos.Y) < 60)
								{
									// Hit and do damage!
									if (enemyUnit.position.Y < enemyUnitPos.Y)
										enemyUnitPos = new Vector3(enemyUnit.position, Mission.AllShipsZHeight);
									hitUnit = true;
									Player.score += (int)enemyUnit.hitpoints / 10;
									enemyUnit.hitpoints -= 150; // do about 1000 damage/sec
								} // if
							} // for

							Vector3 shootPos = shipPos + new Vector3(-1.2f, 0, 0);
							EffectManager.AddMgEffect(shootPos,
								hitUnit ? enemyUnitPos : shootPos + new Vector3(0, 100, 0),
								0, 2, hitUnit, true);//shootNum % 2 == 0);
							shootPos = shipPos + new Vector3(+1.2f, 0, 0);
							EffectManager.AddMgEffect(shootPos,
								hitUnit ? enemyUnitPos : shootPos + new Vector3(0, 100, 0),
								1, 3, hitUnit, false);
							shootNum++;
							lastShootTimeMs = gameTimeMs;
							Input.GamePadRumble(0.015f, 0.125f);
							Player.score += 1;
						} // if
						else
						{
							Vector3 shootPos = shipPos + new Vector3(-1.2f, 0, 0);
							EffectManager.UpdateMgEffect(shootPos, 0);
							shootPos = shipPos + new Vector3(+1.2f, 0, 0);
							EffectManager.UpdateMgEffect(shootPos, 1);
						} // else
						break;
					case WeaponTypes.Plasma:
						if (gameTimeMs - lastShootTimeMs >= 250)
						{
							Vector3 shootPos = shipPos +
								new Vector3(shootNum % 2 == 0 ? -2.4f : +2.4f, 0, 0);
							Mission.AddWeaponProjectile(
								Projectile.WeaponTypes.Plasma, shootPos, true);
							EffectManager.AddFireFlash(shootPos);
							EffectManager.PlaySoundEffect(
								EffectManager.EffectSoundType.PlasmaShoot);
							shootNum++;
							lastShootTimeMs = gameTimeMs;
							Input.GamePadRumble(0.05f, 0.175f);
							Player.score += 5;
						} // if
						break;
					case WeaponTypes.Gattling:
						if (gameTimeMs - lastShootTimeMs >= 100)
						{
							bool hitUnit = false;
							Vector3 enemyUnitPos = new Vector3(0, 100000, 0);
							// Hit enemy units, check all of them
							for (int num = 0; num < Mission.units.Count; num++)
							{
								Unit enemyUnit = Mission.units[num];
								// Near enough to enemy ship?
								if (Math.Abs(enemyUnit.position.X - shipPos.X) < 5.0f &&
									enemyUnit.position.Y > shipPos.Y &&
									(enemyUnit.position.Y - shipPos.Y) < 60)
								{
									// Hit and do damage!
									if (enemyUnit.position.Y < enemyUnitPos.Y)
										enemyUnitPos = new Vector3(enemyUnit.position, Mission.AllShipsZHeight);
									hitUnit = true;
									Player.score += (int)enemyUnit.hitpoints / 10;
									enemyUnit.hitpoints -= 175; // do about 1750 damage/sec
								} // if
							} // for

							Vector3 shootPos = shipPos + new Vector3(-1.2f, 0, 0);
							EffectManager.AddGattlingEffect(shootPos,
								hitUnit ? enemyUnitPos : shootPos + new Vector3(0, 100, 0),
								0, 2, hitUnit, shootNum % 2 == 0);
							shootPos = shipPos + new Vector3(+1.2f, 0, 0);
							EffectManager.AddGattlingEffect(shootPos,
								hitUnit ? enemyUnitPos : shootPos + new Vector3(0, 100, 0),
								1, 3, hitUnit, false);
							if (RandomHelper.GetRandomInt(2) == 0)
							{
								shootPos = shipPos + new Vector3(-2.54f, 0, 0);
								EffectManager.AddGattlingEffect(shootPos,
									hitUnit ? enemyUnitPos : shootPos + new Vector3(0, 100, 0),
									4, 2, hitUnit, false);
								shootPos = shipPos + new Vector3(+2.54f, 0, 0);
								EffectManager.AddGattlingEffect(shootPos,
									hitUnit ? enemyUnitPos : shootPos + new Vector3(0, 100, 0),
									5, 2, hitUnit, false);
							} // if
							shootNum++;
							lastShootTimeMs = gameTimeMs;
							Input.GamePadRumble(0.05f, 0.225f);
							Player.score += 1;
						} // if
						break;
					case WeaponTypes.Rockets:
						if (gameTimeMs - lastShootTimeMs >= 500)
						{
							Vector3 shootPos = shipPos +
								new Vector3(shootNum % 2 == 0 ? -2.64f : +2.64f, 0, 0);
							Mission.AddWeaponProjectile(
								Projectile.WeaponTypes.Rocket, shootPos, true);
							EffectManager.AddFireFlash(shootPos);
							EffectManager.PlaySoundEffect(
								EffectManager.EffectSoundType.RocketShoot);
							shootNum++;
							lastShootTimeMs = gameTimeMs;
							Input.GamePadRumble(0.10f, 0.15f);
							Player.score += 10;
						} // if
						break;
				} // switch
			} // if

			// Fire EMP?
			if (empBombs > 0 &&
				empBombTimeout <= 0 &&
				(Input.GamePadBJustPressed ||
				Input.KeyboardSpaceJustPressed))
			{
				empBombs--;
				empBombTimeout = MaxEmpTimeout;
				empPosition = shipPos + new Vector3(0, 10, 0);
				
				// Give some extra points to player
				Player.score += 1000;
				
				// Big badda boom
				//tst: EffectManager.AddExplosion(empPosition, 10);
				for (int num=0; num<5; num++)
					EffectManager.AddEffect(shipPos+ new Vector3(0, 5, 0),
						EffectManager.EffectType.ExplosionRing, 2+2.5f*num, 0);
				Sound.Play(Sound.Sounds.EMP);
			} // if

			if (empBombTimeout > 0)
			{
				empBombTimeout -= BaseGame.ElapsedTimeThisFrameInMs;

				if (empBombTimeout <= 0)
					empBombTimeout = 0;

				// EMP Distance
				float distance = 64 *
					(MaxEmpTimeout - empBombTimeout) / MaxEmpTimeout;
				
				// Show effect
				BaseGame.DrawLine(
					empPosition +new Vector3(-100, distance, 0),
					empPosition +new Vector3(+100, distance, 0));
				BaseGame.DrawLine(
					empPosition +new Vector3(-100, -distance, 0),
					empPosition +new Vector3(+100, -distance, 0));
				BaseGame.DrawLine(
					empPosition +new Vector3(distance, -100, 0),
					empPosition +new Vector3(distance, +100, 0));
				BaseGame.DrawLine(
					empPosition +new Vector3(-distance, -100, 0),
					empPosition +new Vector3(-distance, +100, 0));

				// Kill all units on screen that are in range!
				for (int num = 0; num < Mission.units.Count; num++)
				{
					// Only include units on the screen and don't kill asteroids.
					// Projectiles and effects stay active too.
					if ((Mission.units[num].position -
						new Vector2(empPosition.X, empPosition.Y)).Length() < distance + 10)// &&
						//Mission.units[num].position.Y - Mission.LookAtPosition.Y < 15 &&
						//looks cooler with: Mission.units[num].unitType != Unit.UnitTypes.Asteroid)
					{
						Player.score += (int)Mission.units[num].hitpoints / 10;
						Mission.units[num].hitpoints = 0;
					} // if
				} // for
			} // if (emp bomb active)

			if (Player.health <= 0)
			{
				victory = false;
				EffectManager.AddExplosion(shipPos, 20);
				for (int num = 0; num<8; num++)
					EffectManager.AddFlameExplosion(shipPos+
						RandomHelper.GetRandomVector3(-12, +12), false);
				Sound.PlayDefeatSound();
				Player.SetGameOverAndUploadHighscore();
			} // if
		} // HandleGameLogic(asteroidManager)
		#endregion
	} // class Player
} // namespace XnaShooter
