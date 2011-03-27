using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using TheWaver.Core;
using TheWaver.StoryLevel01;

namespace TheWaver
{
    public class Player02 : DrawableGameComponent
    {
        //Playership variables
        protected Texture2D playerShipTexture, cannon, crosshair;

        protected PlayerIndex playerIndex;
        protected Vector2 position;
        protected Vector2 CrosPotition;
        protected Vector2 cannonPosition;
        public float cannonRotation;
        protected Rectangle spriteRectangle;
        public Rectangle cannonBallRect;

        //Audio
        private AudioLibrary audio;

        const int maxCannonBalls = 1;
        Core.GameObject[] cannonBalls;


        public bool disArm = false;

        protected SetupShip testA;

        //Score variables
        protected int score;
        protected TimeSpan elapsedTime = TimeSpan.Zero;

        protected float timeElapsed;

        //Player ship health variable
        protected int healthpts, hullPower, weaponPower, speedPower;

        protected SpriteBatch sBatch;

        Rectangle viewportRect, enemy;
        GraphicsDeviceManager graphics;

        public Vector2 cannonBallVelocity;
        public bool isEnemyAlive;

        public Player02(Game game, ref Texture2D playerShipTexture, Texture2D crosshair, ref GraphicsDeviceManager graphics, ref Texture2D cannon, ref Texture2D plasma, PlayerIndex playerID, Rectangle rectangle)
            : base(game)
        {
            this.playerShipTexture = playerShipTexture;
            this.playerIndex = playerID;
            this.spriteRectangle = rectangle;
            this.cannon = cannon;
            this.graphics = graphics;
            this.crosshair = crosshair;
            cannonBallVelocity = Vector2.Zero;

            // Get the audio library
            audio = (AudioLibrary)Game.Services.GetService(typeof(AudioLibrary));

            cannonPosition.X = position.X + playerShipTexture.Width / 2;
            cannonPosition.Y = position.Y + playerShipTexture.Height / 2;

            //Initilizing the position
            position = new Vector2(578, 394);
            CrosPotition = new Vector2();
            //cannonPosition = new Vector2();
            cannonBalls = new Core.GameObject[maxCannonBalls];
            for (int i = 0; i < maxCannonBalls; i++)
            {
                cannonBalls[i] = new Core.GameObject(plasma);
            }
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            viewportRect = new Rectangle(0, 0,graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

        }
        /********************************************************************/
        public Vector2 getPosition()
        {
            return position;
        }
        /********************************************************************/
        //When the player goes to the main menu, all of the game setting will reset to its normal state.
        public void Reset()
        {
            //This ship will be at the center
            position = new Vector2(162, 432);
 
            healthpts = 100;
        }
        /********************************************************************/
        public void setPosition(Vector2 pos)
        {
            position = pos;
        }
        /********************************************************************/
        //Making score public, so other classes can access the information
        public int Score
        {
            get { return score; }
            set
            {
                if (value < 0)
                    score = 0;
                else
                    score = value;
            }
        }
        /********************************************************************/
        //Making score public, so other classes can access the information
        public int HealthPoints
        {
            get { return healthpts; }
            set
            {
                if (value < 0)
                    healthpts = 0;
                else
                    healthpts = value;
            }
        }
        /********************************************************************/
        public float Timer
        {
            get { return timeElapsed; }
            set
            {
                if (value < 0)
                    timeElapsed = 0;
                else
                    timeElapsed = value;
            }
        }
        /********************************************************************/
        //Updating the player's status
        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            position = ClampToViewport(position);

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                score++;
            }

            HandleInput(playerIndex);//Calling the controllers setting

            cannonPosition.X = position.X + playerShipTexture.Width / 2;
            cannonPosition.Y = position.Y + playerShipTexture.Height / 2;

            MouseState mouse = Mouse.GetState();

            //Calculate the distance from the square to the mouse's X and Y position
            float XDistance = cannonPosition.X - mouse.X;
            float YDistance = cannonPosition.Y - mouse.Y;

            CrosPotition.X = mouse.X - 1817;
            CrosPotition.Y = mouse.Y - 830;
            //cannon.rotation = MathHelper.Clamp(cannon.rotation, -MathHelper.Pi*2, 0);
            cannonRotation = (float)Math.Atan2(-YDistance, -XDistance);


            UpdateCannonBalls();

