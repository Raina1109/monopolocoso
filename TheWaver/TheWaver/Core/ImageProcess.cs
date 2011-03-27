using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheWaver.Core
{
    public class ImageProcess : DrawableGameComponent
    {
        //1 means to center screen | 2 means to stretch the image so it can fill up the blank spaces
        public enum DrawMode{Center = 1,Stretch,};
        // Texture to draw
        protected readonly Texture2D texture;
        // Draw Mode
        protected readonly DrawMode drawMode;
        // Spritebatch
        protected SpriteBatch spriteBatch = null;
        // Image Rectangle
        protected Rectangle imageRect;

        /********************************************************************/
        public ImageProcess(Game game, Texture2D texture, DrawMode drawMode)
            : base(game)
        {
            this.texture = texture;
            this.drawMode = drawMode;
            // Get the current spritebatch
            spriteBatch = (SpriteBatch) 
                Game.Services.GetService(typeof (SpriteBatch));

            // Create a rectangle with the size and position of the image
            switch (drawMode)
            {
                case DrawMode.Center:
                    imageRect = new Rectangle((Game.Window.ClientBounds.Width - 
                        texture.Width)/2,(Game.Window.ClientBounds.Height - 
                        texture.Height)/2,texture.Width, texture.Height);
                    break;
                case DrawMode.Stretch:
                    imageRect = new Rectangle(0, 0, Game.Window.ClientBounds.Width, 
                        Game.Window.ClientBounds.Height);
                    break;
            }
        }
        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(texture, imageRect, Color.White);
            base.Draw(gameTime);
        }
        /********************************************************************/
    }
}
