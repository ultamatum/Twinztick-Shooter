﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinztickShooter.Gamestates;

namespace TwinztickShooter.Sprites.Player
{
    class PlayerShip : Sprite_Old
    {
        int lives;
        int screenWidth;
        int screenHeight;
        int player;
        int curFrame;
        float friction = 0.85f;
        float gpx;
        float gpy;
        bool leftGun = true;
        Random rng = new Random();

        List<Bullet> bullets = new List<Bullet>();
        Texture2D bulletImage;

        Vector2 originPoint;
        Vector2 acceleration;
        Vector2 stickPosition;
        Vector2 shipRotation;


        public PlayerShip(int playerNumber)
        {
            lives = 3;
            originPoint = new Vector2(16, 16);
            direction = new Vector2(0, 0);
            acceleration = new Vector2(2, 2);
            player = playerNumber;

            screenWidth = TwinztickShooter.GetScreenWidth();
            screenHeight = TwinztickShooter.GetScreenHeight();

            position.X = screenWidth / 2;
            position.Y = screenHeight / 2;
        }

        public void Init(ContentManager cm)
        {
            image = cm.Load<Texture2D>("ship");
            bulletImage = cm.Load<Texture2D>("Bullet");
        }

        public void Update()
        {
            curFrame++;

            updateHitbox();
            UpdateRotation();
            UpdateDirection();

            position += direction;
            direction.X *= friction;
            direction.Y *= friction;

            for (int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Update();
            }

            #region Player Dependent Updates
            switch (player)
            {
                case 1:
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftShoulder) && (curFrame % 5 == 0))
                    {
                        Vector2 spawnPosition = new Vector2(position.X, position.Y);

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
                        Vector2 spawnPosition = new Vector2(position.X, position.Y);

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
            if (position.X <= 0 + image.Width / 2)
            {
                position.X = 0 + image.Width / 2;
                direction *= 0;
            }
            if (position.Y <= 0 + image.Height / 2)
            {
                position.Y = 0 + image.Height / 2;
                direction *= 0;
            }
            if (position.X >= (screenWidth - image.Width / 2))
            {
                position.X = screenWidth - image.Width / 2;
                direction *= 0;
            }
            if (position.Y >= (screenHeight - image.Height / 2))
            {
                position.Y = screenHeight - image.Height / 2;
                direction *= 0;
            }
            #endregion
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Begin();
            sp.Draw(image, position, null, tint, rotation, originPoint, 1.0f, SpriteEffects.None, 0);
            sp.End();

            for (int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Draw(sp);
            }
        }

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

        public void spawnBullet(Vector2 position, Vector2 direction, int xOffset)
        {
            Vector2 Velocity = new Vector2(10, 10);
            Vector2 normalizedRotation = shipRotation;
            normalizedRotation.Normalize();
            Color selectedColor = new Color(rng.Next(255), rng.Next(255), rng.Next(255));
            Matrix m = Matrix.CreateRotationZ((float)Math.Atan2(shipRotation.Y, shipRotation.X));
            Vector2 v = Vector2.Transform(new Vector2(image.Width / 2 + 3, xOffset), m);
            Bullet newBullet = new Bullet();
            newBullet.position = position + v;
            newBullet.direction = normalizedRotation * Velocity + this.direction;
            newBullet.rotation = rotation;
            newBullet.image = bulletImage;
            newBullet.tint = selectedColor;
            bullets.Add(newBullet);
        }
    }
}
