using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace TheWaver.Core
{
    class Enemy
    {
        Vector2 position;

        Texture2D picture;

        float radius = 40.0f;
        float speed = 150.0f;

        bool firingActive = false;
        bool firing = false;
        float fireSpeed = 1.0f;
        float totalTime = 0.0f;

        public bool FiringActive
        {
            set { firingActive = value; }
        }

        public bool Firing
        {
            set { firing = value; }
            get { return firing; }
        }

        public Enemy(Texture2D picture, Vector2 startPosition, float speed)
        {
            this.picture = picture;
            position = startPosition;
            this.speed = speed;
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public float Radius
        {
            get { return radius; }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(picture, position, null, Color.White, 0.0f, new Vector2(40.0f, 20.0f), 1.0f, SpriteEffects.None, 0.0f);
        }

        //public int CollisionBall(List<Fireball> fireballList)
        //{
        //    for (int i = 0; i < fireballList.Count; i++)
        //    {
        //        if ((fireballList[i].Position - position).Length() < radius)
        //            return i;
        //    }

        //    return -1;
        //}

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position.X -= speed * elapsed;

            if (firingActive)
            {
                totalTime += elapsed;

                if (totalTime > fireSpeed)
                {
                    totalTime = 0.0f;
                    firing = true;
                }
            }
        }
    }
}
