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
        

        public PlayerShip()
        {
            lives = 3;
            originPoint = new Vector2(16, 16);
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
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Begin();
            sp.Draw(image, position, null, tint, rotation, originPoint, 4.0f, SpriteEffects.None, 0);
            sp.End();
        }

        private void UpdateRotation()
        {
            float gpx = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
            float gpy = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;

            if (gpy != 0 && gpx != 0)
            {
                rotation = (float)Math.Atan2(x, y);
            }
        }
    }
}
