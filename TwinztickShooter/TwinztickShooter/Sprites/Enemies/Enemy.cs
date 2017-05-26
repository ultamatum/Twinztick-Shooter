using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinztickShooter.Sprites.Player;
using TwinztickShooter.Tile_Engine;

namespace TwinztickShooter.Sprites.Enemies
{
    class Enemy : Sprite
    {
        #region Declarations
        public int shotDamage = 1;
        public int pointsGained = 10;

        public int acceleration;

        private Random rng = new Random();

        public bool seenPlayer;
        public Vector2 goalLocation;
        public PlayerShip knownPlayerInfo;
        #endregion

        #region Public Methods
        public void HuntForPlayer(PlayerShip player)
        {
            Vector2 playerPosition = player.worldLocation;
            Vector2 distanceVector = playerPosition - worldLocation;
            Double rotationVector = Math.Atan2(distanceVector.Y, distanceVector.X);

            rotationVector = MathHelper.WrapAngle((float)rotationVector);
            
            if ((rotation <= rotationVector + 0.5f && rotation  >= rotationVector - 0.5f) && (distanceVector.X < 700 && distanceVector.Y < 700 && distanceVector.X > -700 && distanceVector.Y > -700))
            {
                knownPlayerInfo = player;
                goalLocation = player.worldLocation;
                seenPlayer = true;
            } else if(worldLocation.X >= goalLocation.X - 10 && worldLocation.X <= goalLocation.X + 10 && worldLocation.Y >= goalLocation.Y - 10 && worldLocation.Y <= goalLocation.Y + 10 || goalLocation == Vector2.Zero)
            {
                goalLocation.X = rng.Next(TileMap.TileWidth * TileMap.MapWidth);
                goalLocation.Y = rng.Next(TileMap.TileHeight * TileMap.MapHeight);
                seenPlayer = false;
            } else
            {
                seenPlayer = false;
            }
        }
        #endregion
    }
}
