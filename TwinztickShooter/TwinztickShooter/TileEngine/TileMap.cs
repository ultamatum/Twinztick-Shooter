using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TwinztickShooter.Tile_Engine
{
    public static class TileMap
    {
        #region Declarations
        public const int TileWidth = 64;                //Width of each tile on the map.
        public const int TileHeight = 64;               //Height of each tile on the map.
        public const int MapWidth = 100;                 //The amount of tiles that go accross the length of the map
        public const int MapHeight = 100;                //The amount of tiles that go accross the height of the map
        public const int MapLayers = 3;                 //The amount of layers the map has.

        static private MapSquare[,] mapCells = new MapSquare[MapWidth, MapHeight];      //An array of blocks that cover the map

        public static bool EditorMode = false;          //Whether or not the map is being edited just now;

        public static SpriteFont spriteFont;            //The font do display (only on editor mode)
        static private Texture2D tileSheet;             //The tilesheet containing all of the tiles.
        #endregion

        #region Initialization
        /// <summary>
        /// Fills in all of the blocks on the map with their default values (sky block on background block)
        /// </summary>
        /// <param name="tileTexture">The tilesheet to be used</param>
        static public void Initialize(Texture2D tileTexture)
        {
            tileSheet = tileTexture;

            Random rng = new Random();

            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    for (int z = 0; z < MapLayers; z++)
                    {
                        
                        mapCells[x, y] = new MapSquare(rng.Next(480), 0, 0, "", true);
                    }
                }
            }
        }
        #endregion

        #region Tile and Tile Sheet Handling
        /// <summary>
        /// Returns how many tiles are in a row on the tile sheet.
        /// </summary>
        public static int TilesPerRow
        {
            get { return tileSheet.Width / TileWidth; }
        }

        /// <summary>
        /// The rectangle that has to be drawn to the screen
        /// </summary>
        /// <param name="tileIndex">The tileID that is being checked</param>
        /// <returns></returns>
        public static Rectangle TileSourceRectangle(int tileIndex)
        {
            return new Rectangle((tileIndex % TilesPerRow) * TileWidth, (tileIndex / TilesPerRow) * TileHeight, TileWidth, TileHeight);
        }
        #endregion

        #region Information about Map Cells
        /// <summary>
        /// Gets the tile under the specific X coordinate in the world
        /// </summary>
        /// <param name="pixelX">The X location of the tile</param>
        /// <returns></returns>
        static public int GetCellByPixelX(int pixelX)
        {
            return pixelX / TileWidth;
        }

        /// <summary>
        /// Gets the tile under the specific Y coordinate in the world
        /// </summary>
        /// <param name="pixelX">The Y location of the tile</param>
        /// <returns></returns>
        static public int GetCellByPixelY(int pixelY)
        {
            return pixelY / TileHeight;
        }

        /// <summary>
        /// Gets the tile under the coordinates given on the tilesheet
        /// </summary>
        /// <param name="pixelLocation">The vector of the location to check on the tilesheet</param>
        /// <returns></returns>
        static public Vector2 GetCellByPixel(Vector2 pixelLocation)
        {
            return new Vector2(GetCellByPixelX((int)pixelLocation.X), GetCellByPixelY((int)pixelLocation.Y));
        }

        /// <summary>
        /// Gets the center of the requested cell
        /// </summary>
        /// <param name="cellX">The X location of the tile on the tilesheet</param>
        /// <param name="cellY">The Y location of the tile on the tilesheet</param>
        /// <returns></returns>
        static public Vector2 GetCellCenter(int cellX, int cellY)
        {
            return new Vector2((cellX * TileWidth) + (TileWidth / 2), (cellY * TileHeight) + (TileHeight / 2));
        }

        /// <summary>
        /// Gets the center of the requested tile
        /// </summary>
        /// <param name="cell">The vector position of the tile on the tilesheet</param>
        /// <returns></returns>
        static public Vector2 GetCellCenter(Vector2 cell)
        {
            return GetCellCenter((int)cell.X, (int)cell.Y);
        }

        /// <summary>
        /// Returns the rectangle of a tile.
        /// </summary>
        /// <param name="cellX">The X position of the tile on the tilesheet</param>
        /// <param name="cellY">The Y position of the tile on the world</param>
        /// <returns></returns>
        static public Rectangle CellWorldRectangle(int cellX, int cellY)
        {
            return new Rectangle(cellX * TileWidth, cellY * TileHeight, TileWidth, TileHeight);
        }

        /// <summary>
        /// Returns the rectangle of a tile in the world
        /// </summary>
        /// <param name="cell">The position of the tile in the world</param>
        /// <returns></returns>
        static public Rectangle CellWorldRectangle(Vector2 cell)
        {
            return CellWorldRectangle((int)cell.X, (int)cell.Y);
        }

        /// <summary>
        /// Returns the rectangle of a tile on the screen
        /// </summary>
        /// <param name="cellX">The world X coordinate of the tile</param>
        /// <param name="cellY">The world Y coordinate of the tile</param>
        /// <returns></returns>
        static public Rectangle CellScreenRectangle(int cellX, int cellY)
        {
            return Camera.WorldToScreen(CellWorldRectangle(cellX, cellY));
        }

        /// <summary>
        /// Returns the rectangle of a tile on the screen
        /// </summary>
        /// <param name="cell"The world position of the tile></param>
        /// <returns></returns>
        static public Rectangle CellSreenRectangle(Vector2 cell)
        {
            return CellScreenRectangle((int)cell.X, (int)cell.Y);
        }

        /// <summary>
        /// Returns if the tile given is passable
        /// </summary>
        /// <param name="cellX">The X position of the tile in the world </param>
        /// <param name="cellY">The Y position of the tile in the world </param>
        /// <returns></returns>
        static public bool CellIsPassable(int cellX, int cellY)
        {
            MapSquare square = GetMapSquareAtCell(cellX, cellY);

            if (square == null)
                return false;
            else
                return square.Passable;
        }

        /// <summary>
        /// Returns if the tile given is passable
        /// </summary>
        /// <param name="cell">The word position of the tile</param>
        /// <returns></returns>
        static public bool CellIsPassable(Vector2 cell)
        {
            return CellIsPassable((int)cell.X, (int)cell.Y);
        }

        /// <summary>
        /// Returns if the tile under a certain pixel is passable
        /// </summary>
        /// <param name="pixelLocation">The world position of the pixel</param>
        /// <returns></returns>
        static public bool CellIsPassableByPixel(Vector2 pixelLocation)
        {
            return CellIsPassable(GetCellByPixelX((int)pixelLocation.X), GetCellByPixelY((int)pixelLocation.Y));
        }

        /// <summary>
        /// Returns the cell code value of a specific cell
        /// </summary>
        /// <param name="cellX">The X position of the cell in the world</param>
        /// <param name="cellY">The Y position of the cell in the world</param>
        /// <returns></returns>
        static public string CellCodeValue(int cellX, int cellY)
        {
            MapSquare square = GetMapSquareAtCell(cellX, cellY);

            if (square == null)
                return "";
            else
                return square.CodeValue;
        }

        /// <summary>
        /// Returns the cell code value of a specific cell
        /// </summary>
        /// <param name="cell">The world position of the cell</param>
        /// <returns></returns>
        static public string CellCodeValue(Vector2 cell)
        {
            return CellCodeValue((int)cell.X, (int)cell.Y);
        }
        #endregion

        #region Information about MapSquare objects
        /// <summary>
        /// Returns the MapSquare object of the tile
        /// </summary>
        /// <param name="tileX">The X coordinate of the tile in the world</param>
        /// <param name="tileY">The Y coordinate of the tile in the world</param>
        /// <returns></returns>
        static public MapSquare GetMapSquareAtCell(int tileX, int tileY)
        {
            if ((tileX >= 0) && (tileX < MapWidth) && (tileY >= 0) && (tileY < MapHeight))
            {
                return mapCells[tileX, tileY];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the MapSquare object at a specific tile
        /// </summary>
        /// <param name="tileX">The X position of the tile in the world</param>
        /// <param name="tileY">The Y position of the tile in the world</param>
        /// <param name="tile">The MapSquare object to place</param>
        static public void SetMapSquareAtCell(int tileX, int tileY, MapSquare tile)
        {
            if ((tileX >= 0) && (tileX < MapWidth) && (tileY >= 0) && (tileY < MapHeight))
            {
                mapCells[tileX, tileY] = tile;
            }
        }

        /// <summary>
        /// Sets the texture of a tile.
        /// </summary>
        /// <param name="tileX">The X position of the tile in the world</param>
        /// <param name="tileY">The Y position of the tile in the world</param>
        /// <param name="layer">What layer the texture has to be placed at</param>
        /// <param name="tileIndex">The TileID of the texture that has to be placed</param>
        static public void SetTileAtCell(int tileX, int tileY, int layer, int tileIndex)
        {
            if ((tileX >= 0) && (tileX < MapWidth) && (tileY >= 0) && (tileY < MapHeight))
            {
                mapCells[tileX, tileY].LayerTiles[layer] = tileIndex;
            }
        }

        /// <summary>
        /// Returns the MapSquare object at the specified pixel
        /// </summary>
        /// <param name="pixelX">The X coordinate of the pixel to return</param>
        /// <param name="pixelY">The Y coordinate of the pixel to return</param>
        /// <returns></returns>
        static public MapSquare GetMapSquareAtPixel(int pixelX, int pixelY)
        {
            return GetMapSquareAtCell(GetCellByPixelX(pixelX), GetCellByPixelY(pixelY));
        }

        /// <summary>
        /// Returns the MapSquare object at the specified location
        /// </summary>
        /// <param name="pixelLocation">The pixel location to check</param>
        /// <returns></returns>
        static public MapSquare GetMapSquareAtPixel(Vector2 pixelLocation)
        {
            return GetMapSquareAtPixel((int)pixelLocation.X, (int)pixelLocation.Y);
        }
        #endregion
        

        #region Drawing
        /// <summary>
        /// Draws all the tiles to the screen
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use to draw the files</param>
        static public void Draw(SpriteBatch spriteBatch)
        {
            int startX = GetCellByPixelX((int)Camera.Position.X);                                       //The starting cell in the X axis that is on the screen so it knows where to start drawing
            int endX = GetCellByPixelX((int)Camera.Position.X + Camera.ViewPortWidth);                  //The ending cell in the X axis that is on the screen so it knows where to stop drawing
            int startY = GetCellByPixelY((int)Camera.Position.Y);                                       //The starting cell in the Y axis that is on the screen so it knows where to start drawing
            int endY = GetCellByPixelY((int)Camera.Position.Y + Camera.ViewPortHeight);                 //The ending cell in the Y axis that is on the screen so it knows where to stop drawing

            //Loops through all of the tiles that are on the screen and draws them
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    for (int z = 0; z < MapLayers; z++)
                    {
                        if ((x >= 0) && (y >= 0) &&
                            (x < MapWidth) && (y < MapHeight))
                        {
                            spriteBatch.Draw(tileSheet, CellScreenRectangle(x, y), TileSourceRectangle(mapCells[x, y].LayerTiles[z]), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f - ((float)z * 0.1f));
                        }
                    }

                    //If the editor is open it draws all of the edit mode items
                    if (EditorMode)
                    {
                        DrawEditModeItems(spriteBatch, x, y);
                    }
                }
            }
        }

        /// <summary>
        /// Draws all of the edit mode items
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use to draw the items</param>
        /// <param name="x">The X coordinate of the item</param>
        /// <param name="y">The Y cordinate of the item</param>
        public static void DrawEditModeItems(SpriteBatch spriteBatch, int x, int y)
        {
            if ((x < 0) || (x >= MapWidth) || (y < 0) || (y >= MapHeight))
                return;

            if (!CellIsPassable(x, y))
            {
                spriteBatch.Draw(tileSheet, CellScreenRectangle(x, y), TileSourceRectangle(1), new Color(255, 0, 0, 80), 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            }

            if (mapCells[x, y].CodeValue != "")
            {
                Rectangle screenRect = CellScreenRectangle(x, y);

                spriteBatch.DrawString(spriteFont, mapCells[x, y].CodeValue, new Vector2(screenRect.X, screenRect.Y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }
        }
        #endregion

        public static void Update()
        {
            Random rng = new Random();

            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    for (int z = 0; z < MapLayers; z++)
                    {

                        mapCells[x, y] = new MapSquare(rng.Next(480), 0, 0, "", true);
                    }
                }
            }
        }

    }
}
