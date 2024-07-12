using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TheGameGame.Input;

namespace TheGameGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D texture;
        private Texture2D tilesTexture;
        private Texture2D coinTexture;
        private SpriteFont scoreFont; // Declare SpriteFont

        private int tileWidth = 32;
        private int tileHeight = 32;
        private int tilemapWidthInTiles = 8;
        private int tilemapHeightInTiles = 8;
        private Texture2D backgroundTexture;

        private Tile[,] gameboard;
        private List<Coin> coins;
        private int score;

        Hero hero;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
        }

        protected override void Initialize()
        {
            gameboard = new Tile[tilemapHeightInTiles, tilemapWidthInTiles];
            coins = new List<Coin>();
            score = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("SpriteSheet");
            tilesTexture = Content.Load<Texture2D>("Tilemap");
            coinTexture = Content.Load<Texture2D>("coin");
            scoreFont = Content.Load<SpriteFont>("ScoreFont"); // Load SpriteFont

            backgroundTexture = Content.Load<Texture2D>("BG");

            InitializeGameObject();
            InitializeGameboard();
            InitializeCoins();
        }

        private void InitializeGameObject()
        {
            float initialX = 21;
            float initialY = _graphics.PreferredBackBufferHeight - 90;
            hero = new Hero(texture, new KeyBoardreader(), new Vector2(initialX, initialY));
        }

        private void InitializeGameboard()
        {
            Rectangle tile1Rect = new Rectangle(10, 0, 75, 64);
            Rectangle tile2Rect = new Rectangle(96, 96, 32, 32);
            Rectangle tile3Rect = new Rectangle(0, 0, 96, 64);
            Rectangle tile4Rect = new Rectangle(10, 32, 74, 55);

            int[,] tileMap = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 2, 0, 2, 0, 2, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 2 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1 }
            };

            for (int y = 0; y < tilemapHeightInTiles; y++)
            {
                for (int x = 0; x < tilemapWidthInTiles; x++)
                {
                    int tileIndex = tileMap[y, x];
                    Tile tile;
                    switch (tileIndex)
                    {
                        case 1:
                            tile = new Tile(tilesTexture, TileType.Impassable, tile1Rect);
                            break;
                        case 2:
                            tile = new Tile(tilesTexture, TileType.Platform, tile2Rect);
                            break;
                        case 3:
                            tile = new Tile(tilesTexture, TileType.Impassable, tile3Rect);
                            break;
                        case 4:
                            tile = new Tile(tilesTexture, TileType.Impassable, tile4Rect);
                            break;
                        default:
                            tile = null;
                            break;
                    }
                    gameboard[y, x] = tile;
                }
            }
        }

        private void InitializeCoins()
        {
            coins.Add(new Coin(coinTexture, new Vector2(200, 400), 0.25f)); // Adjust the scale factor as needed
            coins.Add(new Coin(coinTexture, new Vector2(300, 300), 0.25f)); // Adjust the scale factor as needed
            coins.Add(new Coin(coinTexture, new Vector2(400, 200), 0.25f)); // Adjust the scale factor as needed
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Vector2 positie = hero.GetPositie();
            int x = (int)Math.Floor(positie.X / 100);
            int y = (int)Math.Floor(positie.Y / 60);
            bool isOnGround = gameboard[y + 1, x]?.TileType == TileType.Impassable || gameboard[y + 1, x]?.TileType == TileType.Platform;

            hero.UpdateIsOnGround(isOnGround);
            hero.Update(gameTime);

            for (int i = coins.Count - 1; i >= 0; i--)
            {
                coins[i].Update(gameTime);

                if (hero.GetBounds().Intersects(coins[i].GetBounds()))
                {
                    score += 10;
                    coins.RemoveAt(i);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

            int screenWidth = _graphics.PreferredBackBufferWidth;
            int screenHeight = _graphics.PreferredBackBufferHeight;
            int tileWidth = screenWidth / tilemapWidthInTiles;
            int tileHeight = screenHeight / tilemapHeightInTiles;

            for (int y = 0; y < tilemapHeightInTiles; y++)
            {
                for (int x = 0; x < tilemapWidthInTiles; x++)
                {
                    Tile tile = gameboard[y, x];
                    if (tile != null)
                    {
                        Rectangle destinationRectangle = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
                        _spriteBatch.Draw(tile.Texture, destinationRectangle, tile.SourceRectangle, Color.White);
                    }
                }
            }

            hero.Draw(_spriteBatch);

            foreach (var coin in coins)
            {
                coin.Draw(_spriteBatch);
            }

            _spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(screenWidth - 100, 10), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
