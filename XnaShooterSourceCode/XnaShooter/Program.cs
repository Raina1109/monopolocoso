// Project: XnaShooter, File: Program.cs
// Namespace: XnaShooter, Class: Program
// Path: C:\code\XnaShooter\, Author: Abi
// Code lines: 182, Size of file: 5,41 KB
// Creation date: 02.10.2006 01:33
// Last modified: 27.10.2006 02:40
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using XnaShooter.Sounds;
using XnaShooter.Shaders;
using XnaShooter.Graphics;
using XnaShooter.GameScreens;
using XnaShooter.Game;
#endregion

namespace XnaShooter
{
	static class Program
	{
		#region Variables
		/// <summary>
		/// Version number for this program. Used to check for updates
		/// and also displayed in the main menu.
		/// V. 1.0 was the initial version for XNA 1.0 (2006-10)
		/// V. 2.0 is a little bit updated and for XNA 2.0 (2008-01)
		/// </summary>
		public static int versionHigh = 2,
			versionLow = 0;
		#endregion

		#region Main
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			//* The game
			using (XnaShooterGame game = new XnaShooterGame())
			{
				game.Run();
			}
			/*/ // Unit Tests (only in debug mode!)
			//Sound.TestPlaySounds();
			//Model.TestModels();
			//AnimatedTexture.TestAnimatedTexture();
			//PostScreenMenu.TestPostScreenMenu();
			//Mission.TestRenderLevelBackground();
			//ShadowMapShader.TestShadowMapping();
			//RenderToTexture.TestCreateRenderToTexture();
			//MainMenu.TestMenu();
			//Billboard.TestRenderBillboards();
			//EffectManager.TestEffects();
			//NumbersTextureFont.TestWriteNumbers();
			//Mission.TestHud();
			//Unit.TestUnitAI();
			//*/
		} // Main(args)
		#endregion
	} // class Program
} // namespace XnaShooter
