#region File Description
//-----------------------------------------------------------------------------
// Particle.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TheWaver
{
    namespace Particles
    {
        /// <summary>
        /// particles are the little bits that will make up an effect. each effect will
        /// be comprised of many of these particles. They have basic physical properties,
        /// such as position, velocity, acceleration, and rotation. They'll be drawn as
        /// sprites, all layered on top of one another, and will be very pretty.
        /// </summary>
        public class Particle
        {
            // Position, Velocity, and Acceleration represent exactly what their names
            // indicate. They are public fields rather than properties so that users
            // can directly access their .X and .Y properties.
            public Vector2 position;
            public Vector2 velocity;
            public Vector2 acceleration;

            // how long this particle will "live"
            public float lifetime;

            // how long it has been since initialize was called
            public float timeSinceStart;

            // the scale of this particle
            public float scale;

            // its rotation, in radians
            public float rotation;

            // how fast does it rotate?
            public float rotationSpeed;

            // is this particle still alive? once TimeSinceStart becomes greater than
            // Lifetime, the particle should no longer be drawn or updated.
            public bool Active
            {
                get { return timeSinceStart < lifetime; }
            }


            // initialize is called by ParticleSystem to set up the particle, and prepares
            // the particle for use.
            public void Initialize(Vector2 position, Vector2 velocity, Vector2 acceleration,
                float lifetime, float scale, float rotationSpeed)
            {
                // set the values to the requested values
                this.position = position;
                this.velocity = velocity;
                this.acceleration = acceleration;
                this.lifetime = lifetime;
                this.scale = scale;
                this.rotationSpeed = rotationSpeed;

                // reset TimeSinceStart - we have to do this because particles will be
                // reused.
                this.timeSinceStart = 0.0f;

                // set rotation to some random value between 0 and 360 degrees.
                this.rotation = ParticleSystem.RandomBetween(0, MathHelper.TwoPi);
            }

            // update is called by the ParticleSystem on every frame. This is where the
            // particle's position and that kind of thing get updated.
            public void Update(float dt)
            {
                velocity += acceleration * dt;
                position += velocity * dt;

                rotation += rotationSpeed * dt;

                timeSinceStart += dt;
            }
        }
    }
}
