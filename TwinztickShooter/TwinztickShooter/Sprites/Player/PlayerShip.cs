using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinztickShooter.Sprites.Player
{
    class PlayerShip : Sprite
    {
        int lives;
        int screenWidth;
        int screenHeight;

        Vector2 originPoint;
        Vector2 acceleration;
        Vector2 stickPosition;
        Vector2 shipRotation;


        public PlayerShip()
        {
            lives = 3;
            originPoint = new Vector2(16, 16);
            direction = new Vector2(0, 0);
            acceleration = new Vector2(2, 2);
            
            screenWidth = Game1.GetScreenWidth();
            screenHeight = Game1.GetScreenHeight();

            position.X = screenWidth / 2;
            position.Y = screenHeight / 2;
        }

        public void Init(ContentManager cm)
        {
            image = cm.Load<Texture2D>("ship");
        }

        public void Update()
        {
            updateHitbox();
            UpdateRotation();

            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftTrigger))
            {
                direction += (acceleration * shipRotation);
            }

            position += direction;
            direction.X *= 0.95f;
            direction.Y *= 0.95f;
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Begin();
            sp.Draw(image, position, null, tint, rotation, originPoint, 1.0f, SpriteEffects.None, 0);
            sp.End();
        }

        private void UpdateRotation()
        {
            float gpx = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
            float gpy = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
            stickPosition = new Vector2(gpx, -gpy);

            if(stickPosition != new Vector2(0, 0))
            {
                shipRotation = new Vector2(gpx, -gpy);
                rotation = (float)Math.Atan2(gpx, gpy);
            }
        }
    }
}
