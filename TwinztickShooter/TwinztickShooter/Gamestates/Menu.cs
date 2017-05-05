using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinztickShooter.Gamestates
{
    class Menu
    {
        #region Declarations
        private String[] options = new String[3] { "Play", "Settings", "Quit"};
        private int selection = 1;
        private int screenWidth;
        private int screenHeight;

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
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
            {
                switch(selection)
                {
                    case 1:
                        TwinztickShooter.SwitchGamestate(2);
                        break;
                    case 2:

                        break;
                    case 3:
                        TwinztickShooter.ExitGame();
                        break;
                }
            }

            if (selection > options.Length) selection = 1;
            else if (selection < 1) selection = options.Length;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(samplerState: SamplerState.PointClamp);
            sb.Draw(logo, new Vector2(((screenWidth / 2) - ((logo.Width * 10) / 2)), ((screenHeight / 5) - ((logo.Height * 10) / 2))), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 1f);
            sb.DrawString(font, "SUCK!PENIS", new Vector2(10, 500), Color.Wheat, 0f, Vector2.Zero, 27f, SpriteEffects.None, 1f);
            sb.End();
        }
        #endregion
    }
}
