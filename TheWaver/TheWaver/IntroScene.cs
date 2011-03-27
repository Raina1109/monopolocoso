using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using TheWaver.Core;

namespace TheWaver
{
    public class IntroScene : GameScene//Deriving the GameScene class
    {
        protected AudioLibrary audio;
        /********************************************************************/
        public IntroScene(Game game, Texture2D backgroundImage)
            : base(game)
        {
            //The intro image background will be process to the ImageProcess class and display the image
            Components.Add(new ImageProcess(game, backgroundImage, ImageProcess.DrawMode.Stretch));

            // Get the audio library
            audio = (AudioLibrary)Game.Services.GetService(typeof(AudioLibrary));
            
        }
        /********************************************************************/
        public override void Show()
        {
            MediaPlayer.Play(audio.StartMusic);
            base.Show();//Display the scene
        }
        /********************************************************************/
    }
}
