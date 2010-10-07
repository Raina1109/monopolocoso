// Project: XnaShooter, File: PostScreenMenu.cs
// Namespace: XnaShooter.Shaders, Class: PostScreenMenu
// Path: C:\code\XnaShooter\Shaders, Author: Abi
// Code lines: 369, Size of file: 10,70 KB
// Creation date: 27.09.2006 03:46
// Last modified: 15.10.2006 19:59
// Generated with Commenter by abi.exDream.com

#region Using directives
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.IO;
using System.Text;
using XnaShooter.Game;
using XnaShooter.Graphics;
using XnaShooter.Helpers;
using Texture = XnaShooter.Graphics.Texture;
using Model = XnaShooter.Graphics.Model;
#endregion

namespace XnaShooter.Shaders
{
	/// <summary>
	/// Post screen glow shader based on PostScreenMenu.fx
	/// </summary>
	/// <returns>Shader xnaEffect</returns>
	public class PostScreenDarkenBorder : ShaderEffect
	{
		#region Variables
		/// <summary>
		/// The shader xnaEffect filename for this shader.
		/// </summary>
		private const string Filename = "PostScreenDarkenBorder.fx";

		/// <summary>
		/// Effect handles for window size and scene map.
		/// </summary>
		protected EffectParameter windowSize,
			sceneMap,
			screenBorderFadeoutMap;

		/// <summary>
		/// Links to the passTextures, easier to write code this way.
		/// This are just reference copies. Static to load them only once
		/// (used for both PostScreenMenu and PostScreenGlow).
		/// </summary>
		protected static RenderToTexture sceneMapTexture;

		/// <summary>
		/// Helper texture for the screen border (darken the borders).
		/// </summary>
		private Texture screenBorderFadeoutMapTexture = null;

		/// <summary>
		/// Is this post screen shader started?
		/// Else don't execute Show if it is called.
		/// </summary>
		protected static bool started = false;

		/// <summary>
		/// Started
		/// </summary>
		/// <returns>Bool</returns>
		public bool Started
		{
			get
			{
				return started;
			} // get
		} // Started
		#endregion

		#region Constructor
		/// <summary>
		/// Create post screen menu. Also used for the constructor of
		/// PostScreenGlow (same RenderToTextures used there).
		/// </summary>
		protected PostScreenDarkenBorder(string shaderFilename)
			: base(shaderFilename)
		{
			Load();
		} // PostScreenDarkenBorder()

		/// <summary>
		/// Create post screen menu
		/// </summary>
		public PostScreenDarkenBorder()
			: this(Filename)
		{
		} // PostScreenDarkenBorder()
		#endregion

		#region Dispose
		/// <summary>
		/// Dispose
		/// </summary>
		public override void Dispose()
		{
			base.Dispose();
			if (sceneMapTexture != null)
				sceneMapTexture.Dispose();
			sceneMapTexture = null;
		} // Dispose()
		#endregion

		#region Load
		/// <summary>
		/// Load in case the device got lost.
		/// </summary>
		public override void Load()
		{
			base.Load();
			// Scene map texture
			if (sceneMapTexture == null)
				sceneMapTexture = new RenderToTexture(
					RenderToTexture.SizeType.FullScreen);
		} // Load()
		#endregion

		#region Get parameters
		/// <summary>
		/// Reload
		/// </summary>
		protected override void GetParameters()
		{
			// Can't get parameters if loading failed!
			if (xnaEffect == null)
				return;

			windowSize = xnaEffect.Parameters["windowSize"];
			sceneMap = xnaEffect.Parameters["sceneMap"];

			// We need both windowSize and sceneMap.
			if (windowSize == null ||
				sceneMap == null)
				throw new NotSupportedException("windowSize and sceneMap must be " +
					"valid in PostScreenShader=" + Filename);

			// Load screen border texture
			screenBorderFadeoutMap = xnaEffect.Parameters["screenBorderFadeoutMap"];
			screenBorderFadeoutMapTexture = new Texture("ScreenBorderFadeout.dds");
			// Set texture
			screenBorderFadeoutMap.SetValue(
				screenBorderFadeoutMapTexture.XnaTexture);
		} // GetParameters()
		#endregion

