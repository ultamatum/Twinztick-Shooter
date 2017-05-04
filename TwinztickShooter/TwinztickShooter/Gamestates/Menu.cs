using Microsoft.Xna.Framework;
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
        #endregion

        #region Constructor
        public Menu()
        {

        }
        #endregion

        #region Initialization
        public void Init()
        {

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

        public void Draw()
        {

        }
        #endregion
    }
}
