using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using TheGameGame.Input;
using static TheGameGame.Level;

namespace TheGameGame
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
            LoadMenu();
        }

        private void LoadMenu()
        {
            const int buttonWidth = 200;
            const int buttonHeight = 200;
            int buttonX = (GraphicsDevice.Viewport.Width - buttonWidth) / 2;
            int buttonY = (GraphicsDevice.Viewport.Height - buttonHeight) / 2;
            Texture2D startButtonTexture = Content.Load<Texture2D>("StartButton");
            Rectangle startButtonRectangle = new (buttonX, buttonY, buttonWidth, buttonHeight);
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
            Rectangle startButtonRectangle = new (buttonX, buttonY+75, buttonWidth, buttonHeight);
            Vector2 victoryTextRectangle = new (buttonX, buttonY-75);
            gameOverScreen = new WinScreen(startButtonTexture, startButtonRectangle, victoryTextRectangle, scoreFont);
        }

        private void InitializeGameObject()
        {
            const float initialX = 21;
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
            else if(gameState == GameState.Playing)
            {
                if(hero == null)
                {
                    InitializeGameObject();
                    level.LoadCurrentLevel();
                }
                State state = level.Update(gameTime, hero.GetBoundingBox());
                Vector2 position = hero.GetPositie();
                int x = (int)Math.Floor(position.X / 100);
                int y = (int)Math.Floor(position.Y / 60);
                Tile[,] gameboard = level.Gameboard();
                bool isOnGround = gameboard[y + 1, x]?.TileType == TileType.Impassable || gameboard[y + 1, x]?.TileType == TileType.Platform;

                hero.UpdateIsOnGround(isOnGround);
                hero.Update(gameTime);


                if (level.CheckEnemyCollision(hero.GetBoundingBox()))
                {
                    gameOverScreen.Died();
                    gameState = GameState.GameOver;
                } else if (state == State.FinishedGame)
                {
                    gameState = GameState.GameOver;
                }
                else if(state == State.FinishedLevel)
                {
                    InitializeGameObject();
                }

                base.Update(gameTime);
            }
            else if(gameState == GameState.GameOver)
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
                this.IsMouseVisible = true;
                if (menu.IsStartButtonClicked)
                {
                    gameState = GameState.Playing;
                }
            }
            else if(gameState == GameState.Playing)
            {
                this.IsMouseVisible = false;
                level.Draw(_spriteBatch, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                _spriteBatch.DrawString(scoreFont, "Score: " + level.Score(), new Vector2(_graphics.PreferredBackBufferWidth - 150, 10), Color.Black);
                hero.Draw(_spriteBatch);
            }
            else
            {
                gameOverScreen.Draw(_spriteBatch);
                _spriteBatch.DrawString(scoreFont, "Score: " + level.Score(), new Vector2(_graphics.PreferredBackBufferWidth - 150, 10), Color.Black);
                this.IsMouseVisible = true;
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
