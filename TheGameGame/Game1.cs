using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TheGameGame.Input;

namespace TheGameGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D texture;
        private Texture2D tilesTexture; // Declare Texture2D for tiles

        private int tileWidth = 32; // Width of each tile in pixels
        private int tileHeight = 32; // Height of each tile in pixels
        private int tilemapWidthInTiles = 8; // Number of tiles in the gameboard width
        private int tilemapHeightInTiles = 8; // Number of tiles in the gameboard height
        private Texture2D backgroundTexture;

        private Tile[,] gameboard;

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("SpriteSheet"); // Load your existing texture

            // Load Tilemap.png as a Texture2D
            tilesTexture = Content.Load<Texture2D>("Tilemap");

            backgroundTexture = Content.Load<Texture2D>("BG");

            InitializeGameObject();

            // Initialize the gameboard
            InitializeGameboard();
        }

        private void InitializeGameObject()
        {
            hero = new Hero(texture, new KeyBoardreader());
        }

        private void InitializeGameboard()
        {
            // Define the tile types and source rectangles for each tile
            Rectangle tile1Rect = new Rectangle(10, 0, 75, 64); // Source rectangle for tile 1
            Rectangle tile2Rect = new Rectangle(96, 96, 32, 32); // Source rectangle for tile 2
            Rectangle tile3Rect = new Rectangle(0, 0, 96, 64); // Source rectangle for tile 3
            Rectangle tile4Rect = new Rectangle(10, 32, 74, 55); // Source rectangle for tile 4

            // Map the gameboard
            int[,] tileMap = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 2, 2, 2, 2, 2, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 3 },
                { 0, 0, 3, 0, 3, 0, 0, 4 },
                { 1, 1, 4, 1, 4, 1, 1, 4 }
            };

            // Initialize tiles
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            _spriteBatch.Begin();

            // Draw the background
            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

            // Calculate tile dimensions to fill the screen
            int screenWidth = _graphics.PreferredBackBufferWidth;
            int screenHeight = _graphics.PreferredBackBufferHeight;
            int tileWidth = screenWidth / tilemapWidthInTiles;
            int tileHeight = screenHeight / tilemapHeightInTiles;

            // Draw the portion of the tilemap based on the gameboard array
            for (int y = 0; y < tilemapHeightInTiles; y++)
            {
                for (int x = 0; x < tilemapWidthInTiles; x++)
                {
                    Tile tile = gameboard[y, x];
                    if (tile != null)  // Check if the tile is not null
                    {
                        Rectangle destinationRectangle = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
                        // Draw tile
                        _spriteBatch.Draw(tile.Texture, destinationRectangle, tile.SourceRectangle, Color.White);
                    }
                }
            }

            // Draw hero using _spriteBatch
            hero.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
