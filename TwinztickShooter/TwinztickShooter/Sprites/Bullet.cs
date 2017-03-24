using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinztickShooter.Sprites
{
    class Bullet : Sprite_Old
    {
        Vector2 originPoint;

        public Bullet()
        {
        }

        public void Update()
        {
            position += direction;
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Begin();
            sp.Draw(image, position, null, tint, rotation, originPoint, 2.0f, SpriteEffects.None, 0);
            sp.End();
        }

    }
}
