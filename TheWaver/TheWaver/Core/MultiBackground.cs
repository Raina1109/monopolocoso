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
    class BackgroundLayer
    {
        public Texture2D picture;
        public Vector2 position = Vector2.Zero;
        public Vector2 offset = Vector2.Zero;
        public float depth = 0.0f;
        public float moveRate = 0.0f;
        public Vector2 pictureSize = Vector2.Zero;
        public Color color = Color.White;
    }

    class MultiBackground
    {
        private bool moving = false;
        private bool moveLeftRight = true;

        private Vector2 windowSize;

        private List<BackgroundLayer> layerList = new List<BackgroundLayer>();

        private SpriteBatch batch;

        public MultiBackground(GraphicsDeviceManager graphics)
        {
            windowSize.X = graphics.PreferredBackBufferWidth;
            windowSize.Y = graphics.PreferredBackBufferHeight;
            batch = new SpriteBatch(graphics.GraphicsDevice);
        }

        public void AddLayer(Texture2D picture, float depth, float moveRate)
        {
            BackgroundLayer layer = new BackgroundLayer();
            layer.picture = picture;
            layer.depth = depth;
            layer.moveRate = moveRate;
            layer.pictureSize.X = picture.Width;
            layer.pictureSize.Y = picture.Height;

            layerList.Add(layer);
        }

        public int CompareDepth(BackgroundLayer layer1, BackgroundLayer layer2)
        {
            if (layer1.depth < layer2.depth)
                return 1;
            if (layer1.depth > layer2.depth)
                return -1;
            if (layer1.depth == layer2.depth)
                return 0;
            return 0;
        }

        public void Move(float rate)
        {
            float moveRate = rate / 60.0f;

            foreach (BackgroundLayer layer in layerList)
            {
                float moveDistance = layer.moveRate * moveRate;

                if (!moving)
                {
                    if (moveLeftRight)
                    {
                        layer.position.X -= moveDistance;
                        layer.position.X = layer.position.X % layer.pictureSize.X;
                    }
                    else
                    {
                        layer.position.Y += moveDistance;
                        layer.position.Y = layer.position.Y % layer.pictureSize.Y;
                    }
                }
            }
        }

        public void Draw()
        {
            layerList.Sort(CompareDepth);

            batch.Begin();

            for (int i = 0; i < layerList.Count; i++)
            {
                if (!moveLeftRight)
                {
                    if (layerList[i].position.Y < windowSize.Y)
                    {
                        batch.Draw(layerList[i].picture, new Vector2(0.0f, layerList[i].position.Y), layerList[i].color);
                    }
                    if (layerList[i].position.Y > 0.0f)
                        batch.Draw(layerList[i].picture, new Vector2(0.0f, layerList[i].position.Y - layerList[i].pictureSize.Y), layerList[i].color);
                    else
                        batch.Draw(layerList[i].picture, new Vector2(0.0f, layerList[i].position.Y + layerList[i].pictureSize.Y), layerList[i].color);
                }
                else
                {
                    if (layerList[i].position.X < windowSize.X)
                    {
                        batch.Draw(layerList[i].picture, new Vector2(layerList[i].position.X, 0.0f), layerList[i].color);
                    }
                    if (layerList[i].position.X > 0.0f)
                        batch.Draw(layerList[i].picture, new Vector2(layerList[i].position.X - layerList[i].pictureSize.X, 0.0f), layerList[i].color);
                    else
                        batch.Draw(layerList[i].picture, new Vector2(layerList[i].position.X + layerList[i].pictureSize.X, 0.0f), layerList[i].color);

                }
            }


            batch.End();
        }

        public void SetMoveUpDown()
        {
            moveLeftRight = false;
        }

        public void SetMoveLeftRight()
        {
            moveLeftRight = true;
        }

        public void Stop()
        {
            moving = false;
        }

        public void StartMoving()
        {
            moving = true;
        }

        public void SetLayerPosition(int layerNumber, Vector2 startPosition)
        {
            if (layerNumber < 0 || layerNumber >= layerList.Count) return;

            layerList[layerNumber].position = startPosition;
        }

        public void SetLayerAlpha(int layerNumber, float percent)
        {
            if (layerNumber < 0 || layerNumber >= layerList.Count) return;

            float alpha = (percent / 100.0f);

            layerList[layerNumber].color = new Color(new Vector4(0.0f, 0.0f, 0.0f, alpha));
        }

        public void Update(GameTime gameTime)
        {

            foreach (BackgroundLayer layer in layerList)
            {
                float moveDistance = layer.moveRate / 60.0f;

                if (moving)
                {
                    if (moveLeftRight)
                    {
                        layer.position.X -= moveDistance;
                        layer.position.X = layer.position.X % layer.pictureSize.X;
                    }
                    else
                    {
                        layer.position.Y += moveDistance;
                        layer.position.Y = layer.position.Y % layer.pictureSize.Y;
                    }
                }
            }
        }
    }
}
