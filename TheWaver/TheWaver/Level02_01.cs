using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using TheWaver.Core;
using TheWaver.StoryLevel01;
using TheWaver.StoryLevel02;
//using TheWaver.Particles.Particles;

namespace TheWaver
{
    public class Level02_01 : GameScene
    {
        protected SpriteBatch spriteBatch = null;
        protected TimeSpan elapsedTime = TimeSpan.Zero;
        Texture2D oceanLevel02;

        //Player's ship variable.....................................................
        protected Player03 player;
        protected Texture2D playerShip, playerLeft, playerUp, playerDown;
        
        //Sidebar entities..........................................................
        protected Texture2D sideBar, loadingCrewBar;

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

        private ShipStatus shipStatus;
        protected int crewCollect01, crewCollect02, crewCollect03;
        bool isCountDown, isCountDown02, isCountDown03;
        bool showCountDown, showCountDown02, showCountDown03;

        //TIME VARIABLES.............................................................
        Timer timeElapsed, timeElapsed02, timeElapsed03;
        //ALPHA 1
        float startingTimeInSeconds;//240 seconds is 4 minutes
        int currentMinutes;
        int currentSeconds;
        int currentTime;
        float currentFloatTime;
        string properSeconds;
        //ALPHA 2
        float startingTimeInSeconds02;//240 seconds is 4 minutes
        int currentMinutes02;
        int currentSeconds02;
        int currentTime02;
        float currentFloatTime02;
        string properSeconds02;
        //ALPHA 3
        float startingTimeInSeconds03;//240 seconds is 4 minutes
        int currentMinutes03;
        int currentSeconds03;
        int currentTime03;
        float currentFloatTime03;
        string properSeconds03;
        //BOUNCER
        Rectangle playerRectangle;
        Rectangle rigRectangle;
        Rectangle rigRectangle02;
        Rectangle rigRectangle03;
        Vector2 rigPosition;
        Vector2 rigPosition02;
        Vector2 rigPosition03;

        //PROCEDD TO THE NEXT MISSION VARIABLES.....................................
        public bool isMissionFail = false, isMissionAccomplish = false;

        //FONTS.....................................................................
        SpriteFont levelFont;

        //HELICOPTERS SITE..........................................................
        HeliSite heliSite01, heliSite02, heliSite03;
        HeliObject heliObject01, heliObject02, heliObject03;
        public int saveProgress, saveProgress02, saveProgress03;

        /********************************************************************/
        public Level02_01(Game game, Texture2D oceanLevel02, Texture2D playerShip, Texture2D playerLeft, 
            Texture2D playerDown, Texture2D playerUp, Texture2D sideBar,GraphicsDeviceManager graphics,
            Texture2D chopperDown, Texture2D alpha01c, Texture2D alpha02c, Texture2D alpha03c, Texture2D loadingCrewBar,
            Texture2D STATE_PAUSE, Texture2D STATE_MISSIONACCOMPLISH, Texture2D STATE_MISSIONFAIL,
            SpriteFont levelFont)
            : base(game)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            this.loadingCrewBar = loadingCrewBar;
            //HELICOPTER SITE...............................................................
            //Alpha1...............................................
            rigPosition = new Vector2(860, 120);
            heliSite01 = new HeliSite(game, alpha01c, new Rectangle(0, 0, 335, 203), new Vector2(880, 395));
            heliSite01.Initialize();
            Components.Add(heliSite01);

            heliObject01 = new HeliObject(game, chopperDown, new Rectangle(0, 0, 100, 96), new Vector2(990, 470));
            heliObject01.Initialize();
            Components.Add(heliObject01);

            //Alpha2...............................................
            rigPosition02 = new Vector2(320, 590);
            heliSite02 = new HeliSite(game, alpha02c, new Rectangle(0, 0, 337,192), new Vector2(180, 390));
            heliSite02.Initialize();
            Components.Add(heliSite02);

            heliObject02 = new HeliObject(game, chopperDown, new Rectangle(0, 0, 100, 96), new Vector2(300,455));
            heliObject02.Initialize();
            Components.Add(heliObject02);

            //Alpha3...............................................
            rigPosition03 = new Vector2(1040, 590);
            heliSite03 = new HeliSite(game, alpha03c, new Rectangle(0, 0, 427, 172), new Vector2(693, 15));
            heliSite03.Initialize();
            Components.Add(heliSite03);

            heliObject03 = new HeliObject(game, chopperDown, new Rectangle(0, 0, 100, 96), new Vector2(800, 70));
            heliObject03.Initialize();
            Components.Add(heliObject03);
            //........................................................................................................//

