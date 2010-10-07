// Project: XnaShooter, File: TextureFont.cs
// Namespace: XnaShooter.Graphics, Class: TextureFont
// Path: C:\code\XnaShooter\Graphics, Author: Abi
// Code lines: 380, Size of file: 10,76 KB
// Creation date: 31.08.2006 19:47
// Last modified: 18.09.2006 22:11
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using XnaShooter.Game;
using XnaShooter.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace XnaShooter.Graphics
{
	/// <summary>
	/// Texture font
	/// </summary>
	public class NumbersTextureFont : IGraphicContent
	{
		#region Constants
		/// <summary>
		/// Smaller numbers in the NumbersFont.png graphic
		/// </summary>
		private static readonly Rectangle[] SmallNumberRects =
			{
				// 0
				new Rectangle(0, 0, 25, 36),
				// 1
				new Rectangle(26, 0, 20, 36),
				// 2
				new Rectangle(48, 0, 24, 36),
				// 3
				new Rectangle(72, 0, 24, 36),
				// 4
				new Rectangle(96, 0, 25, 36),
				// 5
				new Rectangle(121, 0, 23, 36),
				// 6
				new Rectangle(144, 0, 25, 36),
				// 7
				new Rectangle(169, 0, 23, 36),
				// 8
				new Rectangle(192, 0, 24, 36),
				// 9
				new Rectangle(216, 0, 24, 36),
				// :
				new Rectangle(240, 0, 15, 36),
			};
		#endregion

		#region Variables
		/// <summary>
		/// Texture for rendering all these fonts
		/// </summary>
		private Texture fontTexture = null;
		/// <summary>
		/// Font texture name
		/// </summary>
		string fontTextureName = "";
		#endregion

		#region Constructor
		/// <summary>
		/// Create texture font
		/// </summary>
		public NumbersTextureFont(string setFontTextureName)
		{
			fontTextureName = setFontTextureName;
			Load();
			BaseGame.RegisterGraphicContentObject(this);
		} // NumbersTextureFont(setFontTexture)
		#endregion

		#region Load
		/// <summary>
		/// Load the font, also used for reloading.
		/// </summary>
		public void Load()
		{
			if (fontTexture == null)
				fontTexture = new Texture(fontTextureName);
		} // Load()
		#endregion

		#region Dispose
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			if (fontTexture != null)
				fontTexture.Dispose();
			fontTexture = null;
		} // Dispose()
		#endregion

		#region Write number
		/// <summary>
		/// Write digit
		/// </summary>
		/// <param name="x">X</param>
		/// <param name="y">Y</param>
		/// <param name="gfxRects">Gfx rects</param>
		/// <returns>Int</returns>
		private int WriteDigit(int x, int y, int digit, Rectangle[] gfxRects)
		{
			float resScalingX = 0.725f * (float)BaseGame.Width / 1024.0f;
			float resScalingY = 0.725f * (float)BaseGame.Height / 768.0f;

			Rectangle rect = gfxRects[digit % gfxRects.Length];
			fontTexture.RenderOnScreen(new Rectangle(x, y,
				(int)Math.Round(rect.Width * resScalingX),
				(int)Math.Round(rect.Height * resScalingY)), rect);

			return (int)Math.Round((rect.Width - 4) * resScalingX);
		} // WriteDigit(x, y, digit)

		/// <summary>
		/// Write digit
		/// </summary>
		/// <param name="x">X</param>
		/// <param name="y">Y</param>
		/// <param name="digit">Digit</param>
		/// <param name="gfxRects">Gfx rects</param>
		/// <param name="alpha">Alpha</param>
		/// <returns>Int</returns>
		private int WriteDigit(int x, int y, int digit, Rectangle[] gfxRects,
			float alpha)
		{
			float resScalingX = 0.725f * (float)BaseGame.Width / 1024.0f;
			float resScalingY = 0.725f * (float)BaseGame.Height / 768.0f;

			Rectangle rect = gfxRects[digit % gfxRects.Length];
			fontTexture.RenderOnScreen(new Rectangle(x, y,
				(int)Math.Round(rect.Width * resScalingX),
				(int)Math.Round(rect.Height * resScalingY)), rect,
				Color.White, alpha);

			return (int)Math.Round((rect.Width-4) * resScalingX);
		} // WriteDigit(x, y, digit)

		/// <summary>
		/// Write number
		/// </summary>
		/// <param name="x">X</param>
		/// <param name="y">Y</param>
		/// <param name="number">Number</param>
		/// <param name="gfxRects">Gfx rects</param>
		/// <returns>Int</returns>
		private int WriteNumber(int x, int y, int number, Rectangle[] gfxRects)
		{
			// Convert to string
			string numberText = number.ToString();
			int width = 0;

			// And now process every letter
			foreach (char numberChar in numberText.ToCharArray())
			{
				width += WriteDigit(x + width, y, (int)numberChar - (int)'0', gfxRects);
			} // foreach (numberChar)

			return width;
		} // WriteNumber(x, y, number)

		/// <summary>
		/// Write number
		/// </summary>
		/// <param name="x">X</param>
		/// <param name="y">Y</param>
		/// <param name="number">Number</param>
		/// <param name="gfxRects">Gfx rects</param>
		/// <param name="alpha">Alpha</param>
		/// <returns>Int</returns>
		private int WriteNumber(int x, int y, int number, Rectangle[] gfxRects,
			float alpha)
		{
			// Convert to string
			string numberText = number.ToString();
			int width = 0;

			// And now process every letter
			foreach (char numberChar in numberText.ToCharArray())
			{
				width += WriteDigit(
					x + width, y, (int)numberChar - (int)'0', gfxRects, alpha);
			} // foreach (numberChar)

			return width;
		} // WriteNumber(x, y, number)

		/// <summary>
		/// Write number
		/// </summary>
		/// <param name="x">X</param>
		/// <param name="y">Y</param>
		/// <param name="number">Number</param>
		public void WriteNumber(int x, int y, int number)
		{
			WriteNumber(x, y, number, SmallNumberRects);
		} // WriteSmallNumber(x, y, number)

		/// <summary>
		/// Write number centered
		/// </summary>
		/// <param name="x">X</param>
		/// <param name="y">Y</param>
		/// <param name="number">Number</param>
		public void WriteNumberCentered(int x, int y, int number)
		{
			float resScalingY = 0.725f * (float)BaseGame.Height / 768.0f;
			WriteNumber(x - (int)(resScalingY *
				(number.ToString().Length * SmallNumberRects[0].Width / 2)),
				y, number, SmallNumberRects);
		} // WriteSmallNumberCentered(x, y, number)

		/// <summary>
		/// Write number centered
		/// </summary>
		/// <param name="x">X</param>
		/// <param name="y">Y</param>
		/// <param name="number">Number</param>
		/// <param name="alpha">Alpha</param>
		public void WriteNumberCentered(int x, int y, int number, float alpha)
		{
			float resScalingY = 0.725f * (float)BaseGame.Height / 768.0f;
			WriteNumber(x - (int)(resScalingY *
				number.ToString().Length * SmallNumberRects[0].Width / 2),
				y, number, SmallNumberRects, alpha);
		} // WriteNumberCentered(x, y, number)
		#endregion

		#region Write time
		/// <summary>
		/// Write time
		/// </summary>
		/// <param name="x">X</param>
		/// <param name="y">Y</param>
		/// <param name="minutes">Minutes</param>
		/// <param name="seconds">Seconds</param>
		public void WriteTime(int x, int y, int minutes, int seconds)
		{
			// Start with minutes
			int minuteWidth = WriteNumber(x, y, minutes, SmallNumberRects);

			// Show : in the middle
			minuteWidth += WriteDigit(x + minuteWidth, y,
				SmallNumberRects.Length - 1, SmallNumberRects);

			// Add a zero if we got 0-9 seconds
			if (seconds < 10)
				minuteWidth += WriteDigit(x + minuteWidth, y, 0, SmallNumberRects);

			// And now add seconds
			WriteNumber(x + minuteWidth, y, seconds, SmallNumberRects);
		} // WriteTime(x, y, minutes)

		/// <summary>
		/// Write time
		/// </summary>
		/// <param name="x">X</param>
		/// <param name="y">Y</param>
		/// <param name="timeMs">Time in ms</param>
		public void WriteTime(int x, int y, int timeMs)
		{
			int minutes = (int)(timeMs / 1000) / 60;
			int seconds = (int)(timeMs / 1000) % 60;
			WriteTime(x, y, minutes, seconds);
		} // WriteTime(x, y, timeMs)
		#endregion

		#region Unit Testing
#if DEBUG
		/// <summary>
		/// Test write numbers
		/// </summary>
		public static void TestWriteNumbers()
		{
			NumbersTextureFont textureFont = null;
			TestGame.Start(
				"TestWriteNumbers",
				delegate
				{
					textureFont = new NumbersTextureFont("NumbersFont");
				},
				delegate
				{
					BaseGame.EnableAlphaBlending();
					textureFont.WriteNumber(100, 100, 123);
					textureFont.WriteNumberCentered(100, 200, 123);
					textureFont.WriteNumberCentered(100, 250, 123);
					textureFont.WriteTime(250, 250, (int)BaseGame.TotalTimeMs);
				});
		} // TestWriteNumbers()
#endif
		#endregion
	} // class NumbersTextureFont
} // namespace XnaShooter.Graphics
