using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinztickShooter.Tile_Engine;

namespace TwinztickShooter.Sprites
{
    class Bullet : Sprite
    {
        #region Declarations
        private Vector2 originPoint;

        private int damage = 1;
        #endregion

        #region Constructor
        public Bullet()
        {
            enabled = true;
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            worldLocation += direction;

            UpdateHitbox();

            if (worldLocation.X < 0 || worldLocation.X > (TileMap.MapWidth * TileMap.TileWidth) || worldLocation.Y < 0 || worldLocation.Y > (TileMap.MapHeight * TileMap.TileHeight))
                enabled = false;
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Draw(image, Camera.WorldToScreen(worldLocation), null, tint, rotation, originPoint, 2.0f, SpriteEffects.None, 0);
        }
        #endregion
        
        #region Helper Methods
        /// <summary>
        /// Checks if the bullet has collided with something then gets removed and damages the sprite
        /// </summary>
        /// <param name="sprite">The sprite to check collision with</param>
        public void CollisionCheck(Sprite sprite)
        {
            if(hitBox.Intersects(sprite.hitBox))
            {
                enabled = false;
                sprite.Damage(damage);
            }
        }
        #endregion
    }
}
