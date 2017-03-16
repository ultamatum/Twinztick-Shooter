using Microsoft.Xna.Framework;
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
        public PlayerShip ship1 = new PlayerShip(false);
        public PlayerShip ship2 = new PlayerShip(true);
        static bool farApart = false;

        public GamePlay()
        {
            
        }

        public void Init(ContentManager cm)
        {
            ship1.Init(cm);
            ship2.Init(cm);
        }

        public void Update()
        {
            ship1.Update();
            ship2.Update();

            if(Vector2.Distance(ship1.position, ship2.position) > 1000)
            {
                farApart = true;
            }
        }

        public void Draw(SpriteBatch sp)
        {
            ship1.Draw(sp);
            ship2.Draw(sp);
        }

        public static bool IsFarApart()
        {
            return farApart;
        }
    }
}