		#region Start
		/// <summary>
		/// Remember back buffer surface
		/// </summary>
		DepthStencilBuffer remBackBufferSurface = null;
		/// <summary>
		/// Start this post screen shader, will just call SetRenderTarget.
		/// All render calls will now be drawn on the sceneMapTexture.
		/// Make sure you don't reset the RenderTarget until you call Show()!
		/// </summary>
		public void Start()
		{
			// Only apply post screen shader if texture is valid and xnaEffect is valid 
			if (sceneMapTexture == null ||
				xnaEffect == null ||
				started == true ||
				// Also skip if we don't use post screen shaders at all!
				BaseGame.UsePostScreenShaders == false)
				return;

			RenderToTexture.SetRenderTarget(sceneMapTexture.RenderTarget, true);
			started = true;

			remBackBufferSurface = null;
			if (sceneMapTexture.ZBufferSurface != null)
			{
				remBackBufferSurface = BaseGame.Device.DepthStencilBuffer;
				BaseGame.Device.DepthStencilBuffer =
					sceneMapTexture.ZBufferSurface;
			} // if (sceneMapTexture.ZBufferSurface)
		} // Start()
		#endregion

		#region Show
		/// <summary>
		/// Execute shaders and show result on screen, Start(..) must have been
		/// called before and the scene should be rendered to sceneMapTexture.
		/// </summary>
		public virtual void Show()
		{
			// Only apply post screen glow if texture is valid and xnaEffect is valid 
			if (sceneMapTexture == null ||
				Valid == false ||
				started == false)
				return;

			started = false;

			// Resolve sceneMapTexture render target for Xbox360 support
			sceneMapTexture.Resolve(true);

			// Don't use or write to the z buffer
			BaseGame.Device.RenderState.DepthBufferEnable = false;
			BaseGame.Device.RenderState.DepthBufferWriteEnable = false;
			// Also don't use any kind of blending.
			BaseGame.Device.RenderState.AlphaBlendEnable = false;
			//unused: BaseGame.Device.RenderState.Lighting = false;

			if (windowSize != null)
				windowSize.SetValue(new float[]
					{ sceneMapTexture.Width, sceneMapTexture.Height });
			if (sceneMap != null)
				sceneMap.SetValue(sceneMapTexture.XnaTexture);

			xnaEffect.CurrentTechnique = xnaEffect.Techniques["ScreenDarkenBorder"];

			// We must have exactly 1 pass!
			if (xnaEffect.CurrentTechnique.Passes.Count != 1)
				throw new Exception("This shader should have exactly 1 pass!");

			try
			{
				xnaEffect.Begin();
				for (int pass = 0; pass < xnaEffect.CurrentTechnique.Passes.Count; pass++)
				{
					if (pass == 0)
						// Do a full reset back to the back buffer
						RenderToTexture.ResetRenderTarget(true);

					EffectPass effectPass = xnaEffect.CurrentTechnique.Passes[pass];
					effectPass.Begin();
					//tst for last pass? VBScreenHelper.Render10x10Grid();
					VBScreenHelper.Render();
					effectPass.End();
				} // for (pass, <, ++)
			} // try
			finally
			{
				xnaEffect.End();

				// Restore z buffer state
				BaseGame.Device.RenderState.DepthBufferEnable = true;
				BaseGame.Device.RenderState.DepthBufferWriteEnable = true;
			} // finally
		} // Show()
		#endregion

		#region Unit Testing
#if DEBUG
		/*unused
		/// <summary>
		/// Test PostScreenDarkenBorder
		/// </summary>
		//[Test]
		public static void TestPostScreenDarkenBorder()
		{
			PreScreenSkyCubeMapping skyCube = null;
			Model testModel = null;
			PostScreenDarkenBorder postScreenShader = null;

			TestGame.Start("TestPostScreenDarkenBorder",
				delegate
				{
					skyCube = new PreScreenSkyCubeMapping();
					testModel = new Model("Asteroid4");
					postScreenShader = new PostScreenDarkenBorder();
				},
				delegate
				{
					// Start post screen shader to render everything to our sceneMap
					if (Input.Keyboard[Keys.Space] == KeyState.Up &&
						Input.GamePadBPressed == false)
						postScreenShader.Start();

					// Draw background sky cube
					skyCube.RenderSky();
					// And our testModel (the asteroid)
					testModel.Render(Matrix.CreateScale(10));

					// And finally show post screen shader
					if (Input.Keyboard[Keys.Space] == KeyState.Up &&
						Input.GamePadBPressed == false)
						postScreenShader.Show();
					
					TextureFont.WriteText(2, 30,
						"Press space or B to disable post screen shader");
					TextureFont.WriteText(2, 60,
						"Press A to show sceneMap of post screen shader");
					TextureFont.WriteText(2, 90,
						"Speed Mode");

					if (Input.Keyboard.IsKeyDown(Keys.A) ||
						Input.GamePadAPressed)
					{
						sceneMapTexture.RenderOnScreen(
							new Rectangle(10, 10, 256, 256));
					} // if (Input.Keyboard.IsKeyDown)
					//*
				});
		} // TestPostScreenMenu()
		*/
#endif
		#endregion
	} // class PostScreenDarkenBorder
} // namespace XnaShooter.Shaders
