using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheWaver.Core
{
    public class UpgradePower : DrawableGameComponent
    {
        // Spritebatch
        protected SpriteBatch spriteBatch = null;

        // Score Position
        protected Vector2 position = new Vector2();

        // Values
        protected int numberPower;

        protected readonly SpriteFont font;
        protected readonly Color fontColor;
        /********************************************************************/
        public UpgradePower(Game game, SpriteFont font, Color fontColor)
            : base(game)
        {
            //class variables = function parameter;
            this.font = font;
            this.fontColor = fontColor;
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }
        /********************************************************************/
        /// <summary>
        /// Power value
        /// </summary>
        public int Value
        {
            get { return numberPower; }
            set { this.numberPower = value; }
        }
        /********************************************************************/
        /// <summary>
        /// Position of component in screen
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            string TextToDraw = string.Format("{0}", numberPower);
            // Draw the text shadow
            spriteBatch.DrawString(font, TextToDraw, new Vector2(position.X + 1, position.Y + 1), Color.Black);
            // Draw the text item
            spriteBatch.DrawString(font, TextToDraw, new Vector2(position.X, position.Y), fontColor);

            base.Draw(gameTime);
        }
    }
}
