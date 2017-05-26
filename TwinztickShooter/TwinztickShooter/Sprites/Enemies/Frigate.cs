using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinztickShooter.Tile_Engine;

namespace TwinztickShooter.Sprites.Enemies
{
    class Frigate : Enemy
    {
        #region Declarations
        public List<Bullet> frigateBullets = new List<Bullet>();

        private Vector2 shipRotation;
        #endregion

        #region Constructor
        public Frigate()
        {
            pointsGained = 100;

            

            acceleration = 1;
            direction.X = 1;
        }
        #endregion

        #region Public Methods
        public void Init(ContentManager cm)
        {
            cm.Load<Texture2D>("Enemies/Enemy Frigate");
        }

        public void Update()
        {
            worldLocation += direction;
            
            Vector2 distanceVector = goalLocation - worldLocation;
            Double rotationVector = Math.Atan2(distanceVector.Y, distanceVector.X);

            rotation = MathHelper.WrapAngle(rotation);

            if (Math.Abs(goalLocation.Y-worldLocation.Y) < 1) worldLocation.Y -= 1;

            //Handling top-left quadrant
            if (rotationVector >= MathHelper.PiOver2 && rotation < -MathHelper.PiOver2)
            {
                rotation -= 0.06f;
            } else if(rotationVector <= -MathHelper.PiOver2 && rotation > MathHelper.PiOver2)
            {
                rotation += 0.06f;
            }else if (rotation > rotationVector + 0.006f || rotation < rotationVector - 0.006f)
            {
                if (rotationVector < rotation)
                {
                    rotation -= 0.06f;
                }
                else if (rotationVector > rotation)
                {
                    rotation += 0.06f;
                }
            }
            else
            {
                direction.X += acceleration;
                direction.Y += acceleration;
            }

            float speed = direction.Length();
            direction.X = speed * (float)Math.Cos(rotation);
            direction.Y = speed * (float)Math.Sin(rotation);

            if (direction.X == 0)
            {
                int i = 0;
            }

            if (direction.X >= 1)
            {
                direction.X = 1;
            }

            if (direction.Y >= 1)
            {
                direction.Y = 1;
            }

            if (direction.X <= -1)
            {
                direction.X = -1f;
            }

            if (direction.Y <= -1)
            {
                direction.Y = -1;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, Camera.WorldToScreen(worldLocation), null, tint, rotation, new Vector2(image.Width / 2, image.Height / 2), 1.0f, SpriteEffects.None, 0);
        }
        #endregion
    }
}
