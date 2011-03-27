using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;
using TheWaver.Core;
using TheWaver.StoryLevel01;
using TheWaver.Particles;

namespace TheWaver
{
    public class Level01_03 : GameScene
    {
        protected SpriteBatch spriteBatch = null;


        //Playership
        protected Texture2D playerShip;
        protected Texture2D smokeTexture;

        //Enemy jetFighter
        //protected Texture2D enemyJetFighter;
        const float maxEnemyHeight = -5.1f;//Default 0.1f
        const float minEnemyHeight = 0.3f;
        const float maxEnemyVelocity = 8.0f;
        const float minEnemyVelocity = 3.0f;
        const int MAXENEMYJETFIGHTER = 2;
        Core.GameObject[] EnemyJetObject;
        Random random = new Random();
        const int maxEnemyCannonBalls = 3;
        Core.GameObject[] enemyCannonBalls;

        //Boss
        Core.GameObject bossShip;
        const int maxBossCannonBalls = 1;
        Core.GameObject[] bossCannonBalls;

        //Missile
        Core.GameObject[] MissileJetObject;
        const int maxMissileCannonBalls = 1;


        //Audio
        private AudioLibrary audio;
        protected Player02 player;

        //PAUSE SCENE
        protected Texture2D pauseScene;
        protected Vector2 pausePosition;
        protected Rectangle pauseRect;
        protected bool paused;

        //T || F -> Mission Accomplish or Fail
        public bool isMissionFail = false, isMissionAccomplish = false;

        //TEXTURES
        protected Texture2D SFBAYTEXTURE, oilRig, bottomStatusBar, bossShip01, missile, crosshair;

        //DISPLAY STRINGS
        protected UpgradePower healthpts, bossPts;

        //MISSION ACCOMPLISH SCENE
        protected Texture2D STATE_MISSIONACCOMPLISH, STATE_MISSIONFAIL;
        protected bool missionComplete;

        //Display Health, Scores
        protected UpgradePower oilPercent;
        protected Texture2D hullGreen, hullOrange, hullRed, hullYellow;

        //Boss Ship for level 01
        //BossShip01 level01_bossShip;
        protected Texture2D bossStatus_GREEN, bossStatus_YELLOW, bossStatus_ORANGE, bossStatus_RED;
        
        Rectangle viewportRect;
        GraphicsDeviceManager graphics;

        //Time Variable
        Timer timeElapsed;
        float startingTimeInSeconds;//30 seconds
        int currentMinutes;
        int currentSeconds;
        int currentTime;
        float currentFloatTime;
        string properSeconds;
        SpriteFont font, boss01Fonts;
        bool isWARN = false;
        bool fall;
        
        
        List<Vector2> smokeList = new List<Vector2>();
        Random randomizer = new Random();

        const float spaceshipGravity = 0.5f;

        ParticleSystem explosion;

