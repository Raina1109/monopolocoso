using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using TheWaver.Core;

namespace TheWaver
{
    public class level01_description : GameScene
    {
        public level01_description(Game game, Texture2D backgroundImage)
            : base(game)
        {
            //The intro image background will be process to the ImageProcess class and display the image
            Components.Add(new ImageProcess(game, backgroundImage, ImageProcess.DrawMode.Stretch));
        }
        /********************************************************************/
        public override void Show()
        {
            base.Show();//Display the scene
        }
        /********************************************************************/
    }
}
