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
using TwinztickShooter.Tile_Engine;

namespace TwinztickShooter.Gamestates
{
    class GamePlay
    {
        #region Variables
        public PlayerShip ship1 = new PlayerShip(1);
        public PlayerShip ship2 = new PlayerShip(2);

        static bool farApart = false;
        public static Vector2 distanceBetweenShips = new Vector2();
        #endregion

        #region Constructor
        public GamePlay(){}
        #endregion

        #region Initialization
        public void Init(ContentManager cm)
        {
            ship1.Init(cm);
            ship2.Init(cm);

            #region Tile Engine set-up
            //Sets up both the Tile Map and the Camera to be the default variables
            TileMap.Initialize(cm.Load<Texture2D>("starmap"));

            Camera.ViewPortWidth = 1920;
            Camera.ViewPortHeight = 1080;
            Camera.WorldRectangle = new Rectangle(0, 0, TileMap.MapWidth * TileMap.TileWidth, TileMap.MapHeight * TileMap.TileHeight);
            Camera.Position = new Vector2(((TileMap.MapWidth * TileMap.TileWidth) / 2) - Camera.ViewPortWidth / 2, ((TileMap.MapHeight * TileMap.TileHeight) / 2) - Camera.ViewPortHeight / 2);
            #endregion
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            #region Ship update
            ship1.Update();
            ship2.Update();

            if (Vector2.Distance(ship1.worldLocation, ship2.worldLocation) > 500)
            {
                farApart = true;
            }
            else
                farApart = false;

            distanceBetweenShips = ship1.worldLocation - ship2.worldLocation;
            #endregion
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            TileMap.Draw(sp);
            ship1.Draw(sp);
            ship2.Draw(sp);
            sp.End();
        }

        //Figures out if the player's ships are far enough apart that they can no longer move apart
        public static bool IsFarApart()
        {
            return farApart;
        }
        #endregion
    }
}
