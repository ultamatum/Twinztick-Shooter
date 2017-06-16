using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinztickShooter.Tile_Engine;

namespace TwinztickShooter.Gamestates
{
    class Menu
    {
        #region Declarations
        private int currentChoice = 0;
        private Random rng = new Random();
        private String[] options = new String[2] {
            "PLAY",
            "QUIT"
        };
        
        private int screenWidth;
        private int screenHeight;
        private int optionTimer;

        private Vector2 cameraGoal;

        private Texture2D logo;
        private SpriteFont font;
        #endregion

        #region Constructor
        public Menu(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }
        #endregion

        #region Initialization
        public void Init(ContentManager cm)
        {
            logo = cm.Load<Texture2D>("Logo");
            
            font = cm.Load<SpriteFont>("Pixel Font");

            TileMap.Initialize(cm.Load<Texture2D>("starmap"));

            optionTimer = 40;

            Camera.ViewPortWidth = 1920;
            Camera.ViewPortHeight = 1080;
            Camera.WorldRectangle = new Rectangle(0, 0, TileMap.MapWidth * TileMap.TileWidth, TileMap.MapHeight * TileMap.TileHeight);
            Camera.Position = new Vector2(((TileMap.MapWidth * TileMap.TileWidth) / 2) - Camera.ViewPortWidth / 2, ((TileMap.MapHeight * TileMap.TileHeight) / 2) - Camera.ViewPortHeight / 2);

            cameraGoal = Camera.Position;
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            optionTimer--;

            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && optionTimer <= 0)
            {
                switch(currentChoice)
                {
                    case 0:
                        TwinztickShooter.SwitchGamestate(1);
                        break;
                    case 1:
                        TwinztickShooter.ExitGame();
                        break;
                }
            }

            if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y <= -0.3f || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed) && optionTimer <= 0)
            {
                currentChoice++;
                optionTimer = 20;
            }
            if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y >= 0.3f || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed) && optionTimer <= 0)
            {
                currentChoice--;
                optionTimer = 20;
            }

            if (currentChoice > options.Length - 1) currentChoice = 0;
            else if (currentChoice < 0) currentChoice = options.Length - 1;

            MoveCamera();
        }

        public void Draw(SpriteBatch sb)
        {
            Color colour;
            sb.Begin(samplerState: SamplerState.PointClamp);
            TileMap.Draw(sb);
            sb.Draw(logo, new Vector2(((screenWidth / 2) - ((logo.Width * 10) / 2)), ((screenHeight / 5) - ((logo.Height * 10) / 2))), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 1f);
            #region Draw Options
            for (int i = 0; i < options.Length; i++)
            {
                if (i == currentChoice)
                {
                    colour = Color.DodgerBlue;
                } else
                {
                    colour = Color.DarkRed;
                }
                sb.DrawString(font, options[i], new Vector2(screenWidth / 2 - font.MeasureString(options[i]).X * 7 / 2, (screenHeight / 8 * 4) + i * 150), colour, 0f, Vector2.Zero, 7f, SpriteEffects.None, 1f);
            }
            #endregion
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

            if(cameraGoal.X < Camera.Position.X)
            {
                Camera.Move(new Vector2(-2, 0));
            } else if(cameraGoal.X > Camera.Position.X)
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