            base.Update(gameTime);
        }
        /********************************************************************/
        protected void HandleInput(PlayerIndex thePlayerIndex)
        {
            // Move the ship with xbox controller
            GamePadState gamepadstatus = GamePad.GetState(thePlayerIndex);
            position.Y += (int)((gamepadstatus.ThumbSticks.Left.Y * 3) * -2);
            position.X += (int)((gamepadstatus.ThumbSticks.Left.X * 3) * 2);

            //only keyboard
            if (thePlayerIndex == PlayerIndex.One)
            {
                HandlePlayer1KeyBoard();
            }
        }
        /********************************************************************/
        //Keyboard function
        private void HandlePlayer1KeyBoard()
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            /////////////////////////////////////////////////
            if (keyboard.IsKeyDown(Keys.W))
                position.Y -= 3;
            if (keyboard.IsKeyDown(Keys.S))
                position.Y += 3;
            if (keyboard.IsKeyDown(Keys.A))
                position.X -= 3;
            if (keyboard.IsKeyDown(Keys.D))
                position.X += 3;
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                FireCannonBall();

            }

        }
        /********************************************************************/
        public void UpdateCannonBalls()
        {
            foreach (Core.GameObject ball in cannonBalls)
            {
                if (ball.alive)
                {
                    ball.position += ball.velocity;

                    if (!viewportRect.Contains(new Point((int)ball.position.X, (int)ball.position.Y)))
                    {
                        ball.alive = false;
                        continue;
                    }
                    cannonBallRect = new Rectangle((int)ball.position.X,(int)ball.position.Y,ball.sprite.Width,ball.sprite.Height);
                }
                //ball.alive = isBallAlive;
                //if (cannonBallRect.Intersects(enemy))
                //{
                //    isEnemyAlive = false;
                //    ball.alive = false;
                //    healthpts += 20;
                //    //player.isBallAlive = false;
                //    audio.Explosion02.Play();
                //}
                
            }
        }
        //public void PassingEnemyEntity(Rectangle enemies, bool isAlive)
        //{
        //    enemy = enemies;
        //    isEnemyAlive = isAlive;
        //}
        /********************************************************************/
        //public void IsPlasmaAlive(bool isBallAlive)
        //{
        //    this.isBallAlive = isBallAlive;
        //}
        /********************************************************************/
        public Rectangle GetBounds()
        {
            //return new Rectangle(10, 30, 200, 160);
            return new Rectangle((int)position.X, (int)position.Y,
                spriteRectangle.Width, spriteRectangle.Height);
        }
        /********************************************************************/
        public Rectangle GetPlasmaBounds()
        {
            return cannonBallRect;
        }
        /********************************************************************/
        private Vector2 ClampToViewport(Vector2 vector)
        {
            //(SpriteBatch)Game.GraphicsDevice.Viewport;
            Viewport vp = Game.GraphicsDevice.Viewport;
            vector.X = MathHelper.Clamp(vector.X, 100, 700);
            vector.Y = MathHelper.Clamp(vector.Y, 300, vp.Y + 400);
            return vector;
        }
        /********************************************************************/
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        /********************************************************************/
        public void FireCannonBall()
        {
            foreach (Core.GameObject ball in cannonBalls)
            {
                if (!ball.alive)
                {
                    audio.PlasmaFire.Play();
                    ball.alive = true;
                    ball.position = cannonPosition - ball.center;
                    cannonBallVelocity = ball.position;
                    ball.velocity = new Vector2(
                        (float)Math.Cos(cannonRotation),
                        (float)Math.Sin(cannonRotation)) * 25.0f;
                    return;
                }
            }
        }
        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            // Get the current spritebatch
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

                foreach (Core.GameObject ball in cannonBalls)
                {
                    if (ball.alive)
                        sBatch.Draw(ball.sprite, ball.position, Color.White);
                }

            sBatch.Draw(playerShipTexture, position, spriteRectangle, Color.White); // Draw the ship// ..position..

            sBatch.Draw(cannon,
              cannonPosition,
              null,
              Color.White,
              cannonRotation,
              new Vector2(cannon.Width / 2, cannon.Height / 2), 0.1f,
              SpriteEffects.None, 0);

            sBatch.Draw(crosshair, CrosPotition, Color.White);

            base.Draw(gameTime);
        }
        /********************************************************************/
    }
}