        //CONSTRUCTOR CLASS
        public Level01_03(Game1 game, ref GraphicsDeviceManager graphics, ref Texture2D smokeTexture, Texture2D playerShip, Texture2D enemyJetFighter, 
            Texture2D bossShip01 , Texture2D missile, Texture2D missile1, Texture2D crosshair, Texture2D cannon, Texture2D plasma, Texture2D bottomStatusBar,
            Texture2D pauseScene,Texture2D STATE_MISSIONACCOMPLISH, Texture2D STATE_MISSIONFAIL,Texture2D SFBAYTEXTURE, 
            Texture2D prologueTop,Texture2D hullGreen, Texture2D hullYellow, Texture2D hullOrange, Texture2D hullRed,
            Texture2D bossStatus_GREEN, Texture2D bossStatus_YELLOW, Texture2D bossStatus_ORANGE,  Texture2D bossStatus_RED,
            SpriteFont font, SpriteFont boss01Fonts)
            : base(game)
        {
            this.smokeTexture = smokeTexture;
            this.playerShip = playerShip;
            this.pauseScene = pauseScene;
            this.STATE_MISSIONACCOMPLISH = STATE_MISSIONACCOMPLISH;
            this.STATE_MISSIONFAIL = STATE_MISSIONFAIL;
            this.SFBAYTEXTURE = SFBAYTEXTURE;
            this.oilRig = prologueTop;
            this.bottomStatusBar = bottomStatusBar;
            this.hullGreen = hullGreen;
            this.hullYellow = hullYellow;
            this.hullOrange = hullOrange;
            this.hullRed = hullRed;
            this.bossShip01 = bossShip01;
            this.bossStatus_GREEN = bossStatus_GREEN;
            this.bossStatus_YELLOW = bossStatus_YELLOW;
            this.bossStatus_ORANGE = bossStatus_ORANGE;
            this.bossStatus_RED = bossStatus_RED;
            this.font = font;
            this.boss01Fonts = boss01Fonts;
            this.missile = missile;
            this.crosshair = crosshair;
            
            //Loading enemy jet fighter
            EnemyJetObject = new Core.GameObject[MAXENEMYJETFIGHTER];
            for (int i = 0; i < MAXENEMYJETFIGHTER; i++)
            {
                EnemyJetObject[i] = new Core.GameObject(enemyJetFighter);
            }

            enemyCannonBalls = new Core.GameObject[maxEnemyCannonBalls];
            for (int i = 0; i < maxEnemyCannonBalls; i++)
                enemyCannonBalls[i] = new Core.GameObject(plasma);

            bossCannonBalls = new Core.GameObject[maxBossCannonBalls];
            for (int i = 0; i < maxBossCannonBalls; i++)
                bossCannonBalls[i] = new Core.GameObject(missile);

            bossShip = new Core.GameObject(bossShip01);

            MissileJetObject = new Core.GameObject[maxMissileCannonBalls];
            for (int i = 0; i < maxMissileCannonBalls; i++)
            {
                MissileJetObject[i] = new Core.GameObject(missile1);
            }

          //  explosion = new ExplosionParticleSystem(game, 1, "explosions");
         //   Components.Add(explosion);

            timeElapsed = new Timer(game);
            timeElapsed.Value = 0;
            Components.Add(timeElapsed);

            // Get the current sprite batch
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            viewportRect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            //level01_bossShip = new BossShip01(game, bossShip01, new Rectangle(0, 0, 600, 123), new Vector2(720, 420));
            //level01_bossShip.Initialize();
            //Components.Add(level01_bossShip);

            // Get the audio library
            audio = (AudioLibrary)Game.Services.GetService(typeof(AudioLibrary));

            //Initilizing boss ship

            healthpts = new UpgradePower(game, font, Color.Lavender);
            healthpts.Position = new Vector2(100, 600);
            healthpts.Value = 100;
            Components.Add(healthpts);

            bossPts = new UpgradePower(game, font, Color.Lavender);
            bossPts.Position = new Vector2(340, 600);
            bossPts.Value = 100;
            Components.Add(bossPts);

            //Initilizing the playership
            player = new Player02(game, ref playerShip, this.crosshair, ref graphics, ref cannon, ref plasma, PlayerIndex.One, new Rectangle(0, 0, 150, 65));
            player.Initialize();
            Components.Add(player);
        }
        //******************************************************//
        public override void Show()
        {
            //Resets the player setting
            player.Reset();
            //level01_bossShip.Reset();
            isMissionFail = false;
            isMissionAccomplish = false;
            isWARN = false;

            //Stop the music
            MediaPlayer.Stop();

            startingTimeInSeconds = 30;//SETS THE TIME
            timeElapsed.Reset();


            MediaPlayer.Play(audio.LastWarriorStanding);

            paused = false;
            pausePosition.X = (Game.Window.ClientBounds.Width - pauseRect.Width) / 2;
            pausePosition.Y = (Game.Window.ClientBounds.Height - pauseRect.Height) / 2;

            player.Visible = true;

            base.Show();
        }
        //******************************************************//
        public override void Hide() { base.Hide(); }
        //******************************************************//
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
        public bool MissionComplete
        {
            get { return missionComplete; }
            set { missionComplete = value; }
        }
        //******************************************************//
        public override void Update(GameTime gameTime)
        {
            //if (!paused)
            //    HandlePowerSourceSprite(gameTime);
            if (!paused)
            {
                if(isMissionAccomplish == false || isMissionFail == true)
                {
                    UpdateEnemies();
                    UpdateBoss();
                    UpdateEnemyDefesne();
                    UpdateBossDefesne();
                    FireEnemyCannonBalls();
                    FireBossCannonBalls();
                    Missile();
                    healthpts.Value = player.HealthPoints;
                    UpdateSpiffyTimer(gameTime);
                }


            }
            base.Update(gameTime);
        }

        public void UpdateBoss()
        {
            bossShip.position = new Vector2(1000, 350);
        }

