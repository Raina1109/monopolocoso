using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using TheWaver.Core;
using TheWaver.Particles;
using TheWaver.StoryLevel01;
using TheWaver.StoryLevel02;

namespace TheWaver
{
    public class Level01_02 : GameScene
    {
        //Spritebatch................................................
        protected SpriteBatch spriteBatch = null;

        //Setup the time.............................................
        protected TimeSpan elapsedTime = TimeSpan.Zero;

        //Player's entities...........................................
        Texture2D playerShip, playerLeft, playerDown, playerUp;
        Texture2D hullGreen, hullYellow, hullOrange, hullRed;
        Texture2D cannon, plasma, playerRocket;
        protected Player01_2 player;
        Rectangle playerRectangle;

        //Enemy's entites...........................................
        const float maxEnemyHeight = -5.1f;//Default 0.1f
        const float minEnemyHeight = 0.3f;
        const float maxEnemyVelocity = 8.0f;
        const float minEnemyVelocity = 3.0f;
        const int MAXENEMYJETFIGHTER = 1;
        Core.GameObject[] EnemyJetObject;
        Random random = new Random();
        const int maxEnemyCannonBalls = 2;
        Core.GameObject[] enemyCannonBalls;
        Rectangle viewportRect;
        
        public enum EnemyType{EXPLOSIVEBOX, HEALTHBOX}
        List<Enemy> enemyShipList = new List<Enemy>();
        float totalTime = 0.0f;

        //BOXES...........................................
        Texture2D box01, box02;
        //SIDEBAR...........................................
        Texture2D sideBar;

        //Audio...........................................
        private AudioLibrary audio;
        private MediaState previousMediaState;

        //TIMER...........................................
        Timer timeElapsed;
        float startingTimeInSeconds;//240 seconds is 4 minutes
        int currentMinutes;
        int currentSeconds;
        int currentTime;
        float currentFloatTime;
        string properSeconds;

        //DISPLAY STRINGS....................................................
        protected UpgradePower healthpts;

        //MISSION ACCOMPLISH SCENE...........................................
        protected Texture2D STATE_MISSIONACCOMPLISH, STATE_MISSIONFAIL;
        protected bool missionComplete;

        //T || F -> Mission Accomplish or Fail...........................................
        public bool isMissionFail = false, isMissionAccomplish = false;

        //PAUSE SCENE...........................................
        protected Texture2D pauseScene;
        protected Vector2 pausePosition;
        protected Rectangle pauseRect;
        protected bool paused;

        protected int speedPower;

        SpriteFont font;

        ParticleSystem EXPOLSION_PARTICLES, SMOKE_PARTICLES;


        //Moving background...........................................
        MultiBackground oceanBackground;
        Texture2D ocean1;

