// Project: XnaShooter, File: Sound.cs
// Namespace: XnaShooter.Sounds, Class: Sound
// Path: C:\code\XnaBook\XnaShooter\Sounds, Author: abi
// Code lines: 299, Size of file: 7,32 KB
// Creation date: 07.12.2006 18:22
// Last modified: 07.12.2006 22:29
// Generated with Commenter by abi.exDream.com

#region Using directives
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using XnaShooter.Helpers;
using XnaShooter.Game;
using XnaShooter.Graphics;
#endregion

namespace XnaShooter.Sounds
{
	/// <summary>
	/// Sound
	/// </summary>
	class Sound
	{
		#region Variables
		/// <summary>
		/// Sound stuff for XAct
		/// </summary>
		static AudioEngine audioEngine;
		/// <summary>
		/// Wave bank
		/// </summary>
		static WaveBank waveBank;
		/// <summary>
		/// Sound bank
		/// </summary>
		static SoundBank soundBank;
		    
		/// <summary>
		/// Special category for music, we can only play one music at a time
		/// and to stop we use this category. For play just use play.
		/// </summary>
    static AudioCategory musicCategory;
		#endregion

		#region Enums
		/// <summary>
		/// Sounds we use in this game.
		/// </summary>
		/// <returns>Enum</returns>
		public enum Sounds
		{
			Defeat,
			EMP,
			EnemyShoot,
			Explosion,
			GattlingShoot,
			Health,
			NewWeapon,
			MgShoot,
			PlasmaShoot,
			Victory,
			GameMusic,
			Click,
			Highlight,
		} // enum Sounds
		#endregion

		#region Constructor
		/// <summary>
		/// Create sound
		/// </summary>
		static Sound()
		{
			try
			{
				string dir = Directories.SoundsDirectory;
				audioEngine = new AudioEngine(
					Path.Combine(dir, "XnaShooter.xgs"));
				waveBank = new WaveBank(audioEngine,
					Path.Combine(dir, "Wave Bank.xwb"));

				// Dummy wavebank call to get rid of the warning that waveBank is
				// never used (well it is used, but only inside of XNA).
				if (waveBank != null)
					soundBank = new SoundBank(audioEngine,
						Path.Combine(dir, "Sound Bank.xsb"));

				// Get the music category to change the music volume and stop music
				musicCategory = audioEngine.GetCategory("Music");
			} // try
			catch (Exception ex)
			{
				// Audio creation crashes in early xna versions, log it and ignore it!
				Log.Write("Failed to create sound class: " + ex.ToString());
			} // catch
		} // Sound()
		#endregion

		#region Play
		/// <summary>
		/// Play
		/// </summary>
		/// <param name="soundName">Sound name</param>
		public static void Play(string soundName)
		{
			if (soundBank == null)
				return;

			try
			{
				soundBank.PlayCue(soundName);
			} // try
			catch (Exception ex)
			{
				Log.Write("Playing sound " + soundName + " failed: " + ex.ToString());
			} // catch
		} // Play(soundName)

		/// <summary>
		/// Play
		/// </summary>
		/// <param name="sound">Sound</param>
		public static void Play(Sounds sound)
		{
			Play(sound.ToString());
		} // Play(sound)

		/// <summary>
		/// Play defeat sound
		/// </summary>
		public static void PlayDefeatSound()
		{
			Play(Sounds.Defeat);
		} // PlayDefeatSound()

		/// <summary>
		/// Play victory sound
		/// </summary>
		public static void PlayVictorySound()
		{
			Play(Sounds.Victory);
		} // PlayVictorySound()

		/// <summary>
		/// Play explosion sound
		/// </summary>
		public static void PlayExplosionSound()
		{
			Play(Sounds.Explosion);
		} // PlayExplosionSound()
		#endregion

		#region Music
		/// <summary>
		/// Start music
		/// </summary>
		public static void StartMusic()
		{
			Play(Sounds.GameMusic);
		} // StartMusic()

		/// <summary>
		/// Stop music
		/// </summary>
		public static void StopMusic()
		{
			musicCategory.Stop(AudioStopOptions.Immediate);
		} // StopMusic()
		#endregion

		#region Update
		/// <summary>
		/// Update, just calls audioEngine.Update!
		/// </summary>
		public static void Update()
		{
			if (audioEngine != null)
				audioEngine.Update();
		} // Update()
		#endregion

		#region Unit Testing
#if DEBUG
		/// <summary>
		/// Test play sounds
		/// </summary>
		[Test]
		public static void TestPlaySounds()
		{
			TestGame.Start(
				delegate
				{
					if (Input.MouseLeftButtonJustPressed ||
						Input.GamePadAJustPressed)
						Sound.Play(Sounds.Defeat);
					else if (Input.MouseRightButtonJustPressed ||
						Input.GamePadBJustPressed)
						Sound.Play(Sounds.Victory);
					else if (Input.KeyboardKeyJustPressed(Keys.D1))
						Sound.Play(Sounds.GameMusic);
					else if (Input.KeyboardKeyJustPressed(Keys.D2))
						Sound.Play(Sounds.EnemyShoot);
					else if (Input.KeyboardKeyJustPressed(Keys.D3))
						Sound.Play(Sounds.Explosion);
					else if (Input.KeyboardKeyJustPressed(Keys.D4))
						Sound.Play(Sounds.Health);
					else if (Input.KeyboardKeyJustPressed(Keys.D5))
						Sound.Play(Sounds.PlasmaShoot);
					else if (Input.KeyboardKeyJustPressed(Keys.D6))
						Sound.Play(Sounds.MgShoot);
					else if (Input.KeyboardKeyJustPressed(Keys.D7))
						Sound.Play(Sounds.GattlingShoot);
					else if (Input.KeyboardKeyJustPressed(Keys.D8))
						Sound.Play(Sounds.EMP);

					TextureFont.WriteText(2, 30,
						"Press 1-8 or A/B or left/right mouse buttons to play back "+
						"sounds!");
				});
		} // TestPlaySounds()
#endif
		#endregion
	} // class Sound
} // XnaShooter.Sounds
