using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinztickShooter.Sprites;
using TwinztickShooter.Sprites.Enemies;
using TwinztickShooter.Sprites.Player;
using TwinztickShooter.Tile_Engine;

namespace TwinztickShooter.Gamestates
{
    class GamePlay
    {
        #region Declarations
        public PlayerShip ship1 = new PlayerShip(1);
        public PlayerShip ship2 = new PlayerShip(2);

        public List<Frigate> frigates = new List<Frigate>();

        public Texture2D frigateImage;

        private Random rng = new Random();

        static bool farApart = false;
        public static Vector2 distanceBetweenShips = new Vector2();
        #endregion

        #region Constructor
        public GamePlay()
        {
            
        }
        #endregion
          
        #region Initialization
        public void Init(ContentManager cm)
        {
            ship1.Init(cm);
            ship2.Init(cm);
            
            TileMap.Initialize(cm.Load<Texture2D>("starmap"));
            frigateImage = cm.Load<Texture2D>("Enemies/Enemy Frigate");

            Camera.ViewPortWidth = 1920;
            Camera.ViewPortHeight = 1080;
            Camera.WorldRectangle = new Rectangle(0, 0, TileMap.MapWidth * TileMap.TileWidth, TileMap.MapHeight * TileMap.TileHeight);
            Camera.Position = new Vector2(((TileMap.MapWidth * TileMap.TileWidth) / 2) - Camera.ViewPortWidth / 2, ((TileMap.MapHeight * TileMap.TileHeight) / 2) - Camera.ViewPortHeight / 2);
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

            #region Enemy Update
            for(int i = 0; i < frigates.Count; i++)
            {
                frigates[i].HuntForPlayer(ship1);
                frigates[i].HuntForPlayer(ship2);
                frigates[i].Update();
            }

            //if(rng.Next(10) == 6)
            //{
                SpawnFrigate();
            //}
            #endregion
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            TileMap.Draw(sb);
            ship1.Draw(sb);
            ship2.Draw(sb);

            for(int i = 0; i < frigates.Count; i++)
            {
                frigates[i].Draw(sb);
            }
            sb.End();
        }

        //Figures out if the player's ships are far enough apart that they can no longer move apart
        public static bool IsFarApart()
        {
            return farApart;
        }
        #endregion
          
        #region Helper Methods
        private void SpawnFrigate()
        {
            Frigate newFrigate = new Frigate();
            newFrigate.worldLocation = new Vector2(rng.Next(TileMap.TileWidth * TileMap.MapWidth), rng.Next(TileMap.TileHeight * TileMap.MapHeight));
            newFrigate.image = frigateImage;
            newFrigate.tint = Color.White;
            frigates.Add(newFrigate);
        }
      
        public void startup()
        {
            Camera.ViewPortWidth = 1920;
            Camera.ViewPortHeight = 1080;
            Camera.WorldRectangle = new Rectangle(0, 0, TileMap.MapWidth * TileMap.TileWidth, TileMap.MapHeight * TileMap.TileHeight);
            Camera.Position = new Vector2(((TileMap.MapWidth * TileMap.TileWidth) / 2) - Camera.ViewPortWidth / 2, ((TileMap.MapHeight * TileMap.TileHeight) / 2) - Camera.ViewPortHeight / 2);
        }
      #endregion
    }
}
