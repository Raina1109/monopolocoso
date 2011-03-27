using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheWaver.Core;
using TheWaver.StoryLevel01;
using TheWaver.StoryLevel02;

namespace TheWaver
{
    public class HeliObject: DrawableGameComponent
    {
        protected Texture2D heliTexutre;
        protected Rectangle rectangle;
        protected Vector2 heliPosition;
        protected SpriteBatch sBatch;
        bool isDetect;
        int mAlphaValue;
        int mFadeIncrement = 3;
        double rectTime = .035;
        double mFadeDelay = .0001;
        //***********************************************************************//
        public HeliObject(Game game, Texture2D heliTexutre, Rectangle rectangle, Vector2 heliPosition)
            : base(game)
        {
            this.heliTexutre = heliTexutre;
            this.rectangle = rectangle;
            this.heliPosition = heliPosition;
        }
        //***********************************************************************//
        public void Reset()
        {
            Visible = true;
            mAlphaValue = 255;
            isDetect = true;
        }
        //***********************************************************************//
        /*
         *         public float ModelRotation
        {
            get { return modelRotation; }
            set { modelRotation = value; }
        }
         */
        public int WIDTH
        {
            get { return heliTexutre.Width; }
        }
        //***********************************************************************//
        public int HEIGHT
        {
            get { return heliTexutre.Height; }
        }
        //***********************************************************************//
        public float POSITION_X
        {
            get { return heliPosition.X; }
        }
        //***********************************************************************//
        public float POSITION_Y
        {
            get { return heliPosition.Y; }
        }
        //***********************************************************************//
        public void PutinStartPosition()
        {
            heliPosition = new Vector2(555, 555);
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
            Rectangle spriterect = new Rectangle((int)heliPosition.X, (int)heliPosition.Y, rectangle.Width, rectangle.Height);
            return spriterect.Intersects(rect);
        }
        //***********************************************************************//
        public override void Draw(GameTime gameTime)
        {
            // Get the current spritebatch
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            sBatch.Draw(heliTexutre, heliPosition, rectangle, Color.White); // Draw the ship// ..position..

            base.Draw(gameTime);
        }
        //***********************************************************************//
    }
}
