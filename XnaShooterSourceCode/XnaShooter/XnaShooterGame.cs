// Project: XnaShooter, File: XnaShooterGame.cs
// Namespace: XnaShooter, Class: XnaShooterGame
// Path: C:\code\XnaShooter\, Author: Abi
// Code lines: 182, Size of file: 5,41 KB
// Creation date: 02.10.2006 01:33
// Last modified: 27.10.2006 02:40
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using XnaShooter.Game;
using XnaShooter.Helpers;
using XnaShooter.GameScreens;
using XnaShooter.Sounds;
using XnaShooter.Graphics;
using XnaShooter.Properties;
using Texture = XnaShooter.Graphics.Texture;
using Model = XnaShooter.Graphics.Model;
#endregion

namespace XnaShooter
{
	/// <summary>
	/// XnaShooterGame main class
	/// </summary>
	public class XnaShooterGame : BaseGame
	{
		#region Variables
		/// <summary>
		/// Game screens stack. We can easily add and remove game screens
		/// and they follow the game logic automatically. Very cool.
		/// </summary>
		private Stack<IGameScreen> gameScreens = new Stack<IGameScreen>();

		/// <summary>
		/// Load the main menu and mouse textures at game start, this way
		/// we have access in all game screens and don't ever have to reload
		/// any of these textures.
		/// </summary>
		public Texture mainMenuTexture = null,
			mouseCursorTexture = null;

		/// <summary>
		/// Also preload the in game textures for all in game controls and stuff.
		/// Note: These Hud textures are saved in png format to save disk space.
		/// Light effect texture is used to show items from a far distance.
		/// </summary>
		public Texture hudTopTexture = null,
			hudBottomTexture = null;

		/// <summary>
		/// Background landscape models and ship models.
		/// </summary>
		public Model[] landscapeModels = null,
			shipModels = null,
			itemModels = null;

		/*obs
		/// <summary>
		/// Explosion texture, displayed when any ship is exploding.
		/// </summary>
		public AnimatedTexture explosionTexture = null;
		 */
		#endregion

		#region Constructor
		/// <summary>
		/// Create your game
		/// </summary>
		public XnaShooterGame()
		{
			// Disable mouse, we use our own mouse texture in the menu
			// and don't use any mouse cursor in the game anyway.
			this.IsMouseVisible = false;
		} // XnaShooterGame()
		#endregion

		#region Initialize
		/// <summary>
		/// Allows the game to perform any initialization it needs.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			// Make sure mouse is centered
			Input.Update();
			Input.MousePos = new Point(
				Window.ClientBounds.X + width / 2,
				Window.ClientBounds.Y + height / 2);
			Input.Update();

			// Load menu textures
			mainMenuTexture = new Texture("MainMenu");
			mouseCursorTexture = new Texture("MouseCursor");
			hudTopTexture = new Texture("HudTop");
			hudBottomTexture = new Texture("HudBottom");

			// Load explosion effect
			//explosionTexture = new AnimatedTexture("Destroy");

			// Load background landscape and wall models
			landscapeModels = new Model[]
				{
					new Model("BackgroundGround"),
					new Model("Building"),
					new Model("Building2"),
					new Model("Building3"),
					new Model("Building4"),
					new Model("Building5"),
					new Model("Kaktus"),
					new Model("Kaktus2"),
					new Model("KaktusBenny"),
					new Model("KaktusSeg"),
				};
			shipModels = new Model[]
				{
					new Model("OwnShip"),
					new Model("Corvette"),
					new Model("SmallTransporter"),
					new Model("Firebird"),
					new Model("RocketFrigate"),
					new Model("Rocket"),
					new Model("Asteroid"),
				};
			itemModels = new Model[]
				{
					new Model("ItemHealth"),
					new Model("ItemMg"),
					new Model("ItemGattling"),
					new Model("ItemPlasma"),
					new Model("ItemRockets"),
					new Model("ItemEmp"),
				};

			// Create main menu screen
			gameScreens.Push(new MainMenu());
			// Start game
			//gameScreens.Push(new Mission());
			//inGame = true;
			
			// Start music
			Sound.StartMusic();
		} // Initialize()
		#endregion

