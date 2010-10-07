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
using XnaShooter.Sounds;
#endregion

namespace XnaShooter.Shaders
{
	/// <summary>
	/// Post screen glow shader based on PostScreenMenu.fx
	/// </summary>
	/// <returns>Shader effect</returns>
	public class PostScreenMenu : ShaderEffect
	{
		#region Variables
		/// <summary>
		/// The shader effect filename for this shader.
		/// </summary>
		private const string Filename = "PostScreenMenu.fx";

		/// <summary>
		/// Effect handles for window size and scene map.
		/// </summary>
		protected EffectParameter windowSize,
			sceneMap,
			downsampleMap,
			blurMap1,
			blurMap2,
			noiseMap,
			timer;

		/// <summary>
		/// Links to the passTextures, easier to write code this way.
		/// This are just reference copies. Static to load them only once
		/// (used for both PostScreenMenu and PostScreenGlow).
		/// </summary>
		protected static RenderToTexture sceneMapTexture,
			downsampleMapTexture,
			blurMap1Texture,
			blurMap2Texture;

		/// <summary>
		/// Helper texture for the noise and film effects.
		/// </summary>
		private Texture noiseMapTexture = null;

		/// <summary>
		/// Is this post screen shader started?
		/// Else don't execute Show if it is called.
		/// </summary>
		protected static bool startedPostScreen = false;

		/// <summary>
		/// Started
		/// </summary>
		/// <returns>Bool</returns>
		public bool Started
		{
			get
			{
				return startedPostScreen;
			} // get
		} // Started
		#endregion

		#region Constructor
		/// <summary>
		/// Create post screen menu. Also used for the constructor of
		/// PostScreenGlow (same RenderToTextures used there).
		/// </summary>
		protected PostScreenMenu(string shaderFilename)
			: base(shaderFilename)
		{
			Load();
		} // PostScreenMenu()

		/// <summary>
		/// Create post screen menu
		/// </summary>
		public PostScreenMenu()
			: this(Filename)
		{
		} // PostScreenMenu()
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
			if (downsampleMapTexture != null)
				downsampleMapTexture.Dispose();
			downsampleMapTexture = null;
			if (blurMap1Texture != null)
				blurMap1Texture.Dispose();
			blurMap1Texture = null;
			if (blurMap2Texture != null)
				blurMap2Texture.Dispose();
			blurMap2Texture = null;
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
					RenderToTexture.SizeType.FullScreenWithZBuffer);
			// Downsample map texture (to 1/4 of the screen)
			if (downsampleMapTexture == null)
				downsampleMapTexture = new RenderToTexture(
					RenderToTexture.SizeType.QuarterScreen);

			// Blur map texture
			if (blurMap1Texture == null)
				blurMap1Texture = new RenderToTexture(
					RenderToTexture.SizeType.QuarterScreen);
			// Blur map texture
			if (blurMap2Texture == null)
				blurMap2Texture = new RenderToTexture(
					RenderToTexture.SizeType.QuarterScreen);
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

			// Init additional stuff
			downsampleMap = xnaEffect.Parameters["downsampleMap"];
			blurMap1 = xnaEffect.Parameters["blurMap1"];
			blurMap2 = xnaEffect.Parameters["blurMap2"];
			timer = xnaEffect.Parameters["Timer"];

