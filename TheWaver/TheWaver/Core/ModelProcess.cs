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

namespace TheWaver.Core
{
    public class ModelProcess : DrawableGameComponent
    {

        protected SpriteBatch spriteBatch = null;

        protected Model modelDisplay;
        float aspectRatio;
        //Vector3 modelPosition = Vector3.Zero;
        protected Vector3 position = new Vector3();
        float modelRotation = 0.0f;
        Vector3 cameraPosition;
        /********************************************************************/
        public ModelProcess(Game game,  Model modelDisplay)
            : base(game)
        {
            this.modelDisplay = modelDisplay;
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }
        /********************************************************************/
        //public override void Update(GameTime gameTime)
        //{
        //    modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds *MathHelper.ToRadians(0.1f);
        //    base.Update(gameTime);
        //}
        /********************************************************************/
        public Model ModelDisplay{get { return modelDisplay; }}
        /********************************************************************/
        public float ModelRotation
        {
            get { return modelRotation; }
            set { modelRotation = value; }
        }
        public float AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; }
        }
        /********************************************************************/
        public Vector3 CameraPosition
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }
        /********************************************************************/
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        /********************************************************************/
        public override void Draw(GameTime gameTime)
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[modelDisplay.Bones.Count];
            modelDisplay.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in modelDisplay.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(modelRotation)
                        * Matrix.CreateTranslation(position);
                    effect.View = Matrix.CreateLookAt(cameraPosition,
                        Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), aspectRatio,
                        1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
       
    }
}
