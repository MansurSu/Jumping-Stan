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
        private Texture2D flagTexture;
        private SpriteFont scoreFont;

        private int tileWidth = 32;
        private int tileHeight = 32;
        private int tilemapWidthInTiles = 8;
        private int tilemapHeightInTiles = 8;
        private Texture2D backgroundTexture;

        private Tile[,] gameboard;
        private List<Coin> coins;
        private int score;
        private Vector2 flagPosition;
        private float flagScale = 2.2f;

        private Hero hero;
        private int currentLevel;

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
            currentLevel = 1; // Start with level 1
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("SpriteSheet");
            tilesTexture = Content.Load<Texture2D>("Tilemap");
            coinTexture = Content.Load<Texture2D>("coin");
            backgroundTexture = Content.Load<Texture2D>("BG");
            flagTexture = Content.Load<Texture2D>("Flag");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");

            InitializeGameObject();
            LoadLevel(currentLevel);
        }

        private void InitializeGameObject()
        {
            float initialX = 21;
            float initialY = _graphics.PreferredBackBufferHeight - 90;
            hero = new Hero(texture, new KeyBoardreader(), new Vector2(initialX, initialY));
        }

        private void LoadLevel(int level)
        {
            switch (level)
            {
                case 1:
                    LoadLevel1();
                    break;
                case 2:
                    LoadLevel2();
                    break;
                case 3:
                    LoadLevel3();
                    break;
                default:
                    LoadLevel1();
                    break;
            }

            coins.Clear();
            InitializeCoins();

            hero.SetPosition(new Vector2(21, _graphics.PreferredBackBufferHeight - 90));
        }

        private void LoadLevel1()
        {
            Rectangle tile1Rect = new Rectangle(10, 0, 75, 64);
            Rectangle tile2Rect = new Rectangle(96, 96, 32, 32);
            Rectangle tile3Rect = new Rectangle(0, 0, 96, 64);
            Rectangle tile4Rect = new Rectangle(10, 32, 74, 55);

            int[,] tileMap = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 2, 0, 2, 0, 2, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 2 },
                { 0, 3, 0, 0, 0, 3, 0, 0 },
                { 1, 4, 1, 1, 1, 4, 1, 1 }
            };

            SetTileMap(tileMap);

            int flagTileX = 1;
            int flagTileY = 2;
            flagPosition = new Vector2(flagTileX * tileWidth, (flagTileY * tileHeight) - flagTexture.Height / 2);
        }

        private void LoadLevel2()
        {
            Rectangle tile1Rect = new Rectangle(10, 0, 75, 64);
            Rectangle tile2Rect = new Rectangle(96, 96, 32, 32);
            Rectangle tile3Rect = new Rectangle(0, 0, 96, 64);
            Rectangle tile4Rect = new Rectangle(10, 32, 74, 55);

            int[,] tileMap = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 2, 0, 0, 2, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 2 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 3, 0, 0, 0, 0, 3 },
                { 1, 1, 4, 1, 1, 1, 1, 4 }
            };

            SetTileMap(tileMap);

            int flagTileX = 1;
            int flagTileY = 2;
            flagPosition = new Vector2(flagTileX * tileWidth, (flagTileY * tileHeight) - flagTexture.Height / 2);
        }

        private void LoadLevel3()
        {
            Rectangle tile1Rect = new Rectangle(10, 0, 75, 64);
            Rectangle tile2Rect = new Rectangle(96, 96, 32, 32);
            Rectangle tile3Rect = new Rectangle(0, 0, 96, 64);
            Rectangle tile4Rect = new Rectangle(10, 32, 74, 55);

            int[,] tileMap = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 2, 2, 2, 2, 2, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 2 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 3, 0, 0, 0, 0, 3 },
                { 1, 1, 4, 1, 1, 1, 1, 4 }
            };

            SetTileMap(tileMap);

            int flagTileX = 1;
            int flagTileY = 2;
            flagPosition = new Vector2(flagTileX * tileWidth, (flagTileY * tileHeight) - flagTexture.Height / 2);
        }

        private void SetTileMap(int[,] tileMap)
        {
            for (int y = 0; y < tilemapHeightInTiles; y++)
            {
                for (int x = 0; x < tilemapWidthInTiles; x++)
                {
                    int tileIndex = tileMap[y, x];
                    Tile tile;
                    switch (tileIndex)
                    {
                        case 1:
                            tile = new Tile(tilesTexture, TileType.Impassable, new Rectangle(10, 0, 75, 64));
                            break;
                        case 2:
                            tile = new Tile(tilesTexture, TileType.Platform, new Rectangle(96, 96, 32, 32));
                            break;
                        case 3:
                            tile = new Tile(tilesTexture, TileType.Impassable, new Rectangle(0, 0, 96, 64));
                            break;
                        case 4:
                            tile = new Tile(tilesTexture, TileType.Impassable, new Rectangle(10, 32, 74, 55));
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
            float coinScale = 0.12f;
            double coinFrameTime = 0.1;

            if (currentLevel == 1)
            {
                coins.Add(new Coin(coinTexture, new Vector2(150, 300), coinScale, coinFrameTime));
                
                coins.Add(new Coin(coinTexture, new Vector2(250, 380), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(300, 380), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(350, 380), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(400, 380), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(500, 300), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(750, 250), coinScale, coinFrameTime));
            }
            else if (currentLevel == 2)
            {
                coins.Add(new Coin(coinTexture, new Vector2(100, 250), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(200, 250), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(300, 250), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(320, 380), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(370, 380), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(420, 380), coinScale, coinFrameTime));
            }
            else if (currentLevel == 3)
            {
                coins.Add(new Coin(coinTexture, new Vector2(50, 100), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(300, 150), coinScale, coinFrameTime));
                coins.Add(new Coin(coinTexture, new Vector2(700, 200), coinScale, coinFrameTime));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Vector2 position = hero.GetPositie();
            int x = (int)Math.Floor(position.X / 100);
            int y = (int)Math.Floor(position.Y / 60);
            bool isOnGround = gameboard[y + 1, x]?.TileType == TileType.Impassable || gameboard[y + 1, x]?.TileType == TileType.Platform;

            hero.UpdateIsOnGround(isOnGround);
            hero.Update(gameTime);

            for (int i = coins.Count - 1; i >= 0; i--)
            {
                coins[i].Update(gameTime);
                if (coins[i].IsCollected(hero))
                {
                    coins.RemoveAt(i);
                    score += 10;
                }
            }

            if (hero.GetBoundingBox().Intersects(new Rectangle((int)flagPosition.X, (int)flagPosition.Y, (int)(flagTexture.Width * flagScale), (int)(flagTexture.Height * flagScale))))
            {
                currentLevel++;
                if (currentLevel > 3)
                {
                    // If there are no more levels, reset to level 1 or show a win screen
                    currentLevel = 1;
                }
                LoadLevel(currentLevel);
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

            _spriteBatch.Draw(flagTexture, flagPosition, null, Color.White, 0, Vector2.Zero, flagScale, SpriteEffects.None, 0);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
