using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using TheWaver.Core;
using TheWaver.StoryLevel01;

namespace TheWaver
{
    public class Player03: DrawableGameComponent
    {
        //Playership variables
        protected Texture2D playerShipTexture;
        protected Texture2D playerTexture;
        protected Texture2D playerLeft;
        protected Texture2D playerUp;
        protected Texture2D playerDown;

        protected PlayerIndex playerIndex;
        protected Vector2 position;
        protected Rectangle spriteRectangle;


        public bool disArm = false;

        protected SetupShip testA;

        //Score variables
        protected int score;
        protected TimeSpan elapsedTime = TimeSpan.Zero;

        protected float timeElapsed;

        //Player ship health variable
        protected int healthpts, hullPower, weaponPower, speedPower;

        protected SpriteBatch sBatch;

        Rectangle viewportRect;
        GraphicsDeviceManager graphics;

        Rectangle verticalRectangle;
        Rectangle playerRectangle;

        //Audio
        private AudioLibrary audio;
        /********************************************************************/
        public Player03(Game game, ref Texture2D playerShipTexture, ref Texture2D playerLeft, ref Texture2D playerDown, ref Texture2D playerUp, ref GraphicsDeviceManager graphics, PlayerIndex playerID, Rectangle rectangle)
            : base(game)
        {
            this.playerShipTexture = playerShipTexture;
            this.playerIndex = playerID;
            this.spriteRectangle = rectangle;
            this.graphics = graphics;
            this.playerDown = playerDown;
            this.playerLeft = playerLeft;
            this.playerUp = playerUp;
            
            // Get the audio library
            audio = (AudioLibrary)Game.Services.GetService(typeof(AudioLibrary));

            playerTexture = playerShipTexture;
            //Initilizing the position
            position = new Vector2();

            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));


            verticalRectangle = new Rectangle(0, 0, 50, 100);
            playerRectangle = this.spriteRectangle;

            viewportRect = new Rectangle(0, 0,
               graphics.GraphicsDevice.Viewport.Width,
               graphics.GraphicsDevice.Viewport.Height);

        }
        /********************************************************************/
        public Vector2 getPosition()
        {
            return position;
        }
        /********************************************************************/
        public void setPosition(Vector2 pos)
        {
            position = pos;
        }
        /********************************************************************/
        //When the player goes to the main menu, all of the game setting will reset to its normal state.
        public void Reset()
        {
            //This ship will be at the center
            position = new Vector2(578, 394);
            score = 0;
            healthpts = 100;
            timeElapsed = 0;
        }
        /********************************************************************/
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
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
            {
                playerTexture = playerUp;
                position.Y -= 2;
                playerRectangle = verticalRectangle;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                playerTexture = playerDown;
                position.Y += 2;
                playerRectangle = verticalRectangle;
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                playerTexture = playerLeft;
                position.X -= 2;
                playerRectangle = spriteRectangle;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                playerTexture = playerShipTexture;
                position.X += 2;
                playerRectangle = spriteRectangle;
            }
        }
        /********************************************************************/
        public Rectangle GetBounds()
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.D))
            {
                //return new Rectangle(10, 30, 200, 160);
                return new Rectangle((int)position.X, (int)position.Y,
                    spriteRectangle.Width, spriteRectangle.Height);
            }
            else
            {
                return new Rectangle((int)position.X, (int)position.Y,
                   verticalRectangle.Width, verticalRectangle.Height);
            }

        }
        /********************************************************************/
        private Vector2 ClampToViewport(Vector2 vector)
        {
            //(SpriteBatch)Game.GraphicsDevice.Viewport;
            Viewport vp = Game.GraphicsDevice.Viewport;
            vector.X = MathHelper.Clamp(vector.X, 300, 1185); //SCREEN |<-  ....  ->| left and right
            vector.Y = MathHelper.Clamp(vector.Y, 10, vp.Y + 660);//SCREEN |^ .. v | up and down
            return vector;
        }
        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            // Get the current spritebatch
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            sBatch.Draw(playerTexture, position, playerRectangle, Color.White); // Draw the ship// ..position..

            base.Draw(gameTime);
        }
        /********************************************************************/
    }
}
