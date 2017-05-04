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
        Vector2 originPoint;
        #endregion

        #region Constructor
        public Bullet() {}
        #endregion

        #region Public Methods
        public void Update()
        {
            worldLocation += direction;
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Draw(image, Camera.WorldToScreen(worldLocation), null, tint, rotation, originPoint, 2.0f, SpriteEffects.None, 0);
        }
        #endregion
    }
}
