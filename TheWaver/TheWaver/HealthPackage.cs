using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheWaver.Core;

namespace TheWaver
{
    public class HealthPackage : DrawableGameComponent
    {
        protected Texture2D healthBox;
        protected Rectangle rectangle;
        protected Vector2 boxPosition;
        protected SpriteBatch sBatch;
        //***********************************************************************//
        public HealthPackage(Game game, Texture2D healthBox, Rectangle rectangle, Vector2 boxPosition)
            : base(game)
        {
            this.healthBox = healthBox;
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
            boxPosition = new Vector2(555, 555);
            Enabled = false;
        }
        //***********************************************************************//
        public bool CheckCollision(Rectangle rect)
        {
            Rectangle spriterect = new Rectangle((int)boxPosition.X, (int)boxPosition.Y,rectangle.Width, rectangle.Height);
            return spriterect.Intersects(rect);
        }
        //***********************************************************************//
        public override void Draw(GameTime gameTime)
        {
            // Get the current spritebatch
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            sBatch.Draw(healthBox, boxPosition, rectangle, Color.White); // Draw the ship// ..position..

            base.Draw(gameTime);
        }
        //***********************************************************************//
    }
}
