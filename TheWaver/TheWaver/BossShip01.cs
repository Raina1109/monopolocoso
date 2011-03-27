using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheWaver.Core;
using TheWaver.StoryLevel01;

namespace TheWaver
{
    public class BossShip01 : DrawableGameComponent
    {
        protected Texture2D bossShip01;
        protected Rectangle rectangle;
        protected Vector2 position;
        protected SpriteBatch sBatch;

        public int bossHealth;

        public BossShip01(Game game, Texture2D bossShip01, Rectangle rectangle, Vector2 position)
            : base(game)
        {
            this.bossShip01 = bossShip01;
            this.rectangle = rectangle;
            this.position = position;
        }
        //***********************************************************************//
        public void Reset()
        {
            Visible = true;
            bossHealth = 100;
        }
        //***********************************************************************//
        public void PutinStartPosition()
        {
            position = new Vector2(720, 400);
            Enabled = false;
        }
        //***********************************************************************//
        public bool CheckCollision(Rectangle rect)
        {
            Rectangle spriterect = new Rectangle((int)position.X, (int)position.Y, rectangle.Width, rectangle.Height);
            return spriterect.Intersects(rect);
        }
        //***********************************************************************//
        public int BossHealth
        {
            get { return bossHealth; }
            set
            {
                if (value < 0)
                    bossHealth = 0;
                else
                    bossHealth = value;
            }
        }
        //***********************************************************************//
        public override void Draw(GameTime gameTime)
        {
            // Get the current spritebatch
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            sBatch.Draw(bossShip01, position, rectangle, Color.White); // Draw the ship// ..position..

            base.Draw(gameTime);
        }
        //***********************************************************************//
    }
}
