using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheWaver.Core;
using TheWaver.StoryLevel01;

namespace TheWaver
{
    public class OilSpill : DrawableGameComponent
    {
        protected Texture2D oilSpillTexture;
        protected Rectangle rectangle;
        protected Vector2 position;
        protected SpriteBatch sBatch;
        int mAlphaValue;
        int mFadeIncrement = 3;
        double rectTime = .035;
        double mFadeDelay = .0001;
        bool isDetect;
        public int oilDotRemove;

        public OilSpill(Game game, Texture2D oilSpillTexture, Rectangle rectangle, Vector2 position)
            : base(game)
        {
            this.oilSpillTexture = oilSpillTexture;
            this.rectangle.Height = rectangle.Height;
            this.rectangle.Width = rectangle.Width;
            this.position = position;
        }
        //***********************************************************************//
        public void Reset()
        {
            mAlphaValue = 255;
            rectangle.Width=38;
            rectangle.Height=37;
            oilDotRemove = 0;
            Visible = true;
            isDetect = true;
        }
        //***********************************************************************//
        public void PutinStartPosition()
        {
            position = new Vector2(555, 555);
            Enabled = false;
        }
        //***********************************************************************//
        public bool CheckCollision(Rectangle rect)
        {
            Rectangle spriterect = new Rectangle((int)position.X, (int)position.Y, rectangle.Width, rectangle.Height);
            return spriterect.Intersects(rect);
        }
        //***********************************************************************//
        public bool IsDetect
        {
            get { return isDetect; }
            set { this.isDetect = value; }
        }
        public int OilDotRemove
        {
            get { return oilDotRemove; }
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
                if (mAlphaValue < 20)
                {
                    Visible = false;
                    oilDotRemove = 1;
                }
            }
            base.Update(gameTime);
        }
        //***********************************************************************//
        public override void Draw(GameTime gameTime)
        {
            // Get the current spritebatch
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            sBatch.Draw(oilSpillTexture, position, rectangle, new Color(255, 255, 255,(byte)MathHelper.Clamp(mAlphaValue, 0, 255)));
    //        spriteBatch.Draw(mCatCreature, new Rectangle(0, 0, mCatCreature.Width, mCatCreature.Height),
    //new Color(255, 255, 255, (byte)MathHelper.Clamp(mAlphaValue, 0, 255)));

            base.Draw(gameTime);
        }
        //***********************************************************************//
    }
}