            //PLAYER'S SHIP...........................................................................................
            this.playerShip = playerShip;
            this.playerDown = playerDown;
            this.playerLeft = playerLeft;
            this.playerUp = playerUp;

            player = new Player03(game, ref playerShip, ref playerLeft, ref playerDown, ref playerUp, ref graphics,  PlayerIndex.One, new Rectangle(0, 0, 100, 53));
            player.Initialize();
            Components.Add(player);
            //........................................................................................................//

            //DISPLAYS OCEAN BACKGROUND->TRANSFER LOCAL VARIABLE TO GLOBAL CLASS VARIABLE
            this.oceanLevel02 = oceanLevel02;

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
            isCountDown = false;
            isCountDown02 = false;
            isCountDown03 = false;
            showCountDown = false;
            showCountDown02 = false;
            showCountDown03 = false;

            //SIDEBAR.......................................................................
            this.sideBar = sideBar;

            //FONT..........................................................................
            this.levelFont = levelFont;

            //Initilizing timing............................................................
            timeElapsed = new Timer(game);
            timeElapsed.Value = 0;
            Components.Add(timeElapsed);

            timeElapsed02 = new Timer(game);
            timeElapsed02.Value = 0;
            Components.Add(timeElapsed02);

            timeElapsed03 = new Timer(game);
            timeElapsed03.Value = 0;
            Components.Add(timeElapsed03);
        }

