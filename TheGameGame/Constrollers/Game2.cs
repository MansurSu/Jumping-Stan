using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TheGameGame.entities;
using TheGameGame.Entities;
using TheGameGame.Input;
using static TheGameGame.Constrollers.Level;

namespace TheGameGame.Constrollers
{
    public class Game2 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D heroTexture;
        private SpriteFont scoreFont;
        private StartScreen menu;
        private WinScreen gameOverScreen;
        private GameState gameState;
        private Texture2D backgroundTexture;
        private Hero hero;
        private Level level;
        private Song backgroundMusic;

        public Game2()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
        }

        protected override void Initialize()
        {
            level = new(Content);
            gameState = GameState.MenuScreen;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("BG");
            heroTexture = Content.Load<Texture2D>("SpriteSheet");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");
            backgroundMusic = Content.Load<Song>("BackGroundMusic");
            LoadMenu();
        }

        private void LoadMenu()
        {
            MediaPlayer.Play(backgroundMusic);  // Starts playing the song.
            MediaPlayer.IsRepeating = true;     // Ensures that the song loops automatically after it ends, so there’s no gap between the end of the song and the restart.
            MediaPlayer.Volume = 0.5f;          // Controls the volume of the background music.Adjust this as needed
            const int buttonWidth = 200;
            const int buttonHeight = 200;
            int buttonX = (GraphicsDevice.Viewport.Width - buttonWidth) / 2;
            int buttonY = (GraphicsDevice.Viewport.Height - buttonHeight) / 2;
            Texture2D startButtonTexture = Content.Load<Texture2D>("StartButton");
            Rectangle startButtonRectangle = new(buttonX, buttonY, buttonWidth, buttonHeight);
            menu = new StartScreen(startButtonTexture, startButtonRectangle);
            LoadVictoryScreen();
        }

        private void LoadVictoryScreen()
        {
            const int buttonWidth = 200;
            const int buttonHeight = 200;
            int buttonX = (GraphicsDevice.Viewport.Width - buttonWidth) / 2;
            int buttonY = (GraphicsDevice.Viewport.Height - buttonHeight) / 2;
            Texture2D startButtonTexture = Content.Load<Texture2D>("StartButton");
            Texture2D gameOverTexture = Content.Load<Texture2D>("GameOver");
            Texture2D winTexture = Content.Load<Texture2D>("WinScreen");
            Rectangle startButtonRectangle = new(buttonX, buttonY + 75, buttonWidth, buttonHeight);
            Vector2 victoryTextRectangle = new(buttonX, buttonY - 75);
            gameOverScreen = new WinScreen(startButtonTexture, startButtonRectangle, scoreFont, gameOverTexture, winTexture);
        }

        private void InitializeGameObject()
        {
            const float initialX = 50;
            float initialY = _graphics.PreferredBackBufferHeight - 90;
            Texture2D debug = Content.Load<Texture2D>("black");
            hero = new Hero(heroTexture, new KeyBoardreader(), new Vector2(initialX, initialY), debug);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (gameState == GameState.MenuScreen)
            {
                MouseState mouseState = Mouse.GetState();
                menu.Update(gameTime, mouseState);
            }
            else if (gameState == GameState.Playing)
            {
                if (hero == null)
                {
                    InitializeGameObject();
                    level.LoadCurrentLevel();
                }
                State state = level.Update(gameTime, hero.GetBoundingBox());
                Tile[,] gameboard = level.Gameboard();
                hero.CheckCollisions(gameboard);
                hero.Update(gameTime);


                if (level.CheckEnemyCollision(hero.GetBoundingBox()))
                {
                    gameOverScreen.Died();
                    gameState = GameState.GameOver;
                }
                else if (state == State.FinishedGame)
                {
                    gameState = GameState.GameOver;
                }
                else if (state == State.FinishedLevel)
                {
                    InitializeGameObject();
                }

                base.Update(gameTime);
            }
            else if (gameState == GameState.GameOver)
            {
                MouseState mouseState = Mouse.GetState();
                gameOverScreen.Update(gameTime, mouseState);
            }
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTexture, GraphicsDevice.Viewport.Bounds, Color.White);

            if (gameState == GameState.MenuScreen)
            {
                // Teken de achtergrondafbeelding op het hele scherm
                menu.Draw(_spriteBatch);
                IsMouseVisible = true;
                if (menu.IsStartButtonClicked)
                {
                    gameState = GameState.Playing;
                }
            }
            else if (gameState == GameState.Playing)
            {
                IsMouseVisible = false;
                level.Draw(_spriteBatch, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                _spriteBatch.DrawString(scoreFont, "Score: " + level.Score(), new Vector2(_graphics.PreferredBackBufferWidth - 150, 10), Color.Black);
                hero.Draw(_spriteBatch);
            }
            else
            {
                gameOverScreen.Draw(_spriteBatch);
                _spriteBatch.DrawString(scoreFont, "Score: " + level.Score(), new Vector2(_graphics.PreferredBackBufferWidth - 150, 10), Color.Black);
                IsMouseVisible = true;
                if (gameOverScreen.IsStartButtonClicked)
                {
                    level = new(Content);
                    hero = null;
                    gameOverScreen.ResetStartButton();
                    gameState = GameState.Playing;
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
