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
        public PlayerShip ship1 = new PlayerShip(1);
        public PlayerShip ship2 = new PlayerShip(2);
        static bool farApart = false;
        public static Vector2 distanceBetweenShips = new Vector2();
        public List<Bullet> bullets = new List<Bullet>();

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

            if (Vector2.Distance(ship1.position, ship2.position) > 300)
            {
                farApart = true;
            }
            else
                farApart = false;

            distanceBetweenShips = ship1.position - ship2.position;
        }

        public void Draw(SpriteBatch sp)
        {
            ship1.Draw(sp);
            ship2.Draw(sp);
        }

        public void spawnBullet(Vector2 position, Vector2 direction)
        {

        }

        public static bool IsFarApart()
        {
            return farApart;
        }
    }
}
