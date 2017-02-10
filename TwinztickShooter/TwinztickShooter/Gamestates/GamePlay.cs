using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinztickShooter.Sprites;
using TwinztickShooter.Sprites.Player;

namespace TwinztickShooter.Gamestates
{
    class GamePlay
    {
        public PlayerShip ship = new PlayerShip();

        public GamePlay()
        {
            
        }

        public void Init(ContentManager cm)
        {
            ship.Init(cm);
        }

        public void Update()
        {
            ship.Update();
        }

        public void Draw(SpriteBatch sp)
        {
            ship.Draw(sp);
        }
    }
}
