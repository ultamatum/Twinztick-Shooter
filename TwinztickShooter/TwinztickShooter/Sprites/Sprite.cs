﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinztickShooter.Sprites
{
    class Sprite
    {
        #region Declarations
        public Texture2D image;

        public Vector2 worldLocation;
        public Vector2 direction;

        public Color tint = Color.White;
        
        public float rotation;
        protected float drawDepth = 11.85f;

        public int health;

        public Rectangle hitBox;
        public Rectangle previousHitBox;

        protected bool enabled;
        protected float drawDepth = 11.85f;
        #endregion

        #region Properties
        /// <summary>
        /// Sets / Returns whether or not the sprite is enabled
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Sets / Returns the location of the sprite in the world
        /// </summary>
        public Vector2 WorldLocation
        {
            get { return worldLocation; }
            set { worldLocation = value; }
        }

        /// <summary>
        /// Returns the coordinates of the center of the world
        /// </summary>
        public Vector2 WorldCenter
        {
            get
            {
                return new Vector2((int)worldLocation.X + (int)(image.Width / 2), (int)worldLocation.Y + (int)(image.Width / 2));
            }
        }

        /// <summary>
        /// Returns the rectangle of the world
        /// </summary>
        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle((int)worldLocation.X, (int)worldLocation.Y, image.Width, image.Height);
            }
        }
        #endregion

        #region Public Methods
        //Updates the position and location of the hitbox
        public void updateHitbox()
        {
            previousHitBox = hitBox;
            hitBox.X = (int)worldLocation.X;
            hitBox.Y = (int)worldLocation.Y;
            hitBox.Width = image.Width;
            hitBox.Height = image.Height;
        }
        #endregion
          
        #region Helper Methods
        public void Damage(int amount)
        {
            health -= amount;
            if(health <= 0)
            {
                enabled = false;
            }
        }
        #endregion
    }
}
