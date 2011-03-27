using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TheWaver.Core;

namespace TheWaver
{
    public class AboutScene : GameScene
    {
        /********************************************************************/
        public AboutScene(Game game, Texture2D backgroundImage)
            : base(game)
        {
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
