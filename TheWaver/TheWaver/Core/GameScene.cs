/* The purpose of this class is to change scenes (or splash-screens) 
 * when the player selects an option
 */

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TheWaver.StoryLevel01;

namespace TheWaver.Core
{
    public abstract class GameScene : DrawableGameComponent
    {
        //VARIABLES////////////////////////
        private readonly List<GameComponent> components;

        /********************************************************************/
        public GameScene(Game game)//CONSTRUCTOR
            : base(game)
        {
            components = new List<GameComponent>();
            Visible = false;
            Enabled = false;
        }
        /********************************************************************/
        /// <summary>
        /// Show the scene
        /// </summary>
        public virtual void Show()
        {
            Visible = true;
            Enabled = true;
        }
        /********************************************************************/
        /// <summary>
        /// Hide the scene
        /// </summary>
        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;
        }
        /********************************************************************/
        /// <summary>
        /// Components of Game Scene
        /// </summary>
        public List<GameComponent> Components{get { return components; }}
        /********************************************************************/
        public override void Update(GameTime gameTime)
        {
            // Update the child GameComponents
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].Enabled)
                    components[i].Update(gameTime);
            }
            base.Update(gameTime);
        }
        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            // Draw the child GameComponents (if drawable)
            for (int i = 0; i < components.Count; i++)
            {
                GameComponent gc = components[i];
                if ((gc is DrawableGameComponent) &&((DrawableGameComponent)gc).Visible)
                    ((DrawableGameComponent)gc).Draw(gameTime);
            }
            base.Draw(gameTime);
        }
        /********************************************************************/
    }
}
