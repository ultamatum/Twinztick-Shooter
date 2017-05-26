﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinztickShooter.Gamestates;

namespace TwinztickShooter
{
    public class TwinztickShooter : Game
    {
        #region Variables
        Menu menu = new Menu(1920, 1080);
        GamePlay game = new GamePlay();
        GameOver gameOver = new GameOver();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int screenWidth = 1920;
        public static int screenHeight = 1080;

        private static bool quitGame = false;

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
            graphics.IsFullScreen = true;
        }
        #endregion

        #region Initialization
        protected override void Initialize()
        {
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            graphics.ApplyChanges();
            base.Initialize();
            menu.Init(Content);
            game.Init(Content);
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
                    menu.Update();
                    break;
                case gamestate.gamePlay:
                    game.Update();
                    break;
                case gamestate.gameOver:
                    gameOver.Update();
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
                    menu.Draw(spriteBatch);
                    break;
                case gamestate.gamePlay:
                    game.Draw(spriteBatch);
                    break;
                case gamestate.gameOver:
                    gameOver.Draw();
                    break;
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Switch to a different gamestate.
        /// </summary>
        /// <param name="stateID">1 = Menu, 2 = Play Game, 3 = Game Over</param>
        public static void SwitchGamestate(int stateID)
        {
            switch(stateID)
            {
                case 0:
                    currentGameState = gamestate.menu;
                    break;
                case 1:
                    currentGameState = gamestate.gamePlay;
                    break;
                case 2:
                    currentGameState = gamestate.gameOver;
                    break;
            }
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
