using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinztickShooter.Gamestates;

namespace TwinztickShooter
{
    public class Game1 : Game
    {
        Menu menu = new Menu();
        GamePlay game = new GamePlay();
        GameOver gameOver = new GameOver();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int screenWidth = 1920;
        public static int screenHeight = 1080;

        enum gamestate {menu, gamePlay, gameOver};
        static gamestate currentGameState = gamestate.gamePlay;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = false;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            game.Init(Content);
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void UnloadContent() {}
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
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
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepPink);

            switch(currentGameState)
            {
                case gamestate.menu:
                    menu.Draw();
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
        /// <param name="stateID">1 = Menu, 2 = Game Play, 3 = Game Over</param>
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

        public static int GetScreenWidth()
        {
            return screenWidth;
        }

        public static int GetScreenHeight()
        {
            return screenHeight;
        }

        public void LoadImage(string fileName)
        {
            Content.Load < Texture2D >(fileName);
        }
    }
}