        public void Missile()
        {
            foreach (Core.GameObject enemy in MissileJetObject)
            {
                if (enemy.alive)
                {
                   
                    enemy.velocity = new Vector2(player.getPosition().X - enemy.position.X, player.getPosition().Y - enemy.position.Y);
                    enemy.velocity.Normalize();

                    enemy.velocity = Vector2.Multiply(enemy.velocity, 3);
                    enemy.rotation = (float)Math.Atan2((player.getPosition().Y - enemy.position.Y), (player.getPosition().X - enemy.position.X));
                    enemy.position += enemy.velocity;



                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 smokePos = enemy.position;
                        
                       // smokePos.Y = enemy.position.Y + 3;

                        smokePos.X += randomizer.Next(10) - 5;
                        smokePos.Y += randomizer.Next(10) - 5;
                        smokeList.Add(smokePos);
                    }
                    


                    if (!viewportRect.Contains(new Point((int)enemy.position.X, (int)enemy.position.Y)))
                    {
                        enemy.alive = false;
                       
                    }
                    for (int i = 0; i < 3 && smokeList.Count > 0; i++)
                    {
                        smokeList.RemoveAt(randomizer.Next(smokeList.Count / 4));
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
                    audio.Explosion02.Play();
                }

                if (enemyRect.Intersects(player.GetBounds()))
                {
            //        explosion.AddParticles(enemy.position);
                    enemy.alive = false;
                    player.HealthPoints -= 10;
                    audio.Explosion.Play();
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
        //******************************************************//
        public void DrawSpiffyTimer()
        {
            Vector2 timerPosition = new Vector2(965, 599);
            Vector2 shadowPosition = new Vector2(966, 600);

            string spiffyLabel, timerLabel;

            if ((currentSeconds != 0) && (currentSeconds > 0))
            {

                if (currentSeconds >= 10)
                { spiffyLabel = currentMinutes + ":" + currentSeconds; }
                else
                { spiffyLabel = currentMinutes + ":" + properSeconds; }

                timerLabel = spiffyLabel;
                spriteBatch.DrawString(boss01Fonts, timerLabel, timerPosition, Color.White);
                if((currentSeconds <= 20) && (currentSeconds >=10))
                    spriteBatch.DrawString(boss01Fonts, timerLabel, timerPosition, Color.Orange);
                if (currentSeconds <= 10)
                    spriteBatch.DrawString(boss01Fonts, timerLabel, timerPosition, Color.Red);
            }

            if ((currentSeconds <= 0) && currentMinutes <= 0)
            {
                    MediaPlayer.Stop();
                    isMissionAccomplish = true;
            }
        }
        //******************************************************//
        public void UpdateEnemies()
        {
            foreach (Core.GameObject enemy in EnemyJetObject)
            {
                if (enemy.alive)
                {
                    

                    if (fall)
                    {
                        enemy.velocity.Y += 5f;
                        enemy.rotation = (float)Math.Max(enemy.rotation - .1, -MathHelper.PiOver2);
                        fall = false;
                    }
                    else
                    {

                        enemy.position += enemy.velocity;
                        
                        if (!viewportRect.Contains(new Point(
                            (int)enemy.position.X,
                            (int)enemy.position.Y)))
                        {
                            enemy.alive = false;
                        }

                    }
                }
                else
                {
                    enemy.alive = true;
                    fall = false;
                    enemy.position = new Vector2(
                        viewportRect.Right,
                        MathHelper.Lerp(
                        (float)viewportRect.Height * minEnemyHeight,
                        (float)viewportRect.Height * maxEnemyHeight,
                        (float)random.NextDouble()));
                    enemy.velocity = new Vector2(
                        MathHelper.Lerp(
                        -minEnemyVelocity,
                        -maxEnemyVelocity,
                        (float)random.NextDouble()), 0);
                }

                Rectangle cannonBallRect = player.cannonBallRect;
                Rectangle enemyRects = new Rectangle((int)enemy.position.X, (int)enemy.position.Y, 100, 130);
                if (cannonBallRect.Intersects(enemyRects))
                {
                    if (!fall)
                    {
                        enemy.alive = false;
                        fall = true;

                        //player.isBallAlive = false;
                        //player.IsPlasmaAlive(false);
                        audio.Explosion02.Play();
                        //player.PassingEnemyEntity(enemyRect, enemy.alive=false);
                    }
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
                            ball.velocity.Y = 11.0f;
                            break;
                        }
                    }
                }
            }
        }

        public void FireBossCannonBalls()
        {
            
                int fireCannonBall = random.Next(0, 50);
                if (fireCannonBall == 1)
                {
                    foreach (Core.GameObject ball in bossCannonBalls)
                    {
                        if (ball.alive == false)
                        {
                            ball.alive = true;
                            ball.position = new Vector2(bossShip.position.X - 100, bossShip.position.Y - 70);
                            ball.velocity.Y = 3.0f;
                            break;
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
        public void UpdateBossDefesne()
        {
           
                foreach (Core.GameObject ball in bossCannonBalls)
                {
                    if (ball.alive)
                    {
                        ball.position -= ball.velocity;

                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 smokePos = ball.position;

                            // smokePos.Y = enemy.position.Y + 3;

                            smokePos.X += randomizer.Next(10) - 5;
                            smokePos.Y += randomizer.Next(10) - 5;
                            smokeList.Add(smokePos);
                        }



                        if (!viewportRect.Contains(new Point((int)ball.position.X, (int)ball.position.Y)))
                        {
                            ball.alive = false;

                            continue;
                        }

                        for (int i = 0; i < 3 && smokeList.Count > 0; i++)
                        {
                            smokeList.RemoveAt(randomizer.Next(smokeList.Count / 4));
                        }
                        
                        

                        //   Rectangle cannonBallRect = new Rectangle((int)ball.position.X, (int)ball.position.Y, 100, (53));

                        /*  if (cannonBallRect.Intersects(player.GetBounds()))
                          {
                              ball.alive = false;
                              player.HealthPoints -= 10;
                              audio.Explosion.Play();
                              //isPhealth = true;
                          } */

                    }
                }
            
        }
        //******************************************************//
        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Draw(SFBAYTEXTURE, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(oilRig, new Vector2(510,-50), Color.White);

            //foreach (Vector2 smokePos in smokeList)
            //    spriteBatch.Draw(smokeTexture, smokePos, null, Color.White, 0, new Vector2(40, 35), 0.2f, SpriteEffects.None, 1);

            spriteBatch.Draw(bossShip.sprite, bossShip.position, null, Color.White, bossShip.rotation, new Vector2(bossShip.sprite.Width / 2, bossShip.sprite.Height / 2), 0.3f, SpriteEffects.None, 0);

            foreach (Core.GameObject enemy in MissileJetObject)
            {
                if (enemy.alive)
                    spriteBatch.Draw(enemy.sprite, enemy.position, null, Color.White, enemy.rotation, new Vector2(enemy.sprite.Width / 2, enemy.sprite.Height / 2), 0.1f, SpriteEffects.None, 0);
            }

            foreach (Core.GameObject BossMissile in bossCannonBalls)
            {
                if (BossMissile.alive == true)
                spriteBatch.Draw(BossMissile.sprite, BossMissile.position, null, Color.White, BossMissile.rotation, new Vector2(BossMissile.sprite.Width / 2, BossMissile.sprite.Height / 2), 0.1f, SpriteEffects.None, 0);
            }

            foreach (Core.GameObject enemy in EnemyJetObject)
            {
                if (enemy.alive)
                {
                    spriteBatch.Draw(enemy.sprite,
                        enemy.position, null, Color.White, enemy.rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }
            }

            foreach (Core.GameObject ball in enemyCannonBalls)
            {
                if (ball.alive == true)
                    spriteBatch.Draw(ball.sprite, ball.position, Color.Yellow);
            }

            

            spriteBatch.Draw(bottomStatusBar, new Vector2(0, 570), Color.White);


            spriteBatch.DrawString(boss01Fonts, "Boss coming in: ", new Vector2(790, 600), Color.White);
            spriteBatch.DrawString(font, "-Protect the last oil rig in\n that region!", new Vector2(510, 600), Color.White);
            spriteBatch.DrawString(font, "-Disable the bos ship when  \n it arrives.", new Vector2(510, 640), Color.White);

            //BOSS'S HEALTH POINTS
            if (bossPts.Value >= 71)
                spriteBatch.Draw(bossStatus_GREEN, new Vector2(260, 620), Color.White);
            else if (bossPts.Value <= 70 && bossPts.Value >= 51)
                spriteBatch.Draw(bossStatus_YELLOW, new Vector2(260, 620), Color.White);
            else if (bossPts.Value <= 50 && bossPts.Value >= 21)
                spriteBatch.Draw(bossStatus_ORANGE, new Vector2(260, 620), Color.White);
            else if (bossPts.Value <= 20)
                spriteBatch.Draw(bossStatus_RED, new Vector2(260, 620), Color.White);

            //PLAYER'S HEALTH POINTS
            if (healthpts.Value >= 71)
                spriteBatch.Draw(hullGreen, new Vector2(60, 620), Color.White);
            else if (healthpts.Value <= 70 && healthpts.Value >= 51)
                spriteBatch.Draw(hullYellow, new Vector2(60, 620), Color.White);
            else if (healthpts.Value <= 50 && healthpts.Value >= 21)
                spriteBatch.Draw(hullOrange, new Vector2(60, 620), Color.White);
            else if (healthpts.Value <= 20)
                spriteBatch.Draw(hullRed, new Vector2(60, 620), Color.White);


            base.Draw(gameTime);

            if (paused)// Draw the "pause" text
                spriteBatch.Draw(pauseScene, new Vector2(0, 0), Color.White);

            if (healthpts.Value <= 0)
            {
                spriteBatch.Draw(STATE_MISSIONFAIL, new Vector2(0, 0), Color.White);
                isMissionFail = true;
            }

            if (bossPts.Value <= 0)
            {
                spriteBatch.Draw(STATE_MISSIONACCOMPLISH, new Vector2(0, 0), Color.White);
                isMissionAccomplish = true;
            }

            DrawSpiffyTimer();

 
        }
        //******************************************************//
    }
}
