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
        private Texture2D tilesTexture; // Declare Texture2D for tiles
        private Texture2D coinTexture; // Declare Texture2D for coins
        private Texture2D flagTexture;  // Declare Texture2D for flag
        private SpriteFont scoreFont; // Declare SpriteFont for score display

        private int tileWidth = 32; // Width of each tile in pixels
        private int tileHeight = 32; // Height of each tile in pixels
        private int tilemapWidthInTiles = 8; // Number of tiles in the gameboard width
        private int tilemapHeightInTiles = 8; // Number of tiles in the gameboard height
        private Texture2D backgroundTexture;

        private Tile[,] gameboard;
        private List<Coin> coins; // List to hold coins
        private int score; // Variable to hold the score
        private Vector2 flagPosition;
        private float flagScale = 2.2f; // Scale for the flag

        Hero hero;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set the resolution of the game window
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
        }

        protected override void Initialize()
        {
            gameboard = new Tile[tilemapHeightInTiles, tilemapWidthInTiles];
            coins = new List<Coin>(); // Initialize the coin list
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("SpriteSheet"); // Load your existing texture
            tilesTexture = Content.Load<Texture2D>("Tilemap");
            coinTexture = Content.Load<Texture2D>("coin");
            backgroundTexture = Content.Load<Texture2D>("BG");
            flagTexture = Content.Load<Texture2D>("Flag"); // Load the flag texture
            scoreFont = Content.Load<SpriteFont>("ScoreFont"); // Load the score font

            InitializeGameObject();
            InitializeGameboard();
            InitializeCoins();
        }

        private void InitializeGameObject()
        {
            float initialX = 21; // Leftmost position
            float initialY = _graphics.PreferredBackBufferHeight - 90; // Adjust for hero height, bottom-most position
            hero = new Hero(texture, new KeyBoardreader(), new Vector2(initialX, initialY));
        }

        private void InitializeGameboard()
        {
            Rectangle tile1Rect = new Rectangle(10, 0, 75, 64); // Source rectangle for tile 1
            Rectangle tile2Rect = new Rectangle(96, 96, 32, 32); // Source rectangle for tile 2
            Rectangle tile3Rect = new Rectangle(0, 0, 96, 64); // Source rectangle for tile 3
            Rectangle tile4Rect = new Rectangle(10, 32, 74, 55); // Source rectangle for tile 4

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

            // Set the flag position on top of the leftmost tile 2
            int flagTileX = 1; // X position of the tile
            int flagTileY = 4; // Y position of the tile
            flagPosition = new Vector2(flagTileX * tileWidth, (flagTileY * tileHeight) - flagTexture.Height / 2); // Adjust the flag position to sit on top of the tile
        }

        private void InitializeCoins()
        {
            float coinScale = 0.12f; // Set the scale to make the coins smaller
            double coinFrameTime = 0.1; // Set the frame time to slow down the animation

            coins.Add(new Coin(coinTexture, new Vector2(150, 300), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(500, 300), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(750, 250), coinScale, coinFrameTime));
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
                if (coins[i].IsCollected(hero))
                {
                    coins.RemoveAt(i);
                    score += 10; // Increase score by 10 for each collected coin
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

            foreach (var coin in coins)
            {
                coin.Draw(_spriteBatch);
            }

            hero.Draw(_spriteBatch);

            _spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(_graphics.PreferredBackBufferWidth - 150, 10), Color.Black);

            // Draw the flag
            _spriteBatch.Draw(flagTexture, flagPosition, null, Color.White, 0, Vector2.Zero, flagScale, SpriteEffects.None, 0);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
