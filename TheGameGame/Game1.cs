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
        private int tilemapWidthInTiles = 8; // Number of tiles in the tilemap width
        private int tilemapHeightInTiles = 8; // Number of tiles in the tilemap height


        private int[,] gameboard = new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 1, 1, 0, 1, 1, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 2 },
            { 1, 0, 1, 1, 1, 1, 1, 2 },
            { 1, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        Hero hero;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("SpriteSheet"); // Load your existing texture

            // Load Tilemap.png as a Texture2D
            tilesTexture = Content.Load<Texture2D>("Tilemap");

            InitializeGameObject();
        }

        private void InitializeGameObject()
        {
            hero = new Hero(texture, new KeyBoardreader());
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            hero.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            _spriteBatch.Begin();

            // Draw the portion of the tilemap based on gameboard array
            for (int y = 0; y < tilemapHeightInTiles; y++)
            {
                for (int x = 0; x < tilemapWidthInTiles; x++)
                {
                    // Get the tile index from gameboard
                    int tileIndex = gameboard[y, x];

                    // Calculate source rectangle in your tilesTexture based on tileIndex
                    Rectangle sourceRectangle = new Rectangle(tileIndex * tileWidth, 0, tileWidth, tileHeight);

                    // Calculate destination rectangle
                    Rectangle destinationRectangle = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);

                    // Draw tile
                    _spriteBatch.Draw(tilesTexture, destinationRectangle, sourceRectangle, Color.White);
                }
            }

            // Draw hero using _spriteBatch
            hero.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
