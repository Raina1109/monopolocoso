using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheWaver.Core;
using TheWaver.StoryLevel01;
using TheWaver.StoryLevel02;

namespace TheWaver
{
    public class HeliSite : DrawableGameComponent
    {
        protected Texture2D heliSiteTexutre;
        protected Rectangle rectangle;
        protected Vector2 heliSitePosition;
        protected SpriteBatch sBatch;
        bool isDetect;
        int mAlphaValue;
        int mFadeIncrement = 3;
        double rectTime = .035;
        double mFadeDelay = .1;
        //***********************************************************************//
        public HeliSite(Game game, Texture2D heliSiteTexutre, Rectangle rectangle, Vector2 heliSitePosition)
            : base(game)
        {
            this.heliSiteTexutre = heliSiteTexutre;
            this.rectangle = rectangle;
            this.heliSitePosition = heliSitePosition;
        }
        //***********************************************************************//
        public void Reset()
        {
            Visible = true;
            mAlphaValue = 255;
            isDetect = true;
        }
        //***********************************************************************//
        public void PutinStartPosition()
        {
            heliSitePosition = new Vector2(555, 555);
            Enabled = false;
        }
        //***********************************************************************//
        public bool IsDetect
        {
            get { return isDetect; }
            set { this.isDetect = value; }
        }
        //***********************************************************************//
        public override void Update(GameTime gameTime)
        {
            if (isDetect == false)
            {
                mFadeDelay -= gameTime.ElapsedGameTime.Ticks;
                rectTime -= gameTime.ElapsedGameTime.Milliseconds;

                if (mFadeDelay <= 0)
                {
                    mFadeDelay = .035; //Reset the Fade delay
                    rectangle.Height -= (int)rectTime;
                    rectangle.Width -= (int)rectTime;
                    mAlphaValue -= mFadeIncrement;//Decrement the fade value for the oilDot
                }
                if (mAlphaValue < 10)
                {
                    Visible = false;
                }
            }
            base.Update(gameTime);
        }
        //***********************************************************************//
        public bool CheckCollision(Rectangle rect)
        {
            Rectangle spriterect = new Rectangle((int)heliSitePosition.X, (int)heliSitePosition.Y, rectangle.Width, rectangle.Height);
            return spriterect.Intersects(rect);
        }
        //***********************************************************************//
        public override void Draw(GameTime gameTime)
        {
            // Get the current spritebatch
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            sBatch.Draw(heliSiteTexutre, heliSitePosition, rectangle, Color.White); // Draw the ship// ..position..

            base.Draw(gameTime);
        }
        //***********************************************************************//
    }
}
