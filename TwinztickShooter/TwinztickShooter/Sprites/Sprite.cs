using AnagramGame.Sprites.Animations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramGame.Sprites
{
    public class Sprite
    {
        #region Declarations
        protected Vector2 worldLocation;
        protected Vector2 velocity;
        protected int frameWidth;
        protected int frameHeight;

        protected bool enabled;
        protected bool flipped = false;
        protected bool onGround;

        protected Rectangle collisionRectangle;
        protected int collideWidth;
        protected int collideHeight;
        protected bool codeBasedBlocks = true;

        protected float drawDepth = 0.85f;
        protected Dictionary<String, AnimationStrip> animations = new Dictionary<string, AnimationStrip>();
        protected string currentAnimation;
        #endregion

        #region Properties
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public Vector2 WorldLocation
        {
            get { return worldLocation; }
            set { worldLocation = value; }
        }

        public Vector2 WorldCenter
        {
            get
            {
                return new Vector2((int)worldLocation.X + (int)(frameWidth / 2), (int)worldLocation.Y + (int)(frameHeight / 2));
            }
        }

        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle((int)worldLocation.X, (int)worldLocation.Y, frameWidth, frameHeight);
            }
        }

        public Rectangle CollisionRectange
        {
            get
            {
                return new Rectangle((int)worldLocation.X + collisionRectangle.X, (int)WorldRectangle.Y + collisionRectangle.Y, collisionRectangle.Width, collisionRectangle.Height);
            }
            set { collisionRectangle = value; }
        }
        #endregion

        #region Helper Methods
        private void UpdateAnimation(GameTime gameTime)
        {
            if(animations.ContainsKey(currentAnimation))
            {
                if(animations[currentAnimation].FinishedPlaying)
                {
                    //PlayAnimation(animations[currentAnimation].NextAnimation);
                }
                else
                {
                    animations[currentAnimation].Update(gameTime);
                }
            }
        }
        #endregion

    }
}
