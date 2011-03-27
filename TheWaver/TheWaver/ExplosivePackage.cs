using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheWaver.Core;

namespace TheWaver
{
    public class ExplosivePackage : DrawableGameComponent
    {
        protected Texture2D expBox;
        protected Rectangle rectangle;
        protected Vector2 boxPosition;
        protected SpriteBatch sBatch;
        //***********************************************************************//
        public ExplosivePackage(Game game, Texture2D expBox, Rectangle rectangle, Vector2 boxPosition)
            : base(game)
        {
            this.expBox = expBox;
            this.rectangle = rectangle;
            this.boxPosition = boxPosition;
        }
        //***********************************************************************//
        public void Reset()
        {
            Visible = true;
        }
        //***********************************************************************//
        public void PutinStartPosition()
        {
            boxPosition = new Vector2(222, 222);
            Enabled = false;
        }
        //***********************************************************************//

        //***********************************************************************//
        public bool CheckCollision(Rectangle rect)
        {
            Rectangle spriterect = new Rectangle((int)boxPosition.X, (int)boxPosition.Y, rectangle.Width, rectangle.Height);
            return spriterect.Intersects(rect);
        }
        //***********************************************************************//
        public override void Draw(GameTime gameTime)
        {
            // Get the current spritebatch
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            sBatch.Draw(expBox, boxPosition, rectangle, Color.White); // Draw the ship// ..position..

            base.Draw(gameTime);
        }
        //***********************************************************************//
    }
}
