using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using TheWaver.Core;
using TheWaver.StoryLevel01;
using TheWaver.StoryLevel02;

namespace TheWaver
{
    class Level02_03 : GameScene
    {
        protected SpriteBatch spriteBatch = null;
        protected TimeSpan elapsedTime = TimeSpan.Zero;

        //Player's ship variable.....................................................
        protected Player player;
        protected Texture2D playerShip, playerLeft, playerUp, playerDown, crosshair;
        protected Texture2D hullGreen, hullYellow, hullOrange, hullRed;

        //Sidebar entities..........................................................
        protected Texture2D sideBar;

        //DISPLAY STATES............................................................
        Texture2D STATE_PAUSE, STATE_MISSIONACCOMPLISH, STATE_MISSIONFAIL;
        //Pause State...............................................................
        protected Texture2D pauseScene;
        protected Vector2 pausePosition;
        protected Rectangle pauseRect;
        protected bool paused;

        //AUDIO....................................................................
        private AudioLibrary audio;
        private MediaState previousMediaState;

        //Display Health, Scores....................................................
        protected UpgradePower healthpts, score, oilPercent;

        private ShipStatus shipStatus;

        //TIME VARIABLES.............................................................
        Timer timeElapsed;
        float startingTimeInSeconds;//240 seconds is 4 minutes
        int currentMinutes;
        int currentSeconds;
        int currentTime;
        float currentFloatTime;
        string properSeconds;
        public int dCount;
        Rectangle rigRectangle;
        Rectangle playerRectangle;
        Vector2 rigPosition;
        Color colors;
        GraphicsDeviceManager graphics;
        Rectangle viewportRect;

        //PROCEDD TO THE NEXT MISSION VARIABLES.....................................
        public bool isMissionFail = false, isMissionAccomplish = false;

        //FONTS.....................................................................
        SpriteFont levelFont;

        /********************************************************************/
        public Level02_03(Game game, Texture2D playerShip, Texture2D playerLeft,
            Texture2D playerDown, Texture2D playerUp, Texture2D sideBar,
            Texture2D crosshair, Texture2D cannon, Texture2D plasma, GraphicsDeviceManager graphics,
            Texture2D hullGreen, Texture2D hullYellow, Texture2D hullOrange, Texture2D hullRed,
            Texture2D STATE_PAUSE, Texture2D STATE_MISSIONACCOMPLISH, Texture2D STATE_MISSIONFAIL,
            SpriteFont levelFont)
            : base(game)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            //PLAYER'S SHIP................................................................
            this.playerShip = playerShip;
            this.playerDown = playerDown;
            this.playerLeft = playerLeft;
            this.playerUp = playerUp;
            this.crosshair = crosshair;
            this.hullGreen = hullGreen;
            this.hullOrange = hullOrange;
            this.hullRed = hullRed;
            this.hullYellow = hullYellow;

            player = new Player(game, ref playerShip, ref playerLeft, ref playerDown, ref playerUp, ref crosshair, ref graphics, ref cannon, ref plasma ,PlayerIndex.One, new Rectangle(0, 0, 100, 53));
            player.Initialize();
            Components.Add(player);

            //DISPLAYS THE STATE.............................................................
            this.STATE_MISSIONACCOMPLISH = STATE_MISSIONACCOMPLISH;
            this.STATE_MISSIONFAIL = STATE_MISSIONFAIL;
            this.STATE_PAUSE = STATE_PAUSE;

            //GET THE AUDIO LIBRARY.........................................................
            audio = (AudioLibrary)Game.Services.GetService(typeof(AudioLibrary));
            MediaPlayer.IsRepeating = true;
            MediaPlayer.IsShuffled = false;
            MediaPlayer.Volume = .25f;
            previousMediaState = MediaState.Playing;

            shipStatus = (ShipStatus)Game.Services.GetService(typeof(ShipStatus));

            //SIDEBAR.......................................................................
            this.sideBar = sideBar;

            //FONT..........................................................................
            this.levelFont = levelFont;

            //Initilzing the health point output text/font..................................
            healthpts = new UpgradePower(game, levelFont, Color.Lavender);
            healthpts.Position = new Vector2(115, 90);
            healthpts.Value = 100;
            Components.Add(healthpts);

            //Initilizing the score point output text/font..................................
            score = new UpgradePower(game, levelFont, Color.Lavender);
            score.Position = new Vector2(180, 230);
            score.Value = 0;
            Components.Add(score);

            //Initilizing timing............................................................
            timeElapsed = new Timer(game);
            timeElapsed.Value = 0;
            Components.Add(timeElapsed);
        }

