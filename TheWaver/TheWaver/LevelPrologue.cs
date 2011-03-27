using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TheWaver.Core;
using TheWaver.StoryLevel01;

namespace TheWaver
{
    public class LevelPrologue : GameScene
    {
        protected SpriteBatch spriteBatch = null;

        //Playership
        protected Texture2D playerShip, plasma, cannon, crosshair;
        protected Texture2D playerLeft;
        protected Texture2D playerUp;
        protected Texture2D playerDown;

        //Audio
        private AudioLibrary audio;
        protected Player player;

        //Oil Spill
        protected int MAXDOT = 15;
        protected OilSpill[] oilSpill01, oilSpill02, oilSpill03, oilSpill04, oilSpill05, oilSpill06,
    oilSpill07, oilSpill08, oilSpill09, oilSpill10;

        //PAUSE SCENE
        protected Texture2D pauseScene;
        protected Vector2 pausePosition;
        protected Rectangle pauseRect;
        protected bool paused;

        //TEXTURES
        protected Texture2D SFBAYTEXTURE, prologueTop;

        //MISSION ACCOMPLISH SCENE
        protected Texture2D STATE_MISSIONACCOMPLISH;
        protected bool missionComplete;

        //Display Health, Scores
        //protected UpgradePower oilPercent;
        protected int oilCount;
        protected int oilCount2;
        protected int oilCount3;
        protected int oilCount4;
        protected int oilCount5;
        protected int oilCount6;
        protected int oilCount7;
        protected int oilCount8;
        protected int oilCount9;
        protected int oilCount10;

        GraphicsDeviceManager graphics;

        SpriteFont font;
        private int speedPower;

        //CONSTRUCTOR CLASS
        public LevelPrologue(Game game, Texture2D playerShip,  Texture2D playerLeft, Texture2D playerDown, Texture2D playerUp, Texture2D crosshair, GraphicsDeviceManager graphics, Texture2D cannon, Texture2D plasma, Texture2D oilSpillTexture, Texture2D pauseScene,
            Texture2D STATE_MISSIONACCOMPLISH, Texture2D SFBAYTEXTURE, Texture2D prologueTop, SpriteFont font)
            : base(game) 
        {
            this.playerShip = playerShip;
            this.pauseScene = pauseScene;
            this.STATE_MISSIONACCOMPLISH = STATE_MISSIONACCOMPLISH;
            this.SFBAYTEXTURE = SFBAYTEXTURE;
            this.prologueTop = prologueTop;
            this.plasma = plasma;
            this.cannon = cannon;
            this.graphics = graphics;
            this.crosshair = crosshair;
            this.playerDown = playerDown;
            this.playerLeft = playerLeft;
            this.playerUp = playerUp;
            this.font = font;

            // Get the current sprite batch
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            // Get the audio library
            audio = (AudioLibrary)Game.Services.GetService(typeof(AudioLibrary));

            //oilSpill_LEVEL01(game, ref oilSpillTexture);//OIL TEXTURE FUNCTION
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
            for (int i = 0; i < MAXDOT; i++)
            {
                oilSpill01[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(597, (86 + (20 * i))));
                oilSpill01[i].Initialize();
                Components.Add(oilSpill01[i]);

                oilSpill02[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(612, (93 + (15 * i))));
                oilSpill02[i].Initialize();
                Components.Add(oilSpill02[i]);

                oilSpill03[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(625, (115 + (10 * i))));
                oilSpill03[i].Initialize();
                Components.Add(oilSpill03[i]);

                oilSpill04[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(639, (134 + (5 * i))));
                oilSpill04[i].Initialize();
                Components.Add(oilSpill04[i]);

                oilSpill05[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(660, (123 + (3 * i))));
                oilSpill05[i].Initialize();
                Components.Add(oilSpill05[i]);

                oilSpill06[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(681, (130 + (2 * i))));
                oilSpill06[i].Initialize();
                Components.Add(oilSpill06[i]);

                oilSpill07[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(582, (94 + (20 * i))));
                oilSpill07[i].Initialize();
                Components.Add(oilSpill07[i]);

                oilSpill08[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(558, (114 + (10 * i))));
                oilSpill08[i].Initialize();
                Components.Add(oilSpill08[i]);

                oilSpill09[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(538, (115 + (10 * i))));
                oilSpill09[i].Initialize();
                Components.Add(oilSpill09[i]);

                oilSpill10[i] = new OilSpill(game, oilSpillTexture, new Rectangle(0, 0, 37, 38), new Vector2(555, (115 + (12 * i))));
                oilSpill10[i].Initialize();
                Components.Add(oilSpill10[i]);
            }

            //Initilizing the playership
            player = new Player(game, ref playerShip, ref playerLeft, ref playerDown, ref playerUp, ref crosshair, ref graphics, ref cannon, ref plasma, PlayerIndex.One, new Rectangle(0, 0, 100, 53));
            //Player's parameter (game, image,only player one, (size of the image, coordinate of the image))
            player.Initialize();
            Components.Add(player);

            oilCount = 0; 
            oilCount2 = 0;
            oilCount3 = 0;
            oilCount4 = 0;
            oilCount5 = 0; 
            oilCount6 = 0;
            oilCount7 = 0;
            oilCount8 = 0;
            oilCount9 = 0;
            oilCount10 = 0;
        }
        //******************************************************//
        public override void Show()
        {
            //Resets the player setting
            player.Reset();

            oilCount = 0;
            oilCount2 = 0;
            oilCount3 = 0;
            oilCount4 = 0;
            oilCount5 = 0;
            oilCount6 = 0;
            oilCount7 = 0;
            oilCount8 = 0;
            oilCount9 = 0;
            oilCount10 = 0;

            //Stop the music
            MediaPlayer.Stop();

            MediaPlayer.Play(audio.StoryLineMusic);

            //Resets the Oil Percent value to 100..
            //oilPercent.Value = 100; 

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
            }

