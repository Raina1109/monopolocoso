using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using TheWaver.Core;
using TheWaver.StoryLevel01;
using TheWaver.StoryLevel02;

namespace TheWaver
{
    public class Credits_Before : GameScene
    {
        //AUDIO....................................................................
        private AudioLibrary audio;

        protected TimeSpan elapsedTime = TimeSpan.Zero;
        public int loadingBar;
        protected bool isAppear;
        protected Texture2D blackBackground;

        protected SpriteBatch spriteBatch = null;

        SpriteFont font;
        /********************************************************************/
        public Credits_Before(Game game, Texture2D backgroundImage, Texture2D blackBackground, SpriteFont font)
            : base(game)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            //The intro image background will be process to the ImageProcess class and display the image
            Components.Add(new ImageProcess(game, backgroundImage, ImageProcess.DrawMode.Stretch));
            this.blackBackground = blackBackground;
            this.font = font;
            loadingBar = 0;
            isAppear = false;

            //GET THE AUDIO LIBRARY.........................................................
            audio = (AudioLibrary)Game.Services.GetService(typeof(AudioLibrary));
            MediaPlayer.IsRepeating = true;
            MediaPlayer.IsShuffled = false;
            MediaPlayer.Volume = .25f;

        }
        /********************************************************************/
        public override void Hide()
        {
            base.Hide();
        }
        /********************************************************************/
        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                loadingBar++;
            }
            if (loadingBar > 6)
            {
                isAppear = true;
            }
            base.Update(gameTime);
        }
        /********************************************************************/
        public override void Show()
        {
            loadingBar = 0;
            isAppear = false;
            MediaPlayer.Play(audio.CreditsMusic);
            base.Show();//Display the scene
        }
        /********************************************************************/
        public int LoadingBar
        {
            get { return loadingBar; }
        }
        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (isAppear == false)
                spriteBatch.Draw(blackBackground, new Vector2(0, 0), Color.White);
            //if(loadingBar > 8)
            //    spriteBatch.DrawString(font, "PRESS SPACEBAR TO CONTINUE", new Vector2(500, 555), Color.LightGray);
        }
    }
}