using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinztickShooter.Tile_Engine;

namespace TwinztickShooter.Gamestates
{
    class GameOver
    {
        #region Declarations
        private Random rng = new Random();
        
        private int screenWidth;
        private int screenHeight;

        private float frameTimer = 0f;

        private bool newHighscoreActive;
        private bool newHighscore;

        private Vector2 cameraGoal;
        
        private SpriteFont font;
        #endregion

        #region Constructor
        public GameOver(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }
        #endregion

        #region Public Methods
        public void Init(ContentManager cm)
        {
            font = cm.Load<SpriteFont>("Pixel Font");

            TileMap.Initialize(cm.Load<Texture2D>("starmap"));

            Camera.ViewPortWidth = 1920;
            Camera.ViewPortHeight = 1080;
            Camera.WorldRectangle = new Rectangle(0, 0, TileMap.MapWidth * TileMap.TileWidth, TileMap.MapHeight * TileMap.TileHeight);
            Camera.Position = new Vector2(((TileMap.MapWidth * TileMap.TileWidth) / 2) - Camera.ViewPortWidth / 2, ((TileMap.MapHeight * TileMap.TileHeight) / 2) - Camera.ViewPortHeight / 2);

            cameraGoal = Camera.Position;
        }

        public void Update(GameTime gameTime)
        {
            MoveCamera();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            frameTimer += elapsed;

            if (frameTimer > 0.25)
            {
                if (newHighscoreActive)
                {
                    newHighscoreActive = false;
                }
                else
                {
                    newHighscoreActive = true;
                }

                frameTimer = 0;
            }

            if(TwinztickShooter.score > TwinztickShooter.highscore)
            {
                newHighscore = true;
                TwinztickShooter.highscore = TwinztickShooter.score;
                TwinztickShooter.SaveHighScore();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(samplerState: SamplerState.PointClamp);
            TileMap.Draw(sb);

            sb.DrawString(font, "GAME OVER!", new Vector2(screenWidth / 2 - font.MeasureString("GAME OVER!").X * 20 / 2, (screenHeight / 8) - 20), Color.Red, 0f, Vector2.Zero, 20f, SpriteEffects.None, 1f);

            sb.DrawString(font, "SCORE: " + TwinztickShooter.score, new Vector2(screenWidth / 2 - font.MeasureString("Score: " + TwinztickShooter.score).X * 5 / 2, (screenHeight / 2) - font.MeasureString("Score: " + TwinztickShooter.score).Y * 5 / 2), Color.Red, 0f, Vector2.Zero, 5f, SpriteEffects.None, 1f);
            sb.DrawString(font, "HIGHSCORE: " + TwinztickShooter.highscore, new Vector2(screenWidth / 2 - font.MeasureString("HIGHSCORE: " + TwinztickShooter.highscore).X * 5 / 2, (screenHeight / 2) + font.MeasureString("HIGHSCORE: " + TwinztickShooter.highscore).Y * 5), Color.Red, 0f, Vector2.Zero, 5f, SpriteEffects.None, 1f);

            if(newHighscore)
            {
                if (newHighscoreActive)
                {
                    sb.DrawString(font, "NEW HIGHSCORE!!!", new Vector2(screenWidth / 2 - font.MeasureString("NEW HIGHSCORE!!!").X * 5 / 2, (screenHeight / 2 + 200)), Color.Red, 0f, Vector2.Zero, 5f, SpriteEffects.None, 1f);
                }
            }

            sb.End();
        }
        #endregion

        #region Helper Methods
        private void MoveCamera()
        {
            if (cameraGoal.X >= Camera.Position.X - 10 && cameraGoal.X <= Camera.Position.X + 50 && cameraGoal.Y >= Camera.Position.Y - 10 && cameraGoal.Y <= Camera.Position.Y)
            {
                cameraGoal.X = rng.Next(0, (TileMap.MapWidth * TileMap.TileWidth) - Camera.ViewPortWidth);
                cameraGoal.Y = rng.Next(0, (TileMap.MapHeight * TileMap.TileHeight) - Camera.ViewPortHeight);
            }

            if (cameraGoal.X < Camera.Position.X)
            {
                Camera.Move(new Vector2(-2, 0));
            }
            else if (cameraGoal.X > Camera.Position.X)
            {
                Camera.Move(new Vector2(+2, 0));
            }

            if (cameraGoal.Y < Camera.Position.Y)
            {
                Camera.Move(new Vector2(0, -2));
            }
            else if (cameraGoal.Y > Camera.Position.Y)
            {
                Camera.Move(new Vector2(0, +2));
            }
        }
        #endregion
    }
}