		#region Dispose
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
				GameSettings.Save();
		} // Dispose(disposing)
		#endregion

		#region Toggle music on/off
		/// <summary>
		/// Toggle music on off
		/// </summary>
		public void ToggleMusicOnOff()
		{
			if (GameSettings.Default.MusicOn)
				Sound.StopMusic();
			else
				Sound.StartMusic();
		} // ToggleMusicOnOff()
		#endregion

		#region Add game screen
		static bool inGame = false;
		/// <summary>
		/// In game
		/// </summary>
		/// <returns>Bool</returns>
		public static bool InGame
		{
			get
			{
				return inGame;
			} // get
		} // InGame

		/// <summary>
		/// Add game screen, which will be used until we quit it or add
		/// another game screen on top of it.
		/// </summary>
		/// <param name="newGameScreen">New game screen</param>
		public void AddGameScreen(IGameScreen newGameScreen)
		{
			gameScreens.Push(newGameScreen);

			inGame = newGameScreen.GetType() == typeof(Mission);
		} // AddGameScreen(newGameScreen)
		#endregion

		#region Remove current game screen
		/// <summary>
		/// Remove current game screen
		/// </summary>
		public void RemoveCurrentGameScreen()
		{
			gameScreens.Pop();

			inGame = gameScreens.Count > 0 &&
				gameScreens.Peek().GetType() == typeof(Mission);
		} // RemoveCurrentGameScreen()
		#endregion

		#region Render menu background
		/// <summary>
		/// Render menu background
		/// </summary>
		public void RenderMenuBackground()
		{
			// Make sure alpha blending is enabled.
			BaseGame.EnableAlphaBlending();
			mainMenuTexture.RenderOnScreen(
				BaseGame.ResolutionRect,
				new Rectangle(0, 0, 1024, 768));
		} // RenderMenuBackground()
		#endregion

		#region Render button
		/// <summary>
		/// Render button
		/// </summary>
		/// <param name="buttonType">Button type</param>
		/// <param name="rect">Rectangle</param>
		public bool RenderMenuButton(
			MenuButton buttonType, Point pos)
		{
			// Calc screen rect for rendering (recalculate relative screen position
			// from 1024x768 to actual screen resolution, just in case ^^).
			Rectangle rect = new Rectangle(
				pos.X * BaseGame.Width / 1024,
				pos.Y * BaseGame.Height / 768,
				200 * BaseGame.Width / 1024,
				77 * BaseGame.Height / 768);

			// Is button highlighted?
			Rectangle innerRect = new Rectangle(
				rect.X, rect.Y + rect.Height / 5,
				rect.Width, rect.Height * 3 / 5);
			bool highlight = Input.MouseInBox(
				//rect);
				// Just use inner rect
				innerRect);

			// Was not highlighted last frame?
			if (highlight &&
				Input.MouseWasNotInRectLastFrame(innerRect))
				Sound.Play(Sound.Sounds.Highlight);

			// See MainMenu.dds for pixel locations
			int buttonNum = (int)buttonType;

			// Correct last 2 button numbers (exit and back)
			//if (buttonNum >= (int)MenuButton.Exit)
			//	buttonNum -= 2;

			Rectangle pixelRect = new Rectangle(3 + 204 * buttonNum,
				840 + 80 * (highlight ? 1 : 0), 200, 77);

			// Render
			mainMenuTexture.RenderOnScreen(rect, pixelRect);

			// Play click sound if button was just clicked
			bool ret =
				(Input.MouseLeftButtonJustPressed ||
				Input.GamePadAJustPressed ||
				Input.KeyboardSpaceJustPressed) &&
				this.IsActive &&
				highlight;

			if (buttonType == MenuButton.Back &&
				(Input.GamePadBackJustPressed ||
				Input.KeyboardEscapeJustPressed))
				ret = true;
			if (buttonType == MenuButton.Missions &&
				Input.GamePadStartPressed)
				ret = true;

			if (ret == true)
				Sound.Play(Sound.Sounds.Click);

			// Return true if button was pressed, false otherwise
			return ret;
		} // RenderButton(buttonType, rect)
		#endregion

		#region Render mouse cursor
		/// <summary>
		/// Render mouse cursor
		/// </summary>
		public void RenderMouseCursor()
		{
#if !XBOX360
			// We got 4 animation steps, rotate them by the current time
			int mouseAnimationStep = (int)(BaseGame.TotalTimeMs / 100) % 4;

			// And render mouse on screen.
			mouseCursorTexture.RenderOnScreen(
				new Rectangle(Input.MousePos.X, Input.MousePos.Y, 60*2/3, 64*2/3),
				new Rectangle(64 * mouseAnimationStep, 0, 60, 64));

			// Draw all sprites (just the mouse cursor)
			SpriteHelper.DrawSprites(width, height);
#endif
		} // RenderMouseCursor()
		#endregion

		#region Update
		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// If that game screen should be quitted, remove it from stack!
			if (gameScreens.Count > 0 &&
				gameScreens.Peek().Quit)
				RemoveCurrentGameScreen();

			// If no more game screens are left, it is time to quit!
			if (gameScreens.Count == 0)
			{
#if DEBUG
				// Don't exit if this is just a unit test
				if (this.GetType() != typeof(TestGame))
#endif
					Exit();
			} // if

			base.Update(gameTime);
		} // Update(gameTime)
		#endregion

		#region Draw
		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			// Kill background (including z buffer, which is important for 3D)
			ClearBackground();

			// Start post screen glow shader, will be shown in BaseGame.Draw
			BaseGame.GlowShader.Start();

			// Disable z buffer, mostly now only 2d content is rendered now.
			//BaseGame.Device.RenderState.DepthBufferEnable = false;

#if !DEBUG
			try
			{
#endif
				// Execute the game screen on top.
				if (gameScreens.Count > 0)
					gameScreens.Peek().Run(this);
#if !DEBUG
			} // try
			catch (Exception ex)
			{
				Log.Write("Failed to execute " + gameScreens.Peek().Name +
					"\nError: " + ex.ToString());
			} // catch
#endif

			base.Draw(gameTime);

			// Show mouse cursor (in all modes except in the game)
			if (inGame == false &&
				gameScreens.Count > 0)
				RenderMouseCursor();
			else
			{
				// In game always center mouse
				Input.CenterMouse();
			} // else

			// Add scene glow on top of everything (2d and 3d!) 
			glowShader.Show();
		} // Draw(gameTime)
		#endregion
	} // class XnaShooterGame
} // namespace XnaShooter
