using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using TheWaver.Core;

namespace TheWaver
{
    class Timer : DrawableGameComponent
    {
        float timeElapsed;

        public Timer(Game game)
            : base(game)
        { }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }


        public float Value
        {
            get { return timeElapsed; }
            set { this.timeElapsed = value; }
        }

        public void Reset()
        {
            timeElapsed = 0;
        }
    }
}
