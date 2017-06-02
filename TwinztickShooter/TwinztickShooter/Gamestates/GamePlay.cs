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
        private SpriteFont font;

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

            Frigate.Init(cm);
            
            TileMap.Initialize(cm.Load<Texture2D>("starmap"));
            frigateImage = cm.Load<Texture2D>("Enemies/Enemy Frigate");
            font = cm.Load<SpriteFont>("Pixel Font");

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
            if(ship1.Enabled) ship1.Update();
            if (ship2.Enabled) ship2.Update();

            if (Vector2.Distance(ship1.worldLocation, ship2.worldLocation) > 500 && ship1.Enabled && ship2.Enabled)
            {
                farApart = true;
            }
            else
            {
                farApart = false;
            }

            distanceBetweenShips = ship1.worldLocation - ship2.worldLocation;

            if(ship1.health <= 0)
            {
                ship1.Enabled = false;
            }

            if(ship2.health <= 0)
            {
                ship2.Enabled = false;
            }

            if(!ship1.Enabled && !ship2.Enabled)
            {
                TwinztickShooter.SwitchGamestate(2);
            }
            #endregion

            #region Enemy Update
            for(int i = 0; i < frigates.Count; i++)
            {
                frigates[i].seenPlayer = false;
                if (ship1.Enabled) frigates[i].HuntForPlayer(ship1);
                if (ship2.Enabled) frigates[i].HuntForPlayer(ship2);
                frigates[i].Update();

                if(frigates[i].health <= 0)
                {
                    TwinztickShooter.score += frigates[i].pointsGained;
                    frigates.Remove(frigates[i]);
                    if(i != 0) i--;
                }

                for(int b = 0; b < ship1.bullets.Count; b++)
                {
                    ship1.bullets[b].CollisionCheck(frigates[i]);
                }

                for (int b = 0; b < ship2.bullets.Count; b++)
                {
                    ship2.bullets[b].CollisionCheck(frigates[i]);
                }

                for (int b = 0; b < frigates[i].frigateBullets.Count; b++)
                {
                    frigates[i].frigateBullets[b].CollisionCheck(ship1);
                    frigates[i].frigateBullets[b].CollisionCheck(ship2);
                }

                if(ship1.hitBox.Intersects(frigates[i].hitBox))
                {
                    TwinztickShooter.score += frigates[i].pointsGained;
                    frigates.Remove(frigates[i]);
                    if (i != 0) i--;
                    ship1.Damage(20);
                }

                if (ship2.hitBox.Intersects(frigates[i].hitBox))
                {
                    TwinztickShooter.score += frigates[i].pointsGained;
                    frigates.Remove(frigates[i]);
                    if (i != 0) i--;
                    ship2.Damage(20);
                }
            }

            if(rng.Next(1000) < 10+ TwinztickShooter.score / 500)
            {
                SpawnFrigate();
            }
            #endregion
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            TileMap.Draw(sb);
            if (ship1.Enabled) ship1.Draw(sb);
            if (ship2.Enabled) ship2.Draw(sb);

            sb.DrawString(font, "SCORE: " + TwinztickShooter.score, new Vector2((TwinztickShooter.screenWidth / 2 - (font.MeasureString("SCORE: " + TwinztickShooter.score).X * 5) / 2), 10), Color.Yellow, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);

            for (int i = 0; i < frigates.Count; i++)
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
