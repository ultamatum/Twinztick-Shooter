using Microsoft.Xna.Framework;
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
        public Texture2D image;
        public Vector2 worldLocation;
        public Color tint = Color.White;
        public Vector2 direction;
        public float rotation;
        public Rectangle hitBox;
        public Rectangle previousHitBox;

        protected bool enabled;
        protected float drawDepth = 11.85f;

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
                return new Vector2((int)worldLocation.X + (int)(image.Width / 2), (int)worldLocation.Y + (int)(image.Width / 2));
            }
        }

        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle((int)worldLocation.X, (int)worldLocation.Y, image.Width, image.Height);
            }
        }
        
        #endregion

        public void KnockAway(Sprite target)
        {
            // return the target back to where it is just touching the edge of our sprite
            do
            {
                target.worldLocation -= target.direction / 10;
                target.updateHitbox();
            } while (hitBox.Intersects(target.hitBox));

            // preserve target's initial speed
            float initialSpeed = target.direction.Length();

            // find a vector directing the target away
            target.direction.Y = target.hitBox.Center.Y - hitBox.Center.Y;
            target.direction.X = target.hitBox.Center.X - hitBox.Center.X;

            // make sure it isnt close to vertical or horizontal
            if (target.direction.X == 0) target.direction.X = -1;
            if (target.direction.Y == 0) target.direction.Y = -1;

            while (Math.Abs(target.direction.X / target.direction.Y) < 0.1f)
                target.direction.X *= 2;
            while (Math.Abs(target.direction.Y / target.direction.X) < 0.1f)
                target.direction.Y *= 2;

            // normalise it and send it on its way, preserving speed
            target.direction.Normalize();
            target.direction *= initialSpeed;
        }

        public void BounceOff(Sprite target)
        {
            if (previousHitBox.Top >= target.previousHitBox.Bottom)
            {
                // hit from bottom
                direction.Y = Math.Abs(direction.Y);
                worldLocation.Y = target.worldLocation.Y + target.image.Height + target.direction.Y;
            }
            else if (previousHitBox.Bottom <= target.previousHitBox.Top + 1)
            {
                // hit from top
                direction.Y = -Math.Abs(direction.Y);
                worldLocation.Y = target.worldLocation.Y - image.Height + target.direction.Y;
            }
            else if (previousHitBox.Left >= target.previousHitBox.Right)
            {
                // hit from right
                direction.X = Math.Abs(direction.X);
                worldLocation.X = target.worldLocation.X + target.image.Width + target.direction.X;
            }
            else if (previousHitBox.Right <= target.previousHitBox.Left)
            {
                // hit from left
                direction.X = -Math.Abs(direction.X);
                worldLocation.X = target.worldLocation.X - image.Width + target.direction.X;
            }
        }

        public void updateHitbox()
        {
            previousHitBox = hitBox;
            hitBox.X = (int)worldLocation.X;
            hitBox.Y = (int)worldLocation.Y;
            hitBox.Width = image.Width;
            hitBox.Height = image.Height;
        }
    }
}
