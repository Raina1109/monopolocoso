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
    public class level01 : GameScene
    {
        protected SpriteBatch spriteBatch = null;

        //Setup the time
        protected TimeSpan elapsedTime = TimeSpan.Zero;

        //Variables
        protected Texture2D ocean, sideBar, statusUpdateENEMY01, statusUpdateMAYDAY, statusUpdateBoxes;
        protected Texture2D playerShip, plasma, enemyJetFighterTopView;
        protected Texture2D STATE_MISSIONFAIL, STATE_MISSIONACCOMPLISH;
        protected Texture2D hullGreen, hullYellow, hullOrange, hullRed;
        protected Texture2D box01, box02;
        protected Texture2D oilSpillTexture, oilRig02, cannon, crosshair;
        protected int oilPercentileRange =0;
        public bool isMissionFail = false, isMissionAccomplish = false;
        protected Texture2D playerLeft, playerUp, playerDown;

        protected int speedPower;

        const float maxEnemyHeight = -5.1f;//Default 0.1f
        const float minEnemyHeight = 0.3f;
        const float maxEnemyVelocity = 8.0f;
        const float minEnemyVelocity = 3.0f;
        const int MAXENEMYJETFIGHTER = 1;
        Core.GameObject[] EnemyJetObject;
        Random random = new Random();
        const int maxEnemyCannonBalls = 2;
        Core.GameObject[] enemyCannonBalls;

        //Pause State
        protected Texture2D pauseScene;
        protected Vector2 pausePosition;
        protected Rectangle pauseRect;
        protected bool paused;

        //Audio
        private AudioLibrary audio;
        private MediaState previousMediaState;

        //Display Health, Scores
        protected UpgradePower healthpts, score, oilPercent;

        private ShipStatus shipStatus;
        //Playership
        protected Player player;
        //Health Package
        protected HealthPackage healthPackage, healthPackage02, healthPackage03, healthPackage04;
        public ExplosivePackage explosivePackage, explosivePackage02, explosivePackage03,
            explosivePackage04, explosivePackage05, explosivePackage06, explosivePackage07, explosivePackage08,
            explosivePackage09, explosivePackage10, explosivePackage11, explosivePackage12, explosivePackage13,
            explosivePackage14, explosivePackage15, explosivePackage16;
        //OilSpill
        protected int MAXDOT = 15;
        protected OilSpill[] oilSpill01, oilSpill02, oilSpill03, oilSpill04, oilSpill05, oilSpill06,
            oilSpill07, oilSpill08, oilSpill09, oilSpill10, oilSpill11, oilSpill12, oilSpill13, oilSpill14, oilSpill15, oilSpill16,
            oilSpill17, oilSpill18, oilSpill19, oilSpill20, oilSpill21, oilSpill22, oilSpill23, oilSpill24, oilSpill25, oilSpill26,
            oilSpill27, oilSpill28, oilSpill29, oilSpill30, oilSpill31, oilSpill32, oilSpill33, oilSpill34, oilSpill35, oilSpill36;
        

        //Time Variable
        Timer timeElapsed;
        float startingTimeInSeconds;//240 seconds is 4 minutes
        int currentMinutes;
        int currentSeconds;
        int currentTime;
        float currentFloatTime;
        string properSeconds;
        public int dCount;
        SpriteFont font;
        Rectangle rigRectangle;
        Rectangle playerRectangle;
        Vector2 rigPosition;
        Color colors;
        GraphicsDeviceManager graphics;
        Rectangle viewportRect;

        //Explosion + Smoke Effects///////////
        //ParticleSystem explosion;
        //ParticleSystem smoke;

        public level01(Game game, Texture2D playerShip, Texture2D playerLeft, Texture2D playerDown, Texture2D playerUp, Texture2D crosshair, GraphicsDeviceManager graphics, Texture2D cannon, Texture2D plasma, Texture2D ocean, Texture2D oilRig02,
            Texture2D sideBar, Texture2D statusUpdateBoxes, Texture2D statusUpdateMAYDAY, Texture2D statusUpdateENEMY01, Texture2D hullGreen, 
            Texture2D hullYellow, Texture2D hullOrange, Texture2D hullRed, Texture2D oilSpillTexture, Texture2D pauseScene,
            Texture2D STATE_MISSIONACCOMPLISH, Texture2D STATE_MISSIONFAIL, Texture2D box01, Texture2D box02, Texture2D enemyJetFighterTopView, 
            SpriteFont font)
            : base(game)
        {
            //classVariables = parameterVariable
            this.playerShip = playerShip;
            this.pauseScene = pauseScene;
            this.ocean = ocean;
            this.sideBar = sideBar;
            this.font = font;
            this.STATE_MISSIONFAIL = STATE_MISSIONFAIL;
            this.STATE_MISSIONACCOMPLISH = STATE_MISSIONACCOMPLISH;
            this.statusUpdateBoxes = statusUpdateBoxes;
            this.statusUpdateMAYDAY = statusUpdateMAYDAY; 
            this.statusUpdateENEMY01 = statusUpdateENEMY01;
            this.plasma = plasma;
            this.oilRig02 = oilRig02;
            this.graphics = graphics;
            this.crosshair = crosshair;
            this.playerDown = playerDown;
            this.playerLeft = playerLeft;
            this.playerUp = playerUp;
            //this.enemyJetFighterTopView = enemyJetFighterTopView;

            EnemyJetObject = new Core.GameObject[MAXENEMYJETFIGHTER];
            for (int i = 0; i < MAXENEMYJETFIGHTER; i++)
                EnemyJetObject[i] = new Core.GameObject(enemyJetFighterTopView);

            enemyCannonBalls = new Core.GameObject[maxEnemyCannonBalls];
            for (int i = 0; i < maxEnemyCannonBalls; i++)
                enemyCannonBalls[i] = new Core.GameObject(plasma);

            //Ship's hull status
            this.hullGreen = hullGreen;
            this.hullYellow = hullYellow;
            this.hullOrange = hullOrange;
            this.hullRed = hullRed;

            // Get the current sprite batch
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            viewportRect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            // Get the audio library
            audio = (AudioLibrary)Game.Services.GetService(typeof(AudioLibrary));

            shipStatus = (ShipStatus)Game.Services.GetService(typeof(ShipStatus));

            rigPosition = new Vector2(850, -60);
            
            // Setup music player
            MediaPlayer.IsRepeating = true;
            MediaPlayer.IsShuffled = false;
            MediaPlayer.Volume = .25f;

            previousMediaState = MediaState.Playing;

            //Initilzing the health point output text/font
            healthpts = new UpgradePower(game, font, Color.Lavender);
            healthpts.Position = new Vector2(115, 90);
            healthpts.Value = 100;
            Components.Add(healthpts);

            //Initilizing the score point output text/font
            score = new UpgradePower(game, font, Color.Lavender);
            score.Position = new Vector2(180, 230);
            score.Value = 0;
            Components.Add(score);

            timeElapsed = new Timer(game);
            timeElapsed.Value = 0;
            Components.Add(timeElapsed);

            //HEALTH PACKAGES/////////////////////
            healthPackage = new HealthPackage(game, box01, new Rectangle(0, 0, 50, 47), new Vector2(555,555));
            healthPackage.Initialize();
            Components.Add(healthPackage);

            healthPackage02 = new HealthPackage(game, box01, new Rectangle(0, 0, 50, 47), new Vector2(333, 333));
            healthPackage02.Initialize();
            Components.Add(healthPackage02);

            healthPackage03 = new HealthPackage(game, box01, new Rectangle(0, 0, 50, 47), new Vector2(924, 339));
            healthPackage03.Initialize();
            Components.Add(healthPackage03);

            healthPackage04 = new HealthPackage(game, box01, new Rectangle(0, 0, 50, 47), new Vector2(864, 444));
            healthPackage04.Initialize();
            Components.Add(healthPackage04);

            //EXPLOSIVE PACKAGE///////////////////
            ExplosiveBoxInitilize(game, box02);

            //Declares the oil variables! DO NOT DELETE
            DeclarOil();
            OilInitilize(game, oilSpillTexture);

            //Initilizing the playership
            //Player's parameter (game, image,only player one, (size of the image, coordinate of the image))
            player = new Player(game, ref playerShip, ref playerLeft, ref playerDown, ref playerUp, ref crosshair, ref graphics, ref cannon, ref plasma, PlayerIndex.One, new Rectangle(0, 0, 100, 53));
            player.Initialize();
            Components.Add(player);

            colors = new Color();

            colors = Color.White;
        }
        /********************************************************************/
        public override void Show()
        {
            healthPackage.PutinStartPosition();
            MediaPlayer.Stop();
            startingTimeInSeconds = 120;//SETS THE TIME

            if ((MediaPlayer.State == MediaState.Stopped) && (previousMediaState == MediaState.Playing))
            {
                MediaPlayer.Play(audio.BackMusic);
            }
            previousMediaState = MediaPlayer.State;

            oilPercentileRange = 0;

            //Resets the player setting
            player.Reset();

            timeElapsed.Reset();

            //Resets the health package
            healthPackage.Reset();
            healthPackage02.Reset();
            healthPackage03.Reset();
            healthPackage04.Reset();

            //Resets the explosive package
            ResetExplosive();

            //Resets the Oil Percent value to 100..
            //oilPercent.Value = 100; 

            //Resets the oil spill
            ResetOil();

            isMissionFail = false;
            isMissionAccomplish = false;
            paused = false;
            pausePosition.X = (Game.Window.ClientBounds.Width - pauseRect.Width) / 2;
            pausePosition.Y = (Game.Window.ClientBounds.Height - pauseRect.Height) / 2;

            player.Visible = true;

            player.SpeedPower = speedPower;

            base.Show();
        }
        /********************************************************************/
        private void HandlePowerSourceSprite(GameTime gameTime)
        {
            //COLLISION_OIL_DETECT();//OIL COLLISION FUNCTION DETECTION

            ShipToOilCollision();

            //PLAYER COLLIDES WITH THE HEALTH BOX
            HealthBoxCollision();
            ////////////////////////////////////////
            //PLAYER COLLIDES WITH THE EXPLOSIVE BOX
            ExplosiveBoxCollis();
            ////////////////////////////////////////
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
        public int SpeedPower
        {
            get { return speedPower; }
            set
            { speedPower = value; }
        }
        /********************************************************************/
        public override void Update(GameTime gameTime)
        {
            if (!paused)
            {
                if (isMissionAccomplish == false || isMissionFail == true)
                {
                    UpdateEnemies();
                    UpdateEnemyDefesne();
                    FireEnemyCannonBalls();
                    UpdateSpiffyTimer(gameTime);
                    healthpts.Value = player.HealthPoints;//passing values to UpgradePower.cs = Player.cs
                    score.Value = player.Score;//Same as above, but different entities

                    HandlePowerSourceSprite(gameTime);
                }
            }
            rigRectangle = new Rectangle((int)rigPosition.X, (int)rigPosition.Y,oilRig02.Width, oilRig02.Height);
            playerRectangle = new Rectangle((int)player.getPosition().X, (int)player.getPosition().Y,
                playerShip.Width, playerShip.Height);

            KeyboardState keyboard = Keyboard.GetState();


            if (playerRectangle.Intersects(rigRectangle))
            {
                //colors = Color.Red;
                if (playerRectangle.Left <= rigRectangle.Right)
                {
                    if (keyboard.IsKeyDown(Keys.A))
                        player.setPosition(new Vector2(player.getPosition().X + 5f, player.getPosition().Y));
                }

                if (playerRectangle.Right >= rigRectangle.Left)
                {
                    if (keyboard.IsKeyDown(Keys.D))
                        player.setPosition(new Vector2(player.getPosition().X - 5f, player.getPosition().Y));
                }

                if (playerRectangle.Top <= rigRectangle.Bottom)
                {
                    if (keyboard.IsKeyDown(Keys.W))
                        player.setPosition(new Vector2(player.getPosition().X, player.getPosition().Y + 5f));
                }

            }

            base.Update(gameTime);
        }
        /********************************************************************/
        public void UpdateEnemies()
        {
            foreach (Core.GameObject enemy in EnemyJetObject)
            {
                if (enemy.alive)
                {
                    enemy.velocity =new Vector2(player.getPosition().X - enemy.position.X, player.getPosition().Y - enemy.position.Y);
                    enemy.velocity.Normalize();

                    enemy.velocity = Vector2.Multiply(enemy.velocity, 3);
                    enemy.rotation = (float)Math.Atan2((player.getPosition().Y - enemy.position.Y), (player.getPosition().X - enemy.position.X));
                    enemy.position += enemy.velocity;

                    if (!viewportRect.Contains(new Point((int)enemy.position.X, (int)enemy.position.Y)))
                    {
                        enemy.alive =false;
                    }
                }
                else
                {
                    enemy.alive =true;
                    int randoms = random.Next(3);

                    if (randoms == 0)
                    {
                    enemy.position.X = random.Next(1280);
                    enemy.position.Y = 0;
                    }
                    else if (randoms == 1)
                    {
                    enemy.position.X = random.Next(1280);
                    enemy.position.Y = 720;
                    }
                    else if (randoms == 2)
                    {
                    enemy.position.X = 0;
                    enemy.position.Y = random.Next(720);
                    }
                    else if (randoms == 3)
                    {
                        enemy.position.X = 1280;
                        enemy.position.Y = random.Next(720);
                    }
                }

                Rectangle cannonBallRect = player.cannonBallRect;
                Rectangle enemyRect = new Rectangle((int)enemy.position.X,(int)enemy.position.Y,100,130);

                if(cannonBallRect.Intersects(enemyRect))
                {
                    enemy.alive = false;
                    player.Score += 20;
                    audio.Explosion02.Play();
                }
                if (enemyRect.Intersects(player.GetBounds()))
                {
                    enemy.alive = false;
                    player.HealthPoints -= 10;
                    audio.Explosion.Play();
                }
            }
        }
        /********************************************************************/
        public void FireEnemyCannonBalls()
        {
            foreach (Core.GameObject enemy in EnemyJetObject)
            {
                int fireCannonBall = random.Next(0, 50);
                if (fireCannonBall == 1)
                {
                    foreach (Core.GameObject ball in enemyCannonBalls)
                    {
                        if (ball.alive == false)
                        {
                            ball.alive = true;
                            ball.position = enemy.position;
                            ball.velocity.Y = 5.0f;
                            break;
                        }
                    }
                }
            }
        }
        /********************************************************************/
        public void UpdateEnemyDefesne()
        {
            foreach (Core.GameObject ball in enemyCannonBalls)
            {
                if (ball.alive)
                {
                    ball.position += ball.velocity;
                    if (!viewportRect.Contains(new Point((int)ball.position.X, (int)ball.position.Y)))
                    {
                        ball.alive = false;
                        continue;
                    }

                    Rectangle cannonBallRect = new Rectangle((int)ball.position.X, (int)ball.position.Y, 100, (53));
  
                    if (cannonBallRect.Intersects(player.GetBounds()))
                    {
                        ball.alive = false;
                        player.HealthPoints -= 10;
                        audio.Explosion.Play();
                        //isPhealth = true;
                    }

                }
            }
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
                spriteBatch.DrawString(font, timerLabel, timerPosition, Color.White);
                if ((currentSeconds <= 10) && (currentSeconds >= 1))
                { spriteBatch.DrawString(font, timerLabel, timerPosition, Color.White); }
            }

            if ((currentSeconds <= 0) && currentMinutes <= 0)
            {
                spriteBatch.Draw(STATE_MISSIONFAIL, new Vector2(0, 0), Color.White);
                MediaPlayer.Stop();

                //NEED AN IF-STATEMENT REFERING TO MISSION ACCOMPLISH/FAILURE
            }
        }
        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            bool AlertMessage01 = true, AlterMessage02 = true;

            spriteBatch.Draw(ocean, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(oilRig02, new Vector2(850, -60), Color.White);

            base.Draw(gameTime);

            foreach (Core.GameObject enemy in EnemyJetObject)
            {
                if (enemy.alive)
                    spriteBatch.Draw(enemy.sprite, enemy.position,null, Color.White, enemy.rotation, new Vector2(enemy.sprite.Width / 2, enemy.sprite.Height / 2), 1f, SpriteEffects.None, 0);
            }

            foreach (Core.GameObject ball in enemyCannonBalls)
            {
                if (ball.alive == true)
                    spriteBatch.Draw(ball.sprite, ball.position, Color.Yellow);
            }

            spriteBatch.Draw(sideBar, new Vector2(0, 0), Color.White);


            spriteBatch.DrawString(font, "-Clean the oil spill less than \n  2 minutes", new Vector2(10,370), Color.White);
            
            DrawSpiffyTimer();//Displays timer

            if (healthpts.Value >= 71)
                spriteBatch.Draw(hullGreen, new Vector2(65, 40), Color.White);
            else if (healthpts.Value <= 70 && healthpts.Value >= 51)
                spriteBatch.Draw(hullYellow, new Vector2(65, 40), Color.White);
            else if (healthpts.Value <= 50 && healthpts.Value >= 21)
                spriteBatch.Draw(hullOrange, new Vector2(65, 40), Color.White);
            else if (healthpts.Value <= 20)
            {
                //Displays the emergency message
                spriteBatch.Draw(hullRed, new Vector2(65, 40), Color.White);
                AlertMessage01 = false;
                AlterMessage02 = false;
                spriteBatch.Draw(statusUpdateMAYDAY, new Vector2(10, 550), Color.White);
            }


            if ((oilPercentileRange > 360) && (AlterMessage02 == true))
            {
                //Displays the enemy message
                AlertMessage01 = false;
                spriteBatch.Draw(statusUpdateENEMY01, new Vector2(10, 550), Color.White);
            }
            if (AlertMessage01 == true)
            {
                //Displays the boxes/cargo message
                spriteBatch.Draw(statusUpdateBoxes, new Vector2(10, 550), Color.White);
            }

            if(oilPercentileRange <= 10)
                spriteBatch.DrawString(font, "0%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 10 && oilPercentileRange <= 20)
                spriteBatch.DrawString(font, "5%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 20 && oilPercentileRange <= 40)
                spriteBatch.DrawString(font, "9%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 40 && oilPercentileRange <= 60)
                spriteBatch.DrawString(font, "14%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 60 && oilPercentileRange <= 80)
                spriteBatch.DrawString(font, "19%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 80 && oilPercentileRange <= 100)
                spriteBatch.DrawString(font, "22%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 100 && oilPercentileRange <= 120)
                spriteBatch.DrawString(font, "29%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 120 && oilPercentileRange <= 140)
                spriteBatch.DrawString(font, "35%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 140 && oilPercentileRange <= 160)
                spriteBatch.DrawString(font, "42%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 160 && oilPercentileRange <= 180)
                spriteBatch.DrawString(font, "52%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 180 && oilPercentileRange <= 200)
                spriteBatch.DrawString(font, "61%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 200 && oilPercentileRange <= 220)
                spriteBatch.DrawString(font, "66%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 220 && oilPercentileRange <= 240)
                spriteBatch.DrawString(font, "69%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 240 && oilPercentileRange <= 260)
                spriteBatch.DrawString(font, "72%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 260 && oilPercentileRange <= 280)
                spriteBatch.DrawString(font, "77%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 280 && oilPercentileRange <= 300)
                spriteBatch.DrawString(font, "79%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 300 && oilPercentileRange <= 320)
                spriteBatch.DrawString(font, "84%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 320 && oilPercentileRange <= 340)
                spriteBatch.DrawString(font, "86%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 340 && oilPercentileRange <= 360)
                spriteBatch.DrawString(font, "87%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 360 && oilPercentileRange <= 380)
                spriteBatch.DrawString(font, "88%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 380 && oilPercentileRange <= 300)
                spriteBatch.DrawString(font, "89%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 300 && oilPercentileRange <= 320)
                spriteBatch.DrawString(font, "90%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 320 && oilPercentileRange <= 340)
                spriteBatch.DrawString(font, "91%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 340 && oilPercentileRange <= 360)
                spriteBatch.DrawString(font, "92%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 360 && oilPercentileRange <= 380)
                spriteBatch.DrawString(font, "93%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 380 && oilPercentileRange <= 400)
                spriteBatch.DrawString(font, "94%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 400 && oilPercentileRange <= 420)
                spriteBatch.DrawString(font, "95%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 420 && oilPercentileRange <= 430)
                spriteBatch.DrawString(font, "97%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 440 && oilPercentileRange <= 460)
                spriteBatch.DrawString(font, "96%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 460 && oilPercentileRange <= 480)
                spriteBatch.DrawString(font, "97%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 480 && oilPercentileRange <= 400)
                spriteBatch.DrawString(font, "98%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 500 && oilPercentileRange <= 535)
                spriteBatch.DrawString(font, "99%", new Vector2(180, 292), Color.White);
            else if (oilPercentileRange >= 535)
            {
                spriteBatch.DrawString(font, "100%", new Vector2(180, 292), Color.White);
                spriteBatch.Draw(STATE_MISSIONACCOMPLISH, new Vector2(0, 0), Color.White);
                isMissionAccomplish = true;
            }

            if (healthpts.Value <= 0)
            {
                spriteBatch.Draw(STATE_MISSIONFAIL, new Vector2(0, 0), Color.White);
                isMissionFail = true;
            }

            if (paused)// Draw the "pause" text
                spriteBatch.Draw(pauseScene, new Vector2(0, 0), Color.White);
        }
        /********************************************************************/
        /********************************************************************/
        //Oil dots array variables
        void DeclarOil()
        {
            oilSpill01 = new OilSpill[MAXDOT];
            oilSpill02 = new OilSpill[MAXDOT];
            oilSpill03 = new OilSpill[MAXDOT];
            oilSpill04 = new OilSpill[MAXDOT];
            oilSpill05 = new OilSpill[MAXDOT];
            oilSpill06 = new OilSpill[MAXDOT];
            oilSpill07 = new OilSpill[MAXDOT];
            oilSpill08 = new OilSpill[MAXDOT];
            oilSpill09 = new OilSpill[MAXDOT];
            oilSpill10 = new OilSpill[MAXDOT];
            oilSpill11 = new OilSpill[MAXDOT];
            oilSpill12 = new OilSpill[MAXDOT];
            oilSpill13 = new OilSpill[MAXDOT];
            oilSpill14 = new OilSpill[MAXDOT];
            oilSpill15 = new OilSpill[MAXDOT];
            oilSpill16 = new OilSpill[MAXDOT];
            oilSpill17 = new OilSpill[MAXDOT];
            oilSpill18 = new OilSpill[MAXDOT];
            oilSpill19 = new OilSpill[MAXDOT];
            oilSpill20 = new OilSpill[MAXDOT];
            oilSpill21 = new OilSpill[MAXDOT];
            oilSpill22 = new OilSpill[MAXDOT];
            oilSpill23 = new OilSpill[MAXDOT];
            oilSpill24 = new OilSpill[MAXDOT];
            oilSpill25 = new OilSpill[MAXDOT];
            oilSpill26 = new OilSpill[MAXDOT];
            oilSpill27 = new OilSpill[MAXDOT];
            oilSpill28 = new OilSpill[MAXDOT];
            oilSpill29 = new OilSpill[MAXDOT];
            oilSpill30 = new OilSpill[MAXDOT];
            oilSpill31 = new OilSpill[MAXDOT];
            oilSpill32 = new OilSpill[MAXDOT];
            oilSpill33 = new OilSpill[MAXDOT];
            oilSpill34 = new OilSpill[MAXDOT];
            oilSpill35 = new OilSpill[MAXDOT];
            oilSpill36 = new OilSpill[MAXDOT];
        }
        void OilInitilize(Game game, Texture2D oilSpillTexture)
        {
            for (int i = 0; i < MAXDOT; i++)
            {
                oilSpill01[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(956, (409 + (10 * i))));
                oilSpill01[i].Initialize();
                Components.Add(oilSpill01[i]);

                oilSpill02[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(980, (403 + (15 * i))));
                oilSpill02[i].Initialize();
                Components.Add(oilSpill02[i]);

                oilSpill03[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(990, (338 + (19 * i))));
                oilSpill03[i].Initialize();
                Components.Add(oilSpill03[i]);

                oilSpill04[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1001, (375 + (20 * i))));
                oilSpill04[i].Initialize();
                Components.Add(oilSpill04[i]);

                oilSpill05[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1016, (360 + (20 * i))));
                oilSpill05[i].Initialize();
                Components.Add(oilSpill05[i]);

                oilSpill06[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1029, (348 + (20 * i))));
                oilSpill06[i].Initialize();
                Components.Add(oilSpill06[i]);

                oilSpill07[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1044, (336 + (20 * i))));
                oilSpill07[i].Initialize();
                Components.Add(oilSpill07[i]);

                oilSpill08[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1065, (333 + (20 * i))));
                oilSpill08[i].Initialize();
                Components.Add(oilSpill08[i]);

                oilSpill09[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1089, (343 + (22 * i))));
                oilSpill09[i].Initialize();
                Components.Add(oilSpill01[i]);

                oilSpill10[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1092, (360 + (20 * i))));
                oilSpill10[i].Initialize();
                Components.Add(oilSpill10[i]);

                oilSpill11[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1115, (375 + (18 * i))));
                oilSpill11[i].Initialize();
                Components.Add(oilSpill11[i]);

                oilSpill12[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1134, (387 + (10 * i))));
                oilSpill12[i].Initialize();
                Components.Add(oilSpill12[i]);

                oilSpill13[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1149, (396 + (8 * i))));
                oilSpill13[i].Initialize();
                Components.Add(oilSpill13[i]);

                oilSpill14[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1167, (409 + (4 * i))));
                oilSpill14[i].Initialize();
                Components.Add(oilSpill14[i]);
                //-----
                oilSpill15[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(401, (58 + (3 * i))));
                oilSpill15[i].Initialize();
                Components.Add(oilSpill15[i]);

                oilSpill16[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(415, (45 + (6 * i))));
                oilSpill16[i].Initialize();
                Components.Add(oilSpill16[i]);

                oilSpill17[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(436, (47 + (9 * i))));
                oilSpill17[i].Initialize();
                Components.Add(oilSpill17[i]);

                oilSpill18[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(452, (31 + (12 * i))));
                oilSpill18[i].Initialize();
                Components.Add(oilSpill18[i]);

                oilSpill19[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(470, (34 + (9 * i))));
                oilSpill19[i].Initialize();
                Components.Add(oilSpill19[i]);

                oilSpill20[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(484, (49 + (6 * i))));
                oilSpill20[i].Initialize();
                Components.Add(oilSpill20[i]);

                oilSpill21[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(505, (50 + (3 * i))));
                oilSpill21[i].Initialize();
                Components.Add(oilSpill21[i]);

                oilSpill22[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(521, (45 + (2 * i))));
                oilSpill22[i].Initialize();
                Components.Add(oilSpill22[i]);
                //------
                oilSpill23[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(344, (532 + (3 * i))));
                oilSpill23[i].Initialize();
                Components.Add(oilSpill23[i]);

                oilSpill24[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(361, (543 + (7 * i))));
                oilSpill24[i].Initialize();
                Components.Add(oilSpill24[i]);

                oilSpill25[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(379, (532 + (8 * i))));
                oilSpill25[i].Initialize();
                Components.Add(oilSpill25[i]);

                oilSpill26[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(400, (537 + (7 * i))));
                oilSpill26[i].Initialize();
                Components.Add(oilSpill26[i]);

                oilSpill27[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(418, (544 + (6 * i))));
                oilSpill27[i].Initialize();
                Components.Add(oilSpill27[i]);

                oilSpill28[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(439, (539 + (4 * i))));
                oilSpill28[i].Initialize();
                Components.Add(oilSpill28[i]);
                //--------
                oilSpill29[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1094, (57 + (3 * i))));
                oilSpill29[i].Initialize();
                Components.Add(oilSpill29[i]);

                oilSpill30[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1118, (57 + (4 * i))));
                oilSpill30[i].Initialize();
                Components.Add(oilSpill30[i]);

                oilSpill31[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1133, (67 + (5 * i))));
                oilSpill31[i].Initialize();
                Components.Add(oilSpill31[i]);

                oilSpill32[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1154, (78 + (6 * i))));
                oilSpill32[i].Initialize();
                Components.Add(oilSpill32[i]);

                oilSpill33[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1173, (73 + (5 * i))));
                oilSpill33[i].Initialize();
                Components.Add(oilSpill33[i]);

                oilSpill34[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1182, (82 + (4 * i))));
                oilSpill34[i].Initialize();
                Components.Add(oilSpill34[i]);

                oilSpill35[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1203, (93 + (3 * i))));
                oilSpill35[i].Initialize();
                Components.Add(oilSpill35[i]);

                oilSpill36[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(1226, (91 + (2 * i))));
                oilSpill36[i].Initialize();
                Components.Add(oilSpill36[i]);
            }
        }
        //Health Boxes01
        void HealthBoxCollision() 
        {
            if (healthPackage.CheckCollision(player.GetBounds()))
            {
                if (healthPackage.Visible == true)//Plays the collision box noise
                {
                    audio.HealthSound.Play();
                    player.HealthPoints += 30;
                }
                healthPackage.Visible = false;//Oil disappears
            }

            if (healthPackage02.CheckCollision(player.GetBounds()))
            {
                if (healthPackage02.Visible == true)//Plays the collision box noise
                {
                    audio.HealthSound.Play();
                    player.HealthPoints += 30;
                }
                healthPackage02.Visible = false;//Oil disappears
            }

            if (healthPackage03.CheckCollision(player.GetBounds()))
            {
                if (healthPackage03.Visible == true)//Plays the collision box noise
                {
                    audio.HealthSound.Play();
                    player.HealthPoints += 30;
                }
                healthPackage03.Visible = false;//Oil disappears
            }

            if (healthPackage04.CheckCollision(player.GetBounds()))
            {
                if (healthPackage04.Visible == true)//Plays the collision box noise
                {
                    audio.HealthSound.Play();
                    player.HealthPoints += 30;
                }
                healthPackage04.Visible = false;//Oil disappears
            }
        }
        //Explosive Boxes02
        void ExplosiveBoxCollis()
        {
            if (explosivePackage.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage.Visible = false;//The box disappears
            }

            if (explosivePackage02.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage02.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage02.Visible = false;//The box disappears
            }

            if (explosivePackage03.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage03.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage03.Visible = false;//The box disappears
            }

            if (explosivePackage04.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage04.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage04.Visible = false;//The box disappears
            }

            if (explosivePackage05.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage05.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage05.Visible = false;//The box disappears
            }

            if (explosivePackage06.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage06.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage06.Visible = false;//The box disappears
            }

            if (explosivePackage07.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage07.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage07.Visible = false;//The box disappears
            }

            if (explosivePackage08.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage08.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage08.Visible = false;//The box disappears
            }

            if (explosivePackage09.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage09.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage09.Visible = false;//The box disappears
            }

            if (explosivePackage10.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage10.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage10.Visible = false;//The box disappears
            }

            if (explosivePackage11.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage11.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage11.Visible = false;//The box disappears
            }

            if (explosivePackage12.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage12.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage12.Visible = false;//The box disappears
            }

            if (explosivePackage13.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage13.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage13.Visible = false;//The box disappears
            }

            if (explosivePackage14.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage14.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage14.Visible = false;//The box disappears
            }

            if (explosivePackage15.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage15.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage15.Visible = false;//The box disappears
            }

            if (explosivePackage16.CheckCollision(player.GetBounds()))
            {
                if (explosivePackage16.Visible == true)//Plays the collision box noise
                {
                    audio.Explosion.Play();
                    player.HealthPoints -= 40;
                }
                explosivePackage16.Visible = false;//The box disappears
            }

        }
        //Explosive Boxes02
        void ExplosiveBoxInitilize(Game game, Texture2D box02)
        {
            explosivePackage = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(1199 , 672));
            explosivePackage.Initialize();
            Components.Add(explosivePackage);

            explosivePackage02 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(699, 324));
            explosivePackage02.Initialize();
            Components.Add(explosivePackage02);

            explosivePackage03 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(596, 297));
            explosivePackage03.Initialize();
            Components.Add(explosivePackage02);

            explosivePackage04 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(699, 164));
            explosivePackage04.Initialize();
            Components.Add(explosivePackage04);

            explosivePackage05 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(335, 478));
            explosivePackage05.Initialize();
            Components.Add(explosivePackage05);

            explosivePackage06 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(906, 571));
            explosivePackage06.Initialize();
            Components.Add(explosivePackage06);

            explosivePackage07 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(1211, 546));
            explosivePackage07.Initialize();
            Components.Add(explosivePackage07);

            explosivePackage08 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(764, 603));
            explosivePackage08.Initialize();
            Components.Add(explosivePackage08);

            explosivePackage09 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(590, 246));
            explosivePackage09.Initialize();
            Components.Add(explosivePackage09);

            explosivePackage10 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(774, 351));
            explosivePackage10.Initialize();
            Components.Add(explosivePackage10);

            explosivePackage11 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(521, 351));
            explosivePackage11.Initialize();
            Components.Add(explosivePackage11);

            explosivePackage12 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(412, 270));
            explosivePackage12.Initialize();
            Components.Add(explosivePackage12);

            explosivePackage13 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(803, 267));
            explosivePackage13.Initialize();
            Components.Add(explosivePackage13);

            explosivePackage14 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(620, 61));
            explosivePackage14.Initialize();
            Components.Add(explosivePackage14);

            explosivePackage15 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(1088, 283));
            explosivePackage15.Initialize();
            Components.Add(explosivePackage15);

            explosivePackage16 = new ExplosivePackage(game, box02, new Rectangle(0, 0, 50, 47), new Vector2(960, 260));
            explosivePackage16.Initialize();
            Components.Add(explosivePackage16);
        }
        //COLLISION FUNCTIONS
        void ShipToOilCollision()
        {
            for (int i = 0; i < MAXDOT; i++)
            {
                if (oilSpill01[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill01[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                        //INSERT SPIRIT
                    }
                    oilSpill01[i].Visible = false;//Oil disappears
                }

                if (oilSpill02[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill02[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill02[i].Visible = false;//Oil disappears
                }

                if (oilSpill03[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill03[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill03[i].Visible = false;//Oil disappears
                }

                if (oilSpill04[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill04[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill04[i].Visible = false;//Oil disappears
                }

                if (oilSpill05[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill05[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill05[i].Visible = false;//Oil disappears
                }

                if (oilSpill06[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill06[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill06[i].Visible = false;//Oil disappears
                }

                if (oilSpill07[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill07[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill07[i].Visible = false;//Oil disappears
                }

                if (oilSpill08[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill08[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill08[i].Visible = false;//Oil disappears
                }

                if (oilSpill09[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill09[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill09[i].Visible = false;//Oil disappears
                }

                if (oilSpill10[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill10[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill10[i].Visible = false;//Oil disappears
                }

                if (oilSpill11[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill11[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill11[i].Visible = false;//Oil disappears
                }

                if (oilSpill12[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill12[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill12[i].Visible = false;//Oil disappears
                }

                if (oilSpill13[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill13[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill13[i].Visible = false;//Oil disappears
                }

                //88888888888888//
                if (oilSpill14[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill14[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill14[i].Visible = false;//Oil disappears
                }

                if (oilSpill15[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill15[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill15[i].Visible = false;//Oil disappears
                }

                if (oilSpill16[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill16[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill16[i].Visible = false;//Oil disappears
                }

                if (oilSpill17[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill17[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill17[i].Visible = false;//Oil disappears
                }

                if (oilSpill18[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill18[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill18[i].Visible = false;//Oil disappears
                }

                if (oilSpill19[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill19[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill19[i].Visible = false;//Oil disappears
                }

                if (oilSpill20[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill20[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill20[i].Visible = false;//Oil disappears
                }

                if (oilSpill21[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill21[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill21[i].Visible = false;//Oil disappears
                }

                if (oilSpill22[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill22[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill22[i].Visible = false;//Oil disappears
                }

                if (oilSpill23[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill23[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill23[i].Visible = false;//Oil disappears
                }

                if (oilSpill24[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill24[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill24[i].Visible = false;//Oil disappears
                }

                if (oilSpill25[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill25[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill25[i].Visible = false;//Oil disappears
                }

                if (oilSpill26[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill26[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill26[i].Visible = false;//Oil disappears
                }

                if (oilSpill27[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill27[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill27[i].Visible = false;//Oil disappears
                }

                if (oilSpill28[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill28[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill28[i].Visible = false;//Oil disappears
                }

                if (oilSpill29[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill29[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill29[i].Visible = false;//Oil disappears
                }

                if (oilSpill30[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill30[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill30[i].Visible = false;//Oil disappears
                }

                if (oilSpill31[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill31[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill31[i].Visible = false;//Oil disappears
                }

                if (oilSpill32[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill32[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill32[i].Visible = false;//Oil disappears
                }

                if (oilSpill32[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill32[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill32[i].Visible = false;//Oil disappears
                }

                if (oilSpill33[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill33[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill33[i].Visible = false;//Oil disappears
                }

                if (oilSpill34[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill34[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill34[i].Visible = false;//Oil disappears
                }

                if (oilSpill35[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill35[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill35[i].Visible = false;//Oil disappears
                }

                if (oilSpill36[i].CheckCollision(player.GetBounds()))
                {
                    if (oilSpill36[i].Visible == true)//Plays the collision box noise
                    {
                        player.Score += 10;
                        
                        oilPercentileRange += 1;
                    }
                    oilSpill36[i].Visible = false;//Oil disappears
                }

            }
        }
        //RESETS KEY
        void ResetOil() 
        {
            for (int i = 0; i < MAXDOT; i++)
            {
                oilSpill01[i].Reset();
                oilSpill02[i].Reset();
                oilSpill03[i].Reset();
                oilSpill04[i].Reset();
                oilSpill05[i].Reset();
                oilSpill06[i].Reset();
                oilSpill07[i].Reset();
                oilSpill08[i].Reset();
                oilSpill09[i].Reset();
                oilSpill10[i].Reset();
                oilSpill11[i].Reset();
                oilSpill12[i].Reset();
                oilSpill13[i].Reset();
                oilSpill14[i].Reset();
                oilSpill15[i].Reset();
                oilSpill16[i].Reset();
                oilSpill17[i].Reset();
                oilSpill18[i].Reset();
                oilSpill19[i].Reset();
                oilSpill20[i].Reset();
                oilSpill21[i].Reset();
                oilSpill22[i].Reset();
                oilSpill23[i].Reset();
                oilSpill24[i].Reset();
                oilSpill25[i].Reset();
                oilSpill26[i].Reset();
                oilSpill27[i].Reset();
                oilSpill28[i].Reset();
                oilSpill29[i].Reset();
                oilSpill30[i].Reset();
                oilSpill31[i].Reset();
                oilSpill32[i].Reset();
                oilSpill33[i].Reset();
                oilSpill34[i].Reset();
                oilSpill35[i].Reset();
                oilSpill36[i].Reset();
            }
        }
        void ResetExplosive()
        {
            explosivePackage.Reset();
            explosivePackage02.Reset();
            explosivePackage03.Reset();
            explosivePackage04.Reset();
            explosivePackage05.Reset();
            explosivePackage06.Reset();
            explosivePackage07.Reset();
            explosivePackage08.Reset();
            explosivePackage09.Reset();
            explosivePackage10.Reset();
            explosivePackage11.Reset();
            explosivePackage12.Reset();
            explosivePackage13.Reset();
            explosivePackage14.Reset();
            explosivePackage15.Reset();
            explosivePackage16.Reset();
        }
    }
}
