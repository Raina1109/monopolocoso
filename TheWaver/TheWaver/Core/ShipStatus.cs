using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace TheWaver.Core
{
    public class ShipStatus
    {
        int SPEEDPOWER, WEAPONPOWER, HULLPOWER;


        public int SpeedPower
        {
            get { return SPEEDPOWER; }
            set { this.SPEEDPOWER = value; }
        }
    }
}
