using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using TwinztickShooter.Gamestates;

namespace TwinztickShooter
{
    public class TwinztickShooter : Game
    {
        #region Variables
        Menu menu = new Menu(1920, 1080);
        GamePlay game = new GamePlay();
        GameOver gameOver = new GameOver(1920, 1080);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public static int score = 0;
        public static int highscore = 0;
        public static int screenWidth = 1920;
        public static int screenHeight = 1080;

        private static bool quitGame = false;
        private static bool justChanged = true;

        enum gamestate {menu, gamePlay, gameOver};
        static gamestate currentGameState = gamestate.menu;
        #endregion

        #region Constructor
        public TwinztickShooter()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = false;
        }
        #endregion

        #region Initialization
        protected override void Initialize()
        {
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            graphics.ApplyChanges();
            base.Initialize();

            LoadHighscore();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent() {}
        #endregion

        #region Gamestate Management
        //Changes the update method based on the current gamestate
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (quitGame)
                Exit();
            
            switch(currentGameState)
            {
                case gamestate.menu:
                    if(justChanged)
                    {
                        menu.Init(Content);
                        justChanged = false;
                    }
                    menu.Update();
                    break;
                case gamestate.gamePlay:
                    if(justChanged)
                    {
                        game.Init(Content);
                        justChanged = false;
                    }
                    game.Update();
                    break;
                case gamestate.gameOver:
                    if (justChanged)
                    {
                        gameOver.Init(Content);
                        justChanged = false;
                    }
                    gameOver.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }
        
        //Changes the draw method based on the current gamestate
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch(currentGameState)
            {
                case gamestate.menu:
                    if (justChanged)
                    {
                        menu.Init(Content);
                        justChanged = false;
                    }
                    menu.Draw(spriteBatch);
                    break;
                case gamestate.gamePlay:
                    if (justChanged)
                    {
                        game.Init(Content);
                        justChanged = false;
                    }
                    game.Draw(spriteBatch);
                    break;
                case gamestate.gameOver:
                    if (justChanged)
                    {
                        game.Init(Content);
                        justChanged = false;
                    }
                    gameOver.Draw(spriteBatch);
                    break;
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Switch to a different gamestate.
        /// </summary>
        /// <param name="stateID">0 = Menu, 1 = Play Game, 2 = Game Over</param>
        public static void SwitchGamestate(int stateID)
        {
            justChanged = true;

            switch(stateID)
            {
                case 0:
                    if (justChanged) return;
                    currentGameState = gamestate.menu;
                    break;
                case 1:
                    if (justChanged) return;
                    currentGameState = gamestate.gamePlay;
                    break;
                case 2:
                    if (justChanged) return;
                    currentGameState = gamestate.gameOver;
                    break;
            }
        }
        #endregion

        #region Helper Methods
        private static void LoadHighscore()
        {
            if(File.Exists("score.txt"))
            {
                StreamReader inputFile = new StreamReader("score.txt");
                highscore = Convert.ToInt32(inputFile.ReadLine());
                inputFile.Close();
            } else
            {
                highscore = 0;
            }
        }

        public static void SaveHighScore()
        {
            StreamWriter outputFile = new StreamWriter("score.txt");
            outputFile.WriteLine(highscore);
            outputFile.Close();
        }
        #endregion

        #region Getters and Setters
        // Returns the screen width
        public static int GetScreenWidth()
        {
            return screenWidth;
        }

        // Returns the screen height
        public static int GetScreenHeight()
        {
            return screenHeight;
        }

        public static void ExitGame()
        {
            quitGame = true;
        }
        #endregion
    }
}