        /********************************************************************/
        public override void Show()
        {
            MediaPlayer.Stop();
            startingTimeInSeconds = 120;//SETS THE TIME
            timeElapsed.Reset();

            if ((MediaPlayer.State == MediaState.Stopped) && (previousMediaState == MediaState.Playing))
                MediaPlayer.Play(audio.BackMusic);

            previousMediaState = MediaPlayer.State;

            isMissionFail = false;
            isMissionAccomplish = false;

            paused = false;
            pausePosition.X = (Game.Window.ClientBounds.Width - pauseRect.Width) / 2;
            pausePosition.Y = (Game.Window.ClientBounds.Height - pauseRect.Height) / 2;


            player.Reset();
            player.Visible = true;

            base.Show();
        }
        /********************************************************************/
        public override void Hide()
        {
            MediaPlayer.Stop();
            base.Hide();
        }
        /********************************************************************/
        public bool Paused
        {
            get { return paused; }
            set
            {
                paused = value;
                if (paused)
                    MediaPlayer.Pause();
                else
                    MediaPlayer.Resume();
            }
        }
        /********************************************************************/
        public override void Update(GameTime gameTime)
        {
            if (!paused)
            {
                if (isMissionAccomplish == false)
                {
                    UpdateSpiffyTimer(gameTime);
                    healthpts.Value = player.HealthPoints;//passing values to UpgradePower.cs = Player.cs
                    score.Value = player.Score;//Same as above, but different entities
                }
            }
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.A))
                player.setPosition(new Vector2(player.getPosition().X + 5f, player.getPosition().Y));
            if (keyboard.IsKeyDown(Keys.D))
                player.setPosition(new Vector2(player.getPosition().X - 5f, player.getPosition().Y));
            if (keyboard.IsKeyDown(Keys.W))
                player.setPosition(new Vector2(player.getPosition().X, player.getPosition().Y + 5f));

            base.Update(gameTime);
        }
        /********************************************************************/
        //Timer
        public void UpdateSpiffyTimer(GameTime gameTime)
        {
            currentFloatTime = startingTimeInSeconds - timeElapsed.Value;

            currentTime = Convert.ToInt32(currentFloatTime);
            currentMinutes = currentTime / 60;
            currentSeconds = currentTime % 60;
            properSeconds = currentSeconds.ToString("00");
        }
        /********************************************************************/
        public void DrawSpiffyTimer()
        {
            Vector2 timerPosition = new Vector2(180, 160);
            Vector2 shadowPosition = new Vector2(181, 161);

            string spiffyLabel, timerLabel;

            if ((currentSeconds != 0) && (currentSeconds > 0))
            {

                if (currentSeconds >= 10)
                { spiffyLabel = currentMinutes + ":" + currentSeconds; }
                else
                { spiffyLabel = currentMinutes + ":" + properSeconds; }

                timerLabel = spiffyLabel;
                spriteBatch.DrawString(levelFont, timerLabel, timerPosition, Color.White);
                if ((currentSeconds <= 10) && (currentSeconds >= 1))
                { spriteBatch.DrawString(levelFont, timerLabel, timerPosition, Color.White); }
            }

            //if ((currentSeconds <= 0) && currentMinutes <= 0)
            //{
            //    spriteBatch.Draw(STATE_MISSIONFAIL, new Vector2(0, 0), Color.White);
            //    MediaPlayer.Stop();

            //    //NEED AN IF-STATEMENT REFERING TO MISSION ACCOMPLISH/FAILURE
            //}
        }
        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Draw(sideBar, new Vector2(0, 0), Color.White);

            if (healthpts.Value >= 71)
                spriteBatch.Draw(hullGreen, new Vector2(65, 40), Color.White);
            else if (healthpts.Value <= 70 && healthpts.Value >= 51)
                spriteBatch.Draw(hullYellow, new Vector2(65, 40), Color.White);
            else if (healthpts.Value <= 50 && healthpts.Value >= 21)
                spriteBatch.Draw(hullOrange, new Vector2(65, 40), Color.White);
            else if (healthpts.Value <= 20)
                spriteBatch.Draw(hullRed, new Vector2(65, 40), Color.White);

            base.Draw(gameTime);

            if (healthpts.Value <= 0)
            {
                spriteBatch.Draw(STATE_MISSIONFAIL, new Vector2(0, 0), Color.White);
                isMissionFail = true;
            }

            if (paused)// Draw the "pause" text
                spriteBatch.Draw(pauseScene, new Vector2(0, 0), Color.White);

            DrawSpiffyTimer();//Displays timer
        }
        /********************************************************************/
    }
}

