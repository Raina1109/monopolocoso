// Project: XnaShooter, File: SimpleCamera.cs
// Namespace: XnaShooter.Game, Class: SimpleCamera
// Path: C:\code\Xna\XnaShooter\Game, Author: Abi
// Code lines: 126, Size of file: 3,06 KB
// Creation date: 13.03.2007 22:57
// Last modified: 16.03.2007 05:02
// Generated with Commenter by abi.exDream.com

#region Using directives
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using XnaShooter.Helpers;
#endregion

namespace XnaShooter.Game
{
	/// <summary>
	/// Simple rotation camera class just to rotate around a little.
	/// Always focuses on the center and uses the same direction vector,
	/// which is only rotate around the z axis.
	/// </summary>
	public class RotationCamera : GameComponent
	{
		#region Variables
		Vector3 initialPos = new Vector3(0, 15, 15),
			cameraPos;
		float rotation = 0;
		float distance = 1.0f;
		#endregion

		#region Constructor
		public RotationCamera(BaseGame game)
			: base(game)
		{
		} // RotationCamera(game)
		#endregion

		#region Initialize
		public override void Initialize()
		{
			base.Initialize();
		} // Initialize
		#endregion

		#region Update
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Update camera position (allow mouse and gamepad)
			rotation += BaseGame.MoveFactorPerSecond / 1.75f;
			cameraPos = Vector3.Transform(initialPos*distance,
				Matrix.CreateRotationZ(rotation));

			// Allow zooming in/out with gamepad, mouse wheel and cursor keys.
			distance += Input.GamePad.ThumbSticks.Left.Y;
			distance += Input.MouseWheelDelta / 2000.0f;
			if (Input.KeyboardDownPressed)
				distance += BaseGame.MoveFactorPerSecond / 2.5f;
			if (Input.KeyboardUpPressed)
				distance -= BaseGame.MoveFactorPerSecond / 2.5f;
			if (distance < 0.01f)
				distance = 0.01f;
			if (distance > 10)
				distance = 10;

			BaseGame.ViewMatrix = Matrix.CreateLookAt(
				cameraPos, new Vector3(0, 0, 0), new Vector3(0, 1, 0));
		} // Update(gameTime)
		#endregion
	} // class SimpleCamera
} // namespace XnaShooter.Game
