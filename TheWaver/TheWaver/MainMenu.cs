using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TheWaver.Core;

namespace TheWaver
{
    public class MainMenu : GameScene
    {
        protected readonly Texture2D elements;
        protected SpriteBatch spriteBatch = null;
        protected TextMenuComponent menu;
        protected AudioLibrary audio;

        /********************************************************************/
        public MainMenu(Game game, Texture2D background, SpriteFont smallFont, SpriteFont largeFont)
            : base(game)
        {
            //Display MainMenu background image
            Components.Add(new ImageProcess(game, background, ImageProcess.DrawMode.Center));

            //Diplay Text componenents and Effects
            string[] items = { "PLAY", "CONTROLLERS", "ABOUT", "TERMINATE" };
            menu = new TextMenuComponent(game, smallFont, largeFont);
            menu.SetMenuItems(items);
            Components.Add(menu);

            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            audio = (AudioLibrary)Game.Services.GetService(typeof(AudioLibrary));
        }
        /********************************************************************/
        public override void Show()
        {
            // Put the menu texts centered in screen
            menu.Position = new Vector2(100, 330);
            base.Show();
        }
        /********************************************************************/
        public override void Hide()
        {
            base.Hide();
        }
        /********************************************************************/
        /// <summary>
        /// Gets the selected menu option
        /// </summary>
        public int SelectedMenuIndex
        {
            get { return menu.SelectedIndex; }
        }
        /********************************************************************/
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        /********************************************************************/
    }
}
