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

            // Load Tiles.png as a Texture2D
            tilesTexture = Content.Load<Texture2D>("Tiles");

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
            // Draw hero using _spriteBatch
            hero.Draw(_spriteBatch);
            // Example: Draw a tile from tilesTexture at position (100, 100)
            Rectangle sourceRectangle = new Rectangle(100, 100, 132, 132); // Example source rectangle
            Rectangle destinationRectangle = new Rectangle(150, 150, 132, 132); // Example destination rectangle
            _spriteBatch.Draw(tilesTexture, destinationRectangle, sourceRectangle, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
