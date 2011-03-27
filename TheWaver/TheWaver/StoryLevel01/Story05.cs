using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TheWaver.Core;

namespace TheWaver
{
    public class Story05 : GameScene
    {
        /********************************************************************/
        public Story05(Game game, Texture2D backgroundImage)
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
