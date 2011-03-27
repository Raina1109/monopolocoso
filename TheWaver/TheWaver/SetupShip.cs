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

namespace TheWaver
{
    public class SetupShip : GameScene
    {
        public int testB;
        protected SpriteBatch spriteBatch = null;
        private ShipStatus shipStatus; 
        protected UpgradePower totalPower, weaponPower, hullStength, speedPower;
        protected ModelProcess modelProcess;
        protected Texture2D error01;
        public bool isError01;
        /********************************************************************/
        public SetupShip(Game game, Texture2D background, Texture2D error01, Model thisModel, SpriteFont font, float aspectRatio)
            : base(game)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            shipStatus = (ShipStatus)Game.Services.GetService(typeof(ShipStatus));
            //Displays the background
            Components.Add(new ImageProcess(game, background, ImageProcess.DrawMode.Stretch));

            modelProcess = new ModelProcess(game, thisModel);
            modelProcess.Position = Vector3.Zero;
            modelProcess.CameraPosition = new Vector3(0.0f, 0.0f, 5000.0f);
            modelProcess.AspectRatio = aspectRatio;
            modelProcess.Initialize();
            Components.Add(modelProcess);

            totalPower = new UpgradePower(game, font, Color.White);
            totalPower.Position = new Vector2(320, 250);
            totalPower.Value = 20;
            shipStatus.SpeedPower = totalPower.Value;
            Components.Add(totalPower);

            weaponPower = new UpgradePower(game, font, Color.White);
            weaponPower.Position = new Vector2(395, 350);
            Components.Add(weaponPower);

            hullStength = new UpgradePower(game, font, Color.White);
            hullStength.Position = new Vector2(395, 450);
            Components.Add(hullStength);

            speedPower = new UpgradePower(game, font, Color.White);
            speedPower.Position = new Vector2(395, 550);
            Components.Add(speedPower);

            this.error01 = error01;
            isError01 = false;
        }
        /********************************************************************/
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (totalPower.Value > 0)
            {
                //Increase the value of the ship
                if (keyboard.IsKeyDown(Keys.E))
                {
                    weaponPower.Value += 1;//Gives more value to the weapon
                    totalPower.Value -= 1;//Decrease the given total power
                    shipStatus.SpeedPower -= 1;
                    
                }
                if (keyboard.IsKeyDown(Keys.D))
                {
                    hullStength.Value += 1;//Gives more value to the hull strength
                    totalPower.Value -= 1;//Decrease the given total power
                    shipStatus.SpeedPower -= 1;
                }
                if (keyboard.IsKeyDown(Keys.C))
                {
                    speedPower.Value += 1;//Gives more value to the hull strength
                    totalPower.Value -= 1;//Decrease the given total power
                    shipStatus.SpeedPower -= 1;
                }
                /////////////////////////////////////////////////////////
                //Decrease the value of the ship
                if ((weaponPower.Value > 0))
                {
                    if (keyboard.IsKeyDown(Keys.Q))
                    {
                        weaponPower.Value -= 1;//Gives more value to the hull strength
                        totalPower.Value += 1;//Decrease the given total power
                        shipStatus.SpeedPower += 1;
                    }
                }
                if(hullStength.Value >0)
                {
                    if (keyboard.IsKeyDown(Keys.S))
                    {
                        hullStength.Value -= 1;//Gives more value to the hull strength
                        totalPower.Value += 1;//Decrease the given total power
                        shipStatus.SpeedPower += 1;
                    }
                }
                if(speedPower.Value >0)
                {
                    if (keyboard.IsKeyDown(Keys.X))
                    {
                        speedPower.Value -= 1;//Gives more value to the hull strength
                        totalPower.Value += 1;//Decrease the given total power
                        shipStatus.SpeedPower += 1;
                    }
                }
                /////////////////////////////////////////////////////////
                //Rotate the player's 3D model ship
                modelProcess.ModelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds *MathHelper.ToRadians(0.1f);
            }
        }
        /********************************************************************/
        //public int SpeedPower
        //{
        //    get { return speedPower.Value; }
        //    set
        //    {
        //        if (value < 0)
        //            speedPower.Value = 0;
        //        else
        //            speedPower.Value = value;
        //    }
        //}
        //Resets the crew member when reseting the game
        public void Reset()
        {
            totalPower.Value = 20;
            weaponPower.Value = 0;
            hullStength.Value = 0;
            speedPower.Value = 0;
            shipStatus.SpeedPower = 20;
            isError01 = false;
        }
        public int TotalPowerValue{get { return totalPower.Value; }}
        public int SpeedPower{ get { return speedPower.Value; } }
        public int HullPower { get { return hullStength.Value; } }

        public override void Show()
        {
            modelProcess.Visible = true;
            isError01 = false;
            base.Show();
        }
    }
}