            paused = false;
            pausePosition.X = (Game.Window.ClientBounds.Width - pauseRect.Width) / 2;
            pausePosition.Y = (Game.Window.ClientBounds.Height - pauseRect.Height) / 2;

            player.Visible = true;


            player.SpeedPower = speedPower;

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
        public int SpeedPower
        {
            get { return speedPower; }
            set
            {speedPower = value;}
        }
        //******************************************************//
        private void HandlePowerSourceSprite(GameTime gameTime)
        {
            for (int i = 0; i < MAXDOT; i++)
            {
                
                if (oilSpill01[i].CheckCollision(player.GetBounds()))
                {
                    oilSpill01[i].IsDetect = false;
                    oilCount = oilSpill01[5].OilDotRemove;
                    
                }

                if (oilSpill02[i].CheckCollision(player.GetBounds()))
                {
                    oilSpill02[i].IsDetect = false;
                    oilCount2 = oilSpill02[5].OilDotRemove;
                }

                if (oilSpill03[i].CheckCollision(player.GetBounds()))
                {
                    oilSpill03[i].IsDetect = false;
                    oilCount3 = oilSpill03[5].OilDotRemove;
                    
                }

                if (oilSpill04[i].CheckCollision(player.GetBounds()))
                {
                    oilSpill04[i].IsDetect = false;
                    oilCount4 = oilSpill04[10].OilDotRemove;
                }

                if (oilSpill05[i].CheckCollision(player.GetBounds()))
                {
                    oilSpill05[i].IsDetect = false;
                    oilCount5 = oilSpill05[5].OilDotRemove;
                }

                if (oilSpill06[i].CheckCollision(player.GetBounds()))
                {
                    oilSpill06[i].IsDetect = false;
                    oilCount6 = oilSpill06[5].OilDotRemove;
                }

                if (oilSpill07[i].CheckCollision(player.GetBounds()))
                {
                    oilSpill07[i].IsDetect = false;
                    oilCount7 = oilSpill07[7].OilDotRemove;
                }

                if (oilSpill08[i].CheckCollision(player.GetBounds()))
                {
                    oilSpill08[i].IsDetect = false;
                    oilCount8 = oilSpill08[5].OilDotRemove;
                }

                if (oilSpill09[i].CheckCollision(player.GetBounds()))
                {
                    oilSpill09[i].IsDetect = false;
                    oilCount9 = oilSpill09[7].OilDotRemove;
                }

                if (oilSpill10[i].CheckCollision(player.GetBounds()))
                {
                    oilSpill10[i].IsDetect = false;
                    oilCount10 = oilSpill10[6].OilDotRemove;
                }
            }
        }
        //******************************************************//
        public bool MissionComplete
        {
            get { return missionComplete;  }
            set { missionComplete = value; }
        }
        //******************************************************//
        public override void Update(GameTime gameTime) 
        {
            if(!paused)
                HandlePowerSourceSprite(gameTime);

            base.Update(gameTime);
        }
        //******************************************************//
        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Draw(SFBAYTEXTURE, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(prologueTop, new Vector2(0,0), Color.White);
            //620, 20
            int finalCount;
            //finalCount = oilCount + oilCount2 + oilCount3 + oilCount4 + oilCount5 + oilCount6 + oilCount7 + oilCount8 + oilCount9 + oilCount10;
            finalCount = oilSpill01[5].OilDotRemove + oilSpill02[5].OilDotRemove + oilSpill03[5].OilDotRemove + oilSpill04[10].OilDotRemove + oilSpill05[5].OilDotRemove + oilSpill06[5].OilDotRemove + oilSpill07[7].OilDotRemove + oilSpill08[9].OilDotRemove + oilSpill09[7].OilDotRemove + oilSpill10[6].OilDotRemove;

            spriteBatch.DrawString(font,"Oil cleaned up: "+finalCount.ToString() +"0%",new Vector2(620,20),Color.White);

            if (finalCount == 10)//CHANGE IT TO 0!
            {
                spriteBatch.Draw(STATE_MISSIONACCOMPLISH, new Vector2(0, 0), Color.White);
                missionComplete = true;
            }
            else
                missionComplete = false;

            base.Draw(gameTime);

            if (paused)// Draw the "pause" text
                spriteBatch.Draw(pauseScene, new Vector2(0, 0), Color.White);
        }
        //******************************************************//
    }
}
