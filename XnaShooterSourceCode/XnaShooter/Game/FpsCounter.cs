#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaShooter.Helpers;
using Microsoft.Xna.Framework.Input;
using XnaShooter.Graphics;
#endregion

namespace XnaShooter.Game
{
	/// <summary>
	/// Simple FPS (Frames per second) counter component,
	/// which displays the current fps in the upper left corner.
	/// </summary>
	class FpsCounter : DrawableGameComponent
	{
		#region Variables
		/// <summary>
		/// Elapsed time this frame in ms. Always have something valid here
		/// in case we devide through this values!
		/// </summary>
		private static float elapsedTimeThisFrameInMs = 0.001f, totalTimeMs = 0;

		/// <summary>
		/// Helper for calculating frames per second. 
		/// </summary>
		private static float startTimeThisSecond = 0;

		/// <summary>
		/// For more accurate frames per second calculations,
		/// just count for one second, then fpsLastSecond is updated.
		/// Btw: Start with 1 to help some tests avoid the devide through zero
		/// problem.
		/// </summary>
		private static int
			frameCountThisSecond = 0,
			totalFrameCount = 0,
			fpsLastSecond = 60;

		/// <summary>
		/// Interpolated fps over the last 10 seconds.
		/// Obviously goes down if our framerate is low.
		/// </summary>
		private static float fpsInterpolated = 100.0f;

		/// <summary>
		/// Fps
		/// </summary>
		/// <returns>Int</returns>
		public static int Fps
		{
			get
			{
				return fpsLastSecond;
			} // get
		} // Fps
		#endregion
		
		#region Constructor
		public FpsCounter(BaseGame game)
			: base(game)
		{
		} // FpsCounter(game)
		#endregion

		#region Update
		/// <summary>
		/// Update
		/// </summary>
		/// <param name="gameTime">Game time</param>
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			elapsedTimeThisFrameInMs =
				(float)gameTime.ElapsedRealTime.TotalMilliseconds;
			totalTimeMs += elapsedTimeThisFrameInMs;

			// Make sure elapsedTimeThisFrameInMs is never 0
			if (elapsedTimeThisFrameInMs <= 0)
				elapsedTimeThisFrameInMs = 0.001f;

			// Increase frame counter for FramesPerSecond
			frameCountThisSecond++;
			totalFrameCount++;

			// One second elapsed?
			if (totalTimeMs - startTimeThisSecond > 1000.0f)
			{
				// Calc fps
				fpsLastSecond = (int)(frameCountThisSecond * 1000.0f /
					(totalTimeMs - startTimeThisSecond));
				if (fpsLastSecond <= 0)
					fpsLastSecond = 1;
				// Reset startSecondTick and repaintCountSecond
				startTimeThisSecond = totalTimeMs;
				frameCountThisSecond = 0;

				fpsInterpolated =
					MathHelper.Lerp(fpsInterpolated, fpsLastSecond, 0.1f);
			} // if (Math.Abs)

			// Restore depth buffer, else 3D rendering will be messed up.
			BaseGame.Device.RenderState.DepthBufferEnable = true;
		} // Update(gameTime)
		#endregion

		#region Draw
		bool showFps =
#if DEBUG
			true;
#else
			false;
#endif

		/// <summary>
		/// Draw
		/// </summary>
		/// <param name="gameTime">Game time</param>
		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			// Show fps (use F1 or left shoulder+y button on gamepad to toggle)
			if (Input.KeyboardF1JustPressed ||
				// Also allow toggeling with gamepad
				(Input.GamePad.Buttons.LeftShoulder == ButtonState.Pressed &&
				Input.GamePadYJustPressed))
				showFps = !showFps;
#if XBOX360
			if (showFps)
				TextureFont.WriteText(BaseGame.XToRes(32), BaseGame.YToRes(26),
					"Fps: " + Fps);
#else
			if (showFps)
				TextureFont.WriteText(2, 2, "Fps: " + Fps);
#endif
		} // Draw(gameTime)
		#endregion
	} // class FpsCounter
} // namespace XnaShooter.Game