        public Level01_02(Game game, Texture2D playerShip, Texture2D playerLeft, Texture2D playerDown, 
            Texture2D playerUp, Texture2D crosshair, GraphicsDeviceManager graphics, Texture2D cannon,
            Texture2D plasma, Texture2D sideBar, Texture2D box01, Texture2D box02, Texture2D ocean1,
            Texture2D hullGreen, Texture2D hullYellow, Texture2D hullOrange, Texture2D hullRed, Texture2D pauseScene,
            Texture2D STATE_MISSIONACCOMPLISH, Texture2D STATE_MISSIONFAIL,  Texture2D enemyJetFighterTopView, 
            ParticleSystem EXPOLSION_PARTICLES, ParticleSystem SMOKE_PARTICLES, SpriteFont font)
            : base(game) 
        {
            this.EXPOLSION_PARTICLES = EXPOLSION_PARTICLES;
            this.SMOKE_PARTICLES = SMOKE_PARTICLES;

            // Get the audio library
            audio = (AudioLibrary)Game.Services.GetService(typeof(AudioLibrary));
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            viewportRect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            //TIME ENTITIES...........................................
            timeElapsed = new Timer(game);
            timeElapsed.Value = 0;
            Components.Add(timeElapsed);

            //PLAYER VIEW ENTITY.....................................
            this.playerShip = playerShip;
            this.playerLeft = playerLeft;
            this.playerUp = playerUp;
            this.playerDown = playerDown;

            //WEAPON ENTITIES........................................
            this.plasma = plasma;
            this.cannon = cannon;

            //MISSION STATEMENT ENTITY...............................
            this.STATE_MISSIONACCOMPLISH = STATE_MISSIONACCOMPLISH;
            this.STATE_MISSIONFAIL = STATE_MISSIONFAIL;

            //SIDE BAR ENTITIES......................................
            this.sideBar = sideBar;
            this.hullGreen = hullGreen;
            this.hullYellow = hullYellow;
            this.hullOrange = hullOrange;
            this.hullRed = hullRed;

            //PAUSE SCENE ENTITY....................................
            this.pauseScene = pauseScene;

            //Initilzing the health point output text/font............
            healthpts = new UpgradePower(game, font, Color.Lavender);
            healthpts.Position = new Vector2(70, 45);
            healthpts.Value = 100;
            Components.Add(healthpts);

            //Background.......................................
            this.ocean1 = ocean1;
            oceanBackground = new MultiBackground(graphics);
            oceanBackground.AddLayer(ocean1, 0.0f, 200.0f);
            oceanBackground.SetMoveLeftRight();
            oceanBackground.StartMoving();

            //Player initlizing................................
            player = new Player01_2(game, ref playerShip, ref crosshair, ref graphics, ref cannon, ref plasma, PlayerIndex.One, new Rectangle(0, 0, 100, 53));
            player.Initialize();
            Components.Add(player);

            //ENEMY ENTITES.....................................
            EnemyJetObject = new Core.GameObject[MAXENEMYJETFIGHTER];
            for (int i = 0; i < MAXENEMYJETFIGHTER; i++)
                EnemyJetObject[i] = new Core.GameObject(enemyJetFighterTopView);

            enemyCannonBalls = new Core.GameObject[maxEnemyCannonBalls];
            for (int i = 0; i < maxEnemyCannonBalls; i++)
                enemyCannonBalls[i] = new Core.GameObject(plasma);

            this.box02 = box02;

            //FONTS.............................................
            this.font = font;
        }
        /********************************************************************/
        public override void Show()
        {
 
            paused = false;
            pausePosition.X = (Game.Window.ClientBounds.Width - pauseRect.Width) / 2;
            pausePosition.Y = (Game.Window.ClientBounds.Height - pauseRect.Height) / 2;

            startingTimeInSeconds = 60;//SETS THE TIME
            timeElapsed.Reset();

            //Resets the player setting
            player.Reset();
            player.Visible = true;

            player.SpeedPower = speedPower;
            MediaPlayer.Play(audio.BaliCrossroads);

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
        public bool MissionComplete
        {
            get { return missionComplete; }
            set { missionComplete = value; }
        }
        /********************************************************************/
        public override void Update(GameTime gameTime)
        {
                if (!paused)
                {
                    if (isMissionAccomplish == false)
                    {
                        UpdateEnemies();
                        UpdateEnemyDefesne();
                        FireEnemyCannonBalls();
                        UpdateSpiffyTimer(gameTime);
                        healthpts.Value = player.HealthPoints;//passing values to UpgradePower.cs = Player.cs
                        oceanBackground.Update(gameTime);
                        CreateEnemyTime();

                        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        totalTime += elapsed;

                        for (int i = 0; i < enemyShipList.Count; i++)
                        {
                            enemyShipList[i].Update(gameTime);
                        }
                    }
                }
            playerRectangle = new Rectangle((int)player.getPosition().X, (int)player.getPosition().Y,playerShip.Width, playerShip.Height);
            base.Update(gameTime);
        }
        /********************************************************************/
        public void UpdateEnemies()
        {
            foreach (Core.GameObject enemy in EnemyJetObject)
            {
                if (enemy.alive)
                {
                    enemy.velocity = new Vector2(player.getPosition().X - enemy.position.X, player.getPosition().Y - enemy.position.Y);
                    enemy.velocity.Normalize();

                    enemy.velocity = Vector2.Multiply(enemy.velocity, 3);
                    enemy.rotation = (float)Math.Atan2((player.getPosition().Y - enemy.position.Y), (player.getPosition().X - enemy.position.X));
                    enemy.position += enemy.velocity;

                    if (!viewportRect.Contains(new Point((int)enemy.position.X, (int)enemy.position.Y)))
                    {
                        enemy.alive = false;
                    }
                }
                else
                {
                    enemy.alive = true;
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
                Rectangle enemyRect = new Rectangle((int)enemy.position.X, (int)enemy.position.Y, 100, 130);

                if (cannonBallRect.Intersects(enemyRect))
                {
                    enemy.alive = false;
                    player.Score += 20;
                    EXPOLSION_PARTICLES.AddParticles(enemy.position);
                    SMOKE_PARTICLES.AddParticles(enemy.position);
                    audio.Explosion02.Play(0.4f, 0.0f, 0.0f);
                }
                if (enemyRect.Intersects(player.GetBounds()))
                {
                    enemy.alive = false;
                    player.HealthPoints -= 10;
                    audio.Explosion02.Play(0.5f,0.0f,0.0f);
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
                    }
                }
            }
        }
        /********************************************************************/
        public void CreateEnemy(EnemyType enemyType)
        {
            int startX = random.Next(800, 1400);
            int startY = random.Next(50, 550);
            if (enemyType == EnemyType.EXPLOSIVEBOX)
            {
                Enemy enemy = new Enemy(box02, new Vector2(startX, startY), 150.0f);
                enemyShipList.Add(enemy);
            }

            //int startX_enemy = random.Next(2000, 2400);
            //if (enemyType == EnemyType.blue)
            //{
            //    Enemy enemy = new Enemy(enemyTextureBlue, new Vector2(startX, startY), 50.0f);
            //    if (totalTime > 7.2f)
            //        enemy.FiringActive = true;
            //    enemyShipList.Add(enemy);
            //}
        }
        /********************************************************************/
        public void CreateEnemyTime()
        {
            if (totalTime > 2.2f && totalTime < 2.21f)
                for (int i = 0; i < 10; i++)
                    CreateEnemy(EnemyType.EXPLOSIVEBOX);

            //if (totalTime > 7.2f && totalTime < 7.21f)
            //    for (int i = 0; i < 7; i++)
            //        CreateEnemy(EnemyType.blue);
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
        //******************************************************//
        public void DrawSpiffyTimer()
        {
            Vector2 timerPosition = new Vector2(380, 40);
            Vector2 shadowPosition = new Vector2(380+1, 40+1);

            string spiffyLabel, timerLabel;

            if ((currentSeconds != 0) && (currentSeconds > 0))
            {

                if (currentSeconds >= 10)
                { spiffyLabel = currentMinutes + ":" + currentSeconds; }
                else
                { spiffyLabel = currentMinutes + ":" + properSeconds; }

                timerLabel = spiffyLabel;
                spriteBatch.DrawString(font, timerLabel, timerPosition, Color.White);
                if ((currentSeconds <= 20) && (currentSeconds >= 10))
                    spriteBatch.DrawString(font, timerLabel, timerPosition, Color.Orange);
                if (currentSeconds <= 10)
                    spriteBatch.DrawString(font, timerLabel, timerPosition, Color.Red);
            }

            if ((currentSeconds <= 0) && currentMinutes <= 0)
            {
                MediaPlayer.Stop();
                isMissionAccomplish = true;
                spriteBatch.Draw(STATE_MISSIONACCOMPLISH, new Vector2(0, 0), Color.White);
            }
        }
        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            oceanBackground.Draw();

            foreach (Core.GameObject enemy in EnemyJetObject)
            {
                if (enemy.alive)
                    spriteBatch.Draw(enemy.sprite, enemy.position, null, Color.White, enemy.rotation, new Vector2(enemy.sprite.Width / 2, enemy.sprite.Height / 2), 1f, SpriteEffects.None, 0);
            }

            foreach (Core.GameObject ball in enemyCannonBalls)
            {
                if (ball.alive == true)
                    spriteBatch.Draw(ball.sprite, ball.position, Color.Yellow);
            }

            spriteBatch.Draw(sideBar, new Vector2(0, 0), Color.White);
            base.Draw(gameTime);



            foreach (Enemy e in enemyShipList)
                e.Draw(spriteBatch);

            if (healthpts.Value >= 71)
                spriteBatch.Draw(hullGreen, new Vector2(120, 15), Color.White);
            else if (healthpts.Value <= 70 && healthpts.Value >= 51)
                spriteBatch.Draw(hullYellow, new Vector2(120, 15), Color.White);
            else if (healthpts.Value <= 50 && healthpts.Value >= 21)
                spriteBatch.Draw(hullOrange, new Vector2(120, 15), Color.White);
            else if (healthpts.Value <= 20 && healthpts.Value >= 1)
            {
                spriteBatch.Draw(hullRed, new Vector2(120, 15), Color.White);
            }
            else if (healthpts.Value <= 0)
            {
                spriteBatch.Draw(STATE_MISSIONFAIL, new Vector2(0, 0), Color.White);
                isMissionFail = true;
            }



            DrawSpiffyTimer();
            if (paused)// Draw the "pause" text
                spriteBatch.Draw(pauseScene, new Vector2(0, 0), Color.White);

        }
        /********************************************************************/
    }
}