        /********************************************************************/
        public override void Show()
        {
            MediaPlayer.Stop();
            crewCollect01 = 0;
            crewCollect02 = 0;
            crewCollect03 = 0;
            startingTimeInSeconds = 40;//SETS THE TIME FOR ALPHA 01
            startingTimeInSeconds02 = 50;//SETS THE TIME FOR ALPHA 02
            startingTimeInSeconds03 = 60;//SETS THE TIME FOR ALPHA 03
            saveProgress = 0;
            saveProgress02 = 0;
            saveProgress03 = 0;
            timeElapsed.Reset();
            timeElapsed02.Reset();
            timeElapsed03.Reset();

            if ((MediaPlayer.State == MediaState.Stopped) && (previousMediaState == MediaState.Playing))
                MediaPlayer.Play(audio.AboveAndBeyond);

            previousMediaState = MediaPlayer.State;

            isCountDown = false;
            isCountDown02 = false;
            isCountDown03 = false;

            isMissionFail = false;
            isMissionAccomplish = false;

            showCountDown = false;
            showCountDown02 = false;
            showCountDown03 = false;

            paused = false;
            pausePosition.X = (Game.Window.ClientBounds.Width - pauseRect.Width) / 2;
            pausePosition.Y = (Game.Window.ClientBounds.Height - pauseRect.Height) / 2;
            
            player.Reset();
            player.Visible = true;

            heliObject01.Reset();
            heliObject02.Reset();
            heliObject03.Reset();
            heliObject01.Visible = true;
            heliObject02.Visible = true;
            heliObject03.Visible = true;

            heliSite01.Reset();
            heliSite02.Reset();
            heliSite03.Reset();
            heliSite01.Visible = true;
            heliSite02.Visible = true;
            heliSite03.Visible = true;

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
                    CollisionManager(gameTime);
                }
            }
            rigRectangle = new Rectangle((int)rigPosition.X, (int)rigPosition.Y, heliObject01.WIDTH, heliObject01.HEIGHT);
            rigRectangle02 = new Rectangle((int)rigPosition02.X, (int)rigPosition02.Y, heliObject02.WIDTH, heliObject02.HEIGHT);
            rigRectangle03 = new Rectangle((int)rigPosition03.X, (int)rigPosition03.Y, heliObject03.WIDTH, heliObject03.HEIGHT);
            playerRectangle = new Rectangle((int)player.getPosition().X, (int)player.getPosition().Y, playerShip.Width, playerShip.Height);

            KeyboardState keyboard = Keyboard.GetState();

            if (playerRectangle.Intersects(rigRectangle))
            {
                if (playerRectangle.Left <= rigRectangle.Right)
                {
                    if (keyboard.IsKeyDown(Keys.A))
                        player.setPosition(new Vector2(player.getPosition().X + 5f, player.getPosition().Y));
                }
            }
            if (playerRectangle.Intersects(rigRectangle02))
            {
                if (playerRectangle.Right >= rigRectangle.Left)
                {
                    if (keyboard.IsKeyDown(Keys.D))
                        player.setPosition(new Vector2(player.getPosition().X - 5f, player.getPosition().Y));
                }
            }

            if (playerRectangle.Intersects(rigRectangle03))
            {
                if (playerRectangle.Top <= rigRectangle.Bottom)
                {
                    if (keyboard.IsKeyDown(Keys.W))
                        player.setPosition(new Vector2(player.getPosition().X, player.getPosition().Y + 5f));
                }
            }

            base.Update(gameTime);
        }
        /********************************************************************/
        public void CollisionManager(GameTime gameTime)
        {
            HeliSiteCollision(gameTime);
        }
        /********************************************************************/
        public void HeliSiteCollision(GameTime gameTime)
        {
            //ALPHA1.........................................
            if (heliSite01.CheckCollision(player.GetBounds()))
            {
                showCountDown = true;
                elapsedTime += gameTime.ElapsedGameTime;

                if (elapsedTime > TimeSpan.FromSeconds(1))
                {
                    elapsedTime -= TimeSpan.FromSeconds(1);
                    saveProgress++;
                }
                if (saveProgress > 10)
                {
                    crewCollect01 = 1;
                    heliSite01.IsDetect = false;
                    isCountDown = true;
                    showCountDown = false;
                }
            }

            //ALPHA2.........................................
            if (heliSite02.CheckCollision(player.GetBounds()))
            {
                showCountDown02 = true;
                elapsedTime += gameTime.ElapsedGameTime;

                if (elapsedTime > TimeSpan.FromSeconds(1))
                {
                    elapsedTime -= TimeSpan.FromSeconds(1);
                    saveProgress02++;
                }
                if (saveProgress02 > 10)
                {
                    crewCollect02 = 1;
                    heliSite02.IsDetect = false;
                    isCountDown02 = true;
                    showCountDown02 = false;
                }
            }

            //ALPHA3.........................................
            if (heliSite03.CheckCollision(player.GetBounds()))
            {
                showCountDown03 = true;
                elapsedTime += gameTime.ElapsedGameTime;

                if (elapsedTime > TimeSpan.FromSeconds(1))
                {
                    elapsedTime -= TimeSpan.FromSeconds(1);
                    saveProgress03++;
                }
                if (saveProgress03 > 10)
                {
                    crewCollect03 = 1;
                    heliSite03.IsDetect = false;
                    isCountDown03 = true;
                    showCountDown03 = false;
                }
            }
        }
        /********************************************************************/
        public void UpdateSpiffyTimer(GameTime gameTime)
        {
            //FOR ALPHA 1..............................................
            if (isCountDown == false)
            {
                currentFloatTime = startingTimeInSeconds - timeElapsed.Value;
                currentTime = Convert.ToInt32(currentFloatTime);
                currentMinutes = currentTime / 60;
                currentSeconds = currentTime % 60;
                properSeconds = currentSeconds.ToString("00");
            }

            //FOR ALPHA 2..............................................
            if (isCountDown02 == false)
            {
                currentFloatTime02 = startingTimeInSeconds02 - timeElapsed02.Value;
                currentTime02 = Convert.ToInt32(currentFloatTime02);
                currentMinutes02 = currentTime02 / 60;
                currentSeconds02 = currentTime02 % 60;
                properSeconds02 = currentSeconds.ToString("00");
            }

            //FOR ALPHA 3..............................................
            if (isCountDown03 == false)
            {
                currentFloatTime03 = startingTimeInSeconds03 - timeElapsed03.Value;
                currentTime03 = Convert.ToInt32(currentFloatTime03);
                currentMinutes03 = currentTime03 / 60;
                currentSeconds03 = currentTime03 % 60;
                properSeconds03 = currentSeconds03.ToString("00");
            }
        }
        /********************************************************************/
        public void DrawSpiffyTimer()
        {
            //ALPHA 1.......................................................
            string spiffyLabel, timerLabel;
            Vector2 timerPosition = new Vector2(1090, 440);
            Vector2 shadowPosition = new Vector2(1090+1, 440+1);

            if (isCountDown == false)
            {
                if ((currentSeconds != 0) && (currentSeconds > 0))
                {
                    if (currentSeconds >= 10)
                        spiffyLabel = currentMinutes + ":" + currentSeconds;
                    else
                        spiffyLabel = currentMinutes + ":" + properSeconds;

                    timerLabel = spiffyLabel;
                    spriteBatch.DrawString(levelFont, timerLabel, timerPosition, Color.White);
                    if ((currentSeconds <= 10) && (currentSeconds >= 1))
                        spriteBatch.DrawString(levelFont, timerLabel, timerPosition, Color.White);
                }
            }
            if (showCountDown == true)
            {
                spriteBatch.Draw(loadingCrewBar, new Vector2(1050, 470), Color.White);
                spriteBatch.DrawString(levelFont, saveProgress.ToString() +"%", new Vector2(1075, 510), Color.White);

            }

            //ALPHA 2.......................................................
            string spiffyLabel02, timerLabel02;
            Vector2 timerPosition02 = new Vector2(320,415);
            Vector2 shadowPosition02 = new Vector2(320+ 1, 415+ 1);

            if (isCountDown02 == false)
            {
                if ((currentSeconds02 != 0) && (currentSeconds02 > 0))
                {

                    if (currentSeconds02 >= 10)
                        spiffyLabel02 = currentMinutes02 + ":" + currentSeconds02;
                    else
                        spiffyLabel02 = currentMinutes02 + ":" + properSeconds02;

                    timerLabel02 = spiffyLabel02;
                    spriteBatch.DrawString(levelFont, timerLabel02, timerPosition02, Color.White);
                    if ((currentSeconds02 <= 10) && (currentSeconds02 >= 1))
                        spriteBatch.DrawString(levelFont, timerLabel02, timerPosition02, Color.White);
                }
            }
            if (showCountDown02 == true)
            {
                spriteBatch.Draw(loadingCrewBar, new Vector2(325, 440), Color.White);
                spriteBatch.DrawString(levelFont, saveProgress02.ToString() + "%", new Vector2(370, 480), Color.White);

            }

            //ALPHA 3.......................................................
            string spiffyLabel03, timerLabel03;
            Vector2 timerPosition03 = new Vector2(1170, 220);
            Vector2 shadowPosition03 = new Vector2(1170 + 1, 220 + 1);

            if (isCountDown03 == false)
            {
                if ((currentSeconds03 != 0) && (currentSeconds03 > 0))
                {

                    if (currentSeconds03 >= 10)
                        spiffyLabel03 = currentMinutes03 + ":" + currentSeconds03;
                    else
                        spiffyLabel03 = currentMinutes03 + ":" + properSeconds03;

                    timerLabel03 = spiffyLabel03;
                    spriteBatch.DrawString(levelFont, timerLabel03, timerPosition03, Color.White);
                    if ((currentSeconds03 <= 10) && (currentSeconds03 >= 1))
                        spriteBatch.DrawString(levelFont, timerLabel03, timerPosition03, Color.White);
                }
            }
            if (showCountDown03 == true)
            {
                spriteBatch.Draw(loadingCrewBar, new Vector2(1030, 30), Color.White);
                spriteBatch.DrawString(levelFont, saveProgress03.ToString() + "%", new Vector2(1080, 70), Color.White);

            }
            //...............................................................//

            //Mission Fail..........................................................
            //ALPHA01
            if (isCountDown == true)
            {
                if ((currentSeconds <= 0) && currentMinutes <= 0)
                {
                    spriteBatch.Draw(STATE_MISSIONFAIL, new Vector2(0, 0), Color.White);
                    MediaPlayer.Stop();
                }
            }
            //ALPHA02
            if (isCountDown02 == true)
            {
                if ((currentSeconds02 <= 0) && currentMinutes02 <= 0)
                {
                    spriteBatch.Draw(STATE_MISSIONFAIL, new Vector2(0, 0), Color.White);
                    MediaPlayer.Stop();
                }
            }
            //ALPHA03
            if (isCountDown == true)
            {
                if ((currentSeconds03 <= 0) && currentMinutes03 <= 0)
                {
                    spriteBatch.Draw(STATE_MISSIONFAIL, new Vector2(0, 0), Color.White);
                    MediaPlayer.Stop();
                }
            }
        }
        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.CornflowerBlue) ;

            spriteBatch.Draw(oceanLevel02, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(sideBar, new Vector2(0, 0), Color.White);

            base.Draw(gameTime);

            int totalCollect = 0;
            totalCollect = crewCollect01 + crewCollect02 + crewCollect03;
            if (crewCollect01 == 1)
                spriteBatch.DrawString(levelFont,"Alpha 1 crew secure.",new Vector2(15,490),Color.White);
            if (crewCollect02 == 1)
                spriteBatch.DrawString(levelFont, "Alpha 2 crew secure.", new Vector2(15,515), Color.White);
            if (crewCollect03 == 1)
                spriteBatch.DrawString(levelFont, "Alpha 3 crew secure.", new Vector2(15,540), Color.White);
            if(totalCollect == 3)
            {
                spriteBatch.Draw(STATE_MISSIONACCOMPLISH, new Vector2(0, 0), Color.White);
                isMissionAccomplish = true;
            }

            DrawSpiffyTimer();//Displays timer

            if (paused)// Draw the "pause" text
                spriteBatch.Draw(STATE_PAUSE, new Vector2(0, 0), Color.White);
        }
        /********************************************************************/
    }
}
