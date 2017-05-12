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
        private int currentChoice = 0;
        private String[] options = new String[3] {
            "PLAY",
            "SETTINGS",
            "QUIT"
        };
        
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
                switch(currentChoice)
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

            if (currentChoice > options.Length) currentChoice = 1;
            else if (currentChoice < 1) currentChoice = options.Length;
        }

        public void Draw(SpriteBatch sb)
        {
            Color colour;

            sb.Begin(samplerState: SamplerState.PointClamp);
            sb.Draw(logo, new Vector2(((screenWidth / 2) - ((logo.Width * 10) / 2)), ((screenHeight / 5) - ((logo.Height * 10) / 2))), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 1f);

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

            sb.End();
        }
        #endregion
    }
}
