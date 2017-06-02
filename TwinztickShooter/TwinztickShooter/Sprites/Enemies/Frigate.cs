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

        private bool leftGun = false;

        private static Texture2D bulletImage;
        
        private int curFrame;
        #endregion

        #region Constructor
        public Frigate()
        {
            pointsGained = 100;

            health = 2;

            acceleration = 1;
            direction.X = 1;
        }
        #endregion

        #region Public Methods
        public static void Init(ContentManager cm)
        {
            cm.Load<Texture2D>("Enemies/Enemy Frigate");
            bulletImage = cm.Load<Texture2D>("Bullet");
        }

        public void Update()
        {
            curFrame++;
            worldLocation += direction;

            #region Movement Code
            shipRotation = new Vector2((float)Math.Cos(rotation) * direction.Length(), (float)Math.Sin(rotation) * direction.Length());

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

            if(Math.Abs(rotation - rotationVector) < 0.06f) rotation = (float)rotationVector;

            float speed = direction.Length();
            direction.X = speed * (float)Math.Cos(rotation);
            direction.Y = speed * (float)Math.Sin(rotation);

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

            if (seenPlayer)
            {
                Vector2 distanceVectorToPlayer = knownPlayerInfo.worldLocation - worldLocation;
                Double rotationVectorToPlayer = Math.Atan2(distanceVector.Y, distanceVector.X);

                if ((rotation <= rotationVector + 0.1f && rotation >= rotationVector - 0.1f) && (distanceVector.X < 400 && distanceVector.Y < 400 && distanceVector.X > -400 && distanceVector.Y > -400))
                {
                    if (curFrame % 10 == 0)
                    {
                        Vector2 spawnPosition = new Vector2(worldLocation.X, worldLocation.Y);

                        if (leftGun)
                        {
                            spawnBullet(spawnPosition, direction, 5);
                            leftGun = false;
                        }
                        else if (!leftGun)
                        {
                            spawnBullet(spawnPosition, direction, -10);
                            leftGun = true;
                        }
                    }
                }
            }
            #endregion

            for (int i = 0; i < frigateBullets.Count; i++)
            {
                frigateBullets[i].Update();

                if (frigateBullets[i].Enabled == false)
                    frigateBullets.Remove(frigateBullets[i]);
            }

            UpdateHitbox();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, Camera.WorldToScreen(worldLocation), null, tint, rotation, new Vector2(image.Width / 2, image.Height / 2), 1.0f, SpriteEffects.None, 0);

            for(int i = 0; i < frigateBullets.Count; i++)
            {
                frigateBullets[i].Draw(sb);
            }
        }
        #endregion

        #region Helper Methods
        //Spawns a bullet at a specified location with a set colour for the enemy
        private void spawnBullet(Vector2 position, Vector2 direction, int xOffset)
        {
            Vector2 Velocity = new Vector2(10, 10);
            Vector2 normalizedRotation = shipRotation;
            normalizedRotation.Normalize();
            Color selectedColor = Color.Red;
            Matrix m = Matrix.CreateRotationZ((float)Math.Atan2(shipRotation.Y, shipRotation.X));
            Vector2 v = Vector2.Transform(new Vector2(image.Width / 2 + 3, xOffset), m);
            Bullet newBullet = new Bullet(2);
            newBullet.worldLocation = position + v;
            newBullet.direction = normalizedRotation * Velocity + this.direction;
            newBullet.rotation = rotation + MathHelper.ToRadians(90);
            newBullet.image = bulletImage;
            newBullet.tint = selectedColor;
            frigateBullets.Add(newBullet);
        }
        #endregion
    }
}
