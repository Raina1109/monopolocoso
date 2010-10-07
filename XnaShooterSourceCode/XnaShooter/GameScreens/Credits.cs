// Project: Rocket Commander, File: Credits.cs
// Namespace: XnaShooter.GameScreens, Class: Credits
// Path: C:\code\XnaShooter\GameScreens, Author: Abi
// Code lines: 46, Size of file: 857 Bytes
// Creation date: 23.11.2005 18:37
// Last modified: 12.12.2005 05:30
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using XnaShooter.Graphics;
using XnaShooter.Game;
using XnaShooter.Properties;
using XnaShooter.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using System.Diagnostics;
#endregion

namespace XnaShooter.GameScreens
{
	/// <summary>
	/// Credits
	/// </summary>
	class Credits : IGameScreen
	{
		#region Properties
		/// <summary>
		/// Name of this game screen
		/// </summary>
		/// <returns>String</returns>
		public string Name
		{
			get
			{
				return "Credits";
			} // get
		} // Name

		private bool quit = false;
		/// <summary>
		/// Returns true if we want to quit this screen and return to the
		/// previous screen. If no more screens are left the game is exited.
		/// </summary>
		/// <returns>Bool</returns>
		public bool Quit
		{
			get
			{
				return quit;
			} // get
		} // Quit
		#endregion

		#region Constructor
		/// <summary>
		/// Create credits
		/// </summary>
		public Credits()
		{
		} // Credits()
		#endregion

		#region Write credits
		/// <summary>
		/// Write credits
		/// </summary>
		/// <param name="xPos">X coordinate</param>
		/// <param name="yPos">Y coordinate</param>
		/// <param name="leftText">Left text</param>
		/// <param name="rightText">Right text</param>
		private void WriteCredits(int xPos, int yPos,
			string leftText, string rightText)
		{
			TextureFont.WriteText(xPos, yPos, leftText);
			TextureFont.WriteText(xPos + 440, yPos/* + 8*/, rightText);
		} // WriteCredits(xPos, yPos, leftText)

		/// <summary>
		/// Write credits with link
		/// </summary>
		/// <param name="xPos">X coordinate</param>
		/// <param name="yPos">Y coordinate</param>
		/// <param name="leftText">Left text</param>
		/// <param name="rightText">Right text</param>
		/// <param name="linkText">Link text</param>
		private void WriteCreditsWithLink(int xPos, int yPos, string leftText,
			string rightText, string linkText, XnaShooterGame game)
		{
			WriteCredits(xPos, yPos, leftText, rightText);

			// Process link (put below rightText)
			bool overLink = Input.MouseInBox(new Rectangle(
				xPos + 440, yPos + 8 + TextureFont.Height, 200, TextureFont.Height));
			TextureFont.WriteText(xPos + 440, yPos /*+ 8*/ + TextureFont.Height, linkText,
				overLink ? Color.Red : Color.White);
			if (overLink &&
				Input.MouseLeftButtonJustPressed)
			{
#if !XBOX360
				Process.Start(linkText);
				Thread.Sleep(100);
#endif
			} // if
		} // WriteCreditsWithLink(xPos, yPos, leftText)
		#endregion

		#region Run
		/// <summary>
		/// Run game screen. Called each frame.
		/// </summary>
		/// <param name="game">Form for access to asteroid manager and co</param>
		public void Run(XnaShooterGame game)
		{
			// Render background
			game.RenderMenuBackground();

			// Credits
			int xPos = 50 * BaseGame.Width / 1024;
			int yPos = 260 * BaseGame.Height / 768;
			TextureFont.WriteText(xPos, yPos, "Credits");

			WriteCreditsWithLink(xPos, yPos+56, "Idea, Design, Programming",
				"Benjamin Nitschke (abi)",
				"http://abi.exdream.com", game);
			WriteCredits(xPos, yPos + 137, "Thanks fly out to",
				"Leif Griga, Christoph Rienaecker (WAII),");
			WriteCredits(xPos, yPos + 177, "",
				"Boje Holtz, Enrico Cieslik (Judge),");
			WriteCredits(xPos, yPos + 217, "",
				"Kai Walter (for some sound effects),");
			WriteCredits(xPos, yPos + 257, "",
				"ZMan (www.thezbuffer.com),");
			WriteCredits(xPos, yPos + 297, "",
				"Christina Storm, the XNA team");
			WriteCredits(xPos, yPos + 337, "",
				"and the .NET teams of Microsoft.");

			TextureFont.WriteText(BaseGame.XToRes(150), BaseGame.YToRes(687),
				"Dedicated to the great XNA Framework.");

			if (game.RenderMenuButton(MenuButton.Back,
				new Point(1024 - 230, 768 - 150)))
			{
				quit = true;
			} // if
		} // Run(game)
		#endregion
	} // class Credits
} // namespace XnaShooter.GameScreens
