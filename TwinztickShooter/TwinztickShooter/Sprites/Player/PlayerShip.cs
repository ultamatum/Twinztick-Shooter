using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinztickShooter.Gamestates;
using TwinztickShooter.Tile_Engine;

namespace TwinztickShooter.Sprites.Player
{
    class PlayerShip : Sprite
    {
        #region Variables
        private int screenWidth;
        private int screenHeight;
        private int player;
        private int curFrame;
        private float friction = 0.85f;
        private float gpx;
        private float gpy;
        private bool leftGun = true;
        private Random rng = new Random();

        public List<Bullet> bullets = new List<Bullet>();
        private Texture2D bulletImage;

        private Vector2 originPoint;
        private Vector2 acceleration;
        private Vector2 stickPosition;
        private Vector2 shipRotation;
        #endregion

        #region Constructor
        //Sets the default variables for the players
        public PlayerShip(int playerNumber)
        {
            health = 100;
            originPoint = new Vector2(16, 16);
            direction = new Vector2(0, 0);
            acceleration = new Vector2(2, 2);
            player = playerNumber;

            screenWidth = TwinztickShooter.GetScreenWidth();
            screenHeight = TwinztickShooter.GetScreenHeight();

            if(playerNumber == 1)
            {
                worldLocation.X = 3180;
                worldLocation.Y = 3200;
            } else
            {
                worldLocation.X = 3220;
                worldLocation.Y = 3200;
            }
        }
        #endregion

        #region Initialization
        //Loads the neccesary images
        public void Init(ContentManager cm)
        {
            health = 100;
            enabled = true;
            image = cm.Load<Texture2D>("Player/ship" + player);
            bulletImage = cm.Load<Texture2D>("Bullet");
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            curFrame++;

            UpdateHitbox();
            UpdateRotation();
            UpdateDirection();

            worldLocation += direction;
            direction.X *= friction;
            direction.Y *= friction;

            RepositionCamera();

            for (int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Update();

                if (bullets[i].Enabled == false)
                    bullets.Remove(bullets[i]);
            }

            #region Player Dependent Updates
            //Shoots out bullets from the guns on the front of each ship
            switch (player)
            {
                case 1:
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftShoulder) && (curFrame % 5 == 0))
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
                    break;

                case 2:
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightShoulder) && (curFrame % 5 == 0))
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
                    break;
            }
            #endregion

            #region Edge Check
            //Stops the player from moving off the world
            if (worldLocation.X <= 0 + image.Width / 2)
            {
                worldLocation.X = 0 + image.Width / 2;
                direction *= 0;
            }

            if (worldLocation.Y <= 0 + image.Height / 2)
            {
                worldLocation.Y = 0 + image.Height / 2;
                direction *= 0;
            }

            if (worldLocation.X >= (TileMap.MapWidth * 64 - image.Width / 2))
            {
                worldLocation.X = TileMap.MapWidth * 64 - image.Width / 2;
                direction *= 0;
            }

            if (worldLocation.Y >= (TileMap.MapHeight * 64 - image.Height / 2))
            {
                worldLocation.Y = TileMap.MapHeight * 64 - image.Height / 2;
                direction *= 0;
            }
            #endregion
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, Camera.WorldToScreen(worldLocation), null, tint, rotation, originPoint, 1.0f, SpriteEffects.None, 0);

            for (int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Draw(sb);
            }
        }
        #endregion

        #region Helper Methods
        //Updates the rotation by getting the position of the analogue sticks and then using Atan2 to change that angle of the ship
        private void UpdateRotation()
        {
            if (player == 1)
            {
                gpx = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
                gpy = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
                stickPosition = new Vector2(gpx, -gpy);
            }
            else
            {
                gpx = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X;
                gpy = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
                stickPosition = new Vector2(gpx, -gpy);
            }

            if (stickPosition != new Vector2(0, 0))
            {
                shipRotation = new Vector2(gpx, -gpy);
                rotation = (float)Math.Atan2(gpx, gpy);
            }
        }

        //Updates the direction by getting the dot product of the intended direction of the ships and the distance between them to make sure they dont go too far apart
        private void UpdateDirection()
        {
            Vector2 intendedDirection = (direction + (acceleration * shipRotation));
            intendedDirection.Normalize();
            Vector2 distanceNormalized = GamePlay.distanceBetweenShips;
            distanceNormalized.Normalize();
            float scalar = Vector2.Dot(intendedDirection, distanceNormalized);

            if (player == 1)
            {
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftTrigger)){
                    if (!GamePlay.IsFarApart())
                    {
                        direction += (acceleration * shipRotation);
                    }
                    else
                    {
                        if (scalar > 0)
                        {
                            direction += (acceleration * shipRotation) * (float)(Math.Pow(GamePlay.distanceBetweenShips.Length(), -5));
                        }
                        else
                        {
                            direction += (acceleration * shipRotation);
                        }
                    }
                }
            }
            else if (player == 2)
            {
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightTrigger))
                {
                    if (!GamePlay.IsFarApart())
                    {
                        direction += (acceleration * shipRotation);
                    }
                    else
                    {
                        if (scalar < 0)
                        {
                            direction += (acceleration * shipRotation) * (float)(Math.Pow(GamePlay.distanceBetweenShips.Length(), -5));
                        }
                        else
                        {
                            direction += (acceleration * shipRotation);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Moves the camera so the player is always on the screen
        /// </summary>
        private void RepositionCamera()
        {
            int screenLocX = (int)Camera.WorldToScreen(worldLocation).X;
            int screenLocY = (int)Camera.WorldToScreen(worldLocation).Y;

            if (screenLocX > 1680)
            {
                Camera.Move(new Vector2(screenLocX - 1680, 0));
            }

            if (screenLocX < 200)
            {
                Camera.Move(new Vector2(screenLocX - 200, 0));
            }

            if (screenLocY > 820)
            {
                Camera.Move(new Vector2(0, screenLocY - 820));
            }

            if (screenLocY < 200)
            {
                Camera.Move(new Vector2(0, screenLocY - 200));
            }
        }


        //Spawns a bullet at a specified location with a random colour
        private void spawnBullet(Vector2 position, Vector2 direction, int xOffset)
        {
            Vector2 Velocity = new Vector2(10, 10);
            Vector2 normalizedRotation = shipRotation;
            normalizedRotation.Normalize();
            Color selectedColor = new Color(rng.Next(255), rng.Next(255), rng.Next(255));
            Matrix m = Matrix.CreateRotationZ((float)Math.Atan2(shipRotation.Y, shipRotation.X));
            Vector2 v = Vector2.Transform(new Vector2(image.Width / 2 + 3, xOffset), m);
            Bullet newBullet = new Bullet(1);
            newBullet.worldLocation = position + v;
            newBullet.direction = normalizedRotation * Velocity + this.direction;
            newBullet.rotation = rotation;
            newBullet.image = bulletImage;
            newBullet.tint = selectedColor;
            bullets.Add(newBullet);
        }
        #endregion
    }
}