			// Load noise texture for stripes effect
			noiseMap = xnaEffect.Parameters["noiseMap"];
			noiseMapTexture = new Texture("Noise128x128.dds");
			// Set texture
			noiseMap.SetValue(noiseMapTexture.XnaTexture);
		} // GetParameters()
		#endregion

		#region Start
		DepthStencilBuffer remBackBufferSurface = null;
		/// <summary>
		/// Start this post screen shader, will just call SetRenderTarget.
		/// All render calls will now be drawn on the sceneMapTexture.
		/// Make sure you don't reset the RenderTarget until you call Show()!
		/// </summary>
		public void Start()
		{
			// Only apply post screen shader if texture is valid and effect is valid 
			if (sceneMapTexture == null ||
				xnaEffect == null ||
				startedPostScreen == true ||
				// Also skip if we don't use post screen shaders at all!
				BaseGame.UsePostScreenShaders == false)
				return;

			RenderToTexture.SetRenderTarget(sceneMapTexture.RenderTarget, true);
			startedPostScreen = true;

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
			// Only apply post screen glow if texture is valid and effect is valid 
			if (sceneMapTexture == null ||
				Valid == false ||
				startedPostScreen == false)
				return;

			startedPostScreen = false;

			// Resolve sceneMapTexture render target for Xbox360 support
			sceneMapTexture.Resolve(true);

			try
			{
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
				//can't do this yet, we haven't rendered and resolved this:
				/*
				if (downsampleMap != null)
					downsampleMap.SetValue(downsampleMapTexture.XnaTexture);
				if (blurMap1 != null)
					blurMap1.SetValue(blurMap1Texture.XnaTexture);
				if (blurMap2 != null)
					blurMap2.SetValue(blurMap2Texture.XnaTexture);
				 */

				if (timer != null)
					// Add a little offset to prevent first effect.
					timer.SetValue(BaseGame.TotalTime+0.75f);

				xnaEffect.CurrentTechnique = xnaEffect.Techniques[
					BaseGame.CanUsePS20 ? "ScreenGlow20" : "ScreenGlow"];
				
				// We must have exactly 4 passes!
				if (xnaEffect.CurrentTechnique.Passes.Count != 4)
					throw new Exception("This shader should have exactly 4 passes!");

				xnaEffect.Begin();//SaveStateMode.None);
				for (int pass = 0; pass < xnaEffect.CurrentTechnique.Passes.Count; pass++)
				{
					if (pass == 0)
						downsampleMapTexture.SetRenderTarget();
					else if (pass == 1)
						blurMap1Texture.SetRenderTarget();
					else if (pass == 2)
						blurMap2Texture.SetRenderTarget();
					else
					{
						// Do a full reset back to the back buffer
						RenderToTexture.ResetRenderTarget(true);

						if (sceneMapTexture.ZBufferSurface != null)
							BaseGame.Device.DepthStencilBuffer = remBackBufferSurface;
					} // else

					EffectPass effectPass = xnaEffect.CurrentTechnique.Passes[pass];
					effectPass.Begin();
					//tst for last pass? VBScreenHelper.Render10x10Grid();
					VBScreenHelper.Render();
					effectPass.End();

					if (pass == 0)
					{
						downsampleMapTexture.Resolve(false);
						if (downsampleMap != null)
							downsampleMap.SetValue(downsampleMapTexture.XnaTexture);
						xnaEffect.CommitChanges();
					} // if
					else if (pass == 1)
					{
						blurMap1Texture.Resolve(false);
						if (blurMap1 != null)
							blurMap1.SetValue(blurMap1Texture.XnaTexture);
						xnaEffect.CommitChanges();
					} // else if
					else if (pass == 2)
					{
						blurMap2Texture.Resolve(false);
						if (blurMap2 != null)
							blurMap2.SetValue(blurMap2Texture.XnaTexture);
						xnaEffect.CommitChanges();
					} // else if
				} // for (pass, <, ++)
			} // try
			catch (Exception ex)
			{
				// Make effect invalid, continue rendering without this
				// post screen shader.
				xnaEffect = null;
				RenderToTexture.ResetRenderTarget(true);

				if (sceneMapTexture.ZBufferSurface != null)
					BaseGame.Device.DepthStencilBuffer = remBackBufferSurface;
#if DEBUG
				throw ex;
#else
				Log.Write("Failed to render post screen shader "+Filename+": "+
					ex.ToString());
#endif
			} // catch
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
		/// <summary>
		/// Test post screen menu
		/// </summary>
		//[Test]
		public static void TestPostScreenMenu()
		{
			//Model testModel = null;
			//PostScreenMenu menuShader = null;

			TestGame.Start("TestPostScreenMenu",
				delegate
				{
					//testModel = new Model("Asteroid2.x");
					//menuShader = new PostScreenMenu();
					Sound.StartMusic();
				},
				delegate
				{
					BaseGame.GlowShader.Start();

					//YourGame.RenderGameBackground();
					//Thread.Sleep(10);
					//testModel.Render(Vector3.Empty);
					
					BaseGame.DrawLine(
						Vector3.Zero, new Vector3(100, 100, 100), Color.Red);
					BaseGame.FlushLineManager3D();

					//if (Input.Keyboard.IsKeyDown(Keys.Space) == false)
					//	menuShader.Show();

					if (Input.Keyboard.IsKeyDown(Keys.LeftAlt) == false &&
						Input.GamePadAPressed == false)
						BaseGame.GlowShader.Show();
					else
					{
						// Resolve first
						sceneMapTexture.Resolve(true);
						startedPostScreen = false;

						// Reset background buffer
						RenderToTexture.ResetRenderTarget(true);
						// Just show scene map
						//BaseGame.UI.PostScreenGlowShader.
						sceneMapTexture.RenderOnScreen(BaseGame.ResolutionRect);
					} // else
					
					TextureFont.WriteText(2, 30,
						"Press left alt or A to just show the unchanged screen.");
					TextureFont.WriteText(2, 60,
						"Press space or B to see all menu post screen render passes.");

					//*TODO
					if (Input.Keyboard.IsKeyDown(Keys.Space) ||// == false)
						Input.GamePadBPressed)
					{
						PostScreenMenu psm = BaseGame.GlowShader;
						sceneMapTexture.RenderOnScreen(
							new Rectangle(10, 10, 256, 256));
						downsampleMapTexture.RenderOnScreen(
							new Rectangle(10 + 256 + 10, 10, 256, 256));
						blurMap1Texture.RenderOnScreen(
							new Rectangle(10 + 256 + 10 + 256 + 10, 10, 256, 256));
						blurMap2Texture.RenderOnScreen(
							new Rectangle(10, 10 + 256 + 10, 256, 256));
					} // if (Input.Keyboard.IsKeyDown)
					//*/
				});
		} // TestPostScreenMenu()
#endif
		#endregion
	} // class PostScreenMenu
} // namespace XnaShooter.Shaders
