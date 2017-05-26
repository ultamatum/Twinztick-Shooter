using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            acceleration = 10;
        }
        #endregion

        #region Public Methods
        public void Init(ContentManager cm)
        {
            
        }

        public void Update()
        {
            worldLocation += direction;

            if(!seenPlayer)
            {
                Vector2 distanceVector = goalLocation - worldLocation;
                Double rotationVector = Math.Atan2(distanceVector.Y, distanceVector.X);

                if(rotation < rotationVector + 0.006f && rotation > rotationVector - 0.006f)
                {
                    if(rotationVector < rotation)
                    {
                        rotation -= 0.006f;
                    } else if(rotationVector > rotation)
                    {
                        rotation += 0.006f;
                    }
                } else
                {
                    direction.X += acceleration;
                    direction.Y += acceleration;
                }
            } else
            {

            }
        }

        public void Draw()
        {

        }
        #endregion
    }
}
