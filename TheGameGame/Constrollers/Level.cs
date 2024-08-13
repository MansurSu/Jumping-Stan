using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Windows.Forms;
using TheGameGame.entities;
using TheGameGame.Entities;


namespace TheGameGame.Constrollers
{
    public class Level
    {
        private Tile[,] gameboard;
        private List<Coin> coins;
        private Vector2 flagPosition;
        private const int tileWidth = 100;
        private const int tileHeight = 60;
        private const int tilemapWidthInTiles = 8;
        private const int tilemapHeightInTiles = 8;
        private int currentLevel;
        private readonly ContentManager content;
        private Texture2D flagTexture;
        private Texture2D tilesTexture;
        private Texture2D coinTexture;
        private const float coinScale = 0.12f;
        private const double coinFrameTime = 0.1;
        private int score;
        private Enemy enemy;
        private const float flagScale = 2.2f;

        public Tile[,] Gameboard() => gameboard;

        public int Score() => score;

        public Level(ContentManager content)
        {
            this.content = content;
            LoadContent();
            currentLevel = 1;
        }

        private void LoadContent()
        {
            flagTexture = content.Load<Texture2D>("Flag");
            tilesTexture = content.Load<Texture2D>("Tilemap");
            coinTexture = content.Load<Texture2D>("coin");
        }

        public void LoadCurrentLevel()
        {
            switch (currentLevel)
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
            }
        }
        private void LoadLevel1()
        {
            coins = new();
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

            const int flagTileX = 0;
            const int flagTileY = 1;
            flagPosition = new Vector2(flagTileX * tileWidth, flagTileY * tileHeight - flagTexture.Height / 2);
            coins.Add(new Coin(coinTexture, new Vector2(150, 300), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(250, 380), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(300, 380), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(350, 380), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(400, 380), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(500, 300), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(750, 250), coinScale, coinFrameTime));
            Texture2D zombieTexture = content.Load<Texture2D>("enemy1"); // Load the zombie sprite sheet


            // Assuming the sprite sheet is 2618x1157 with 3 images (adjust accordingly)
            Rectangle[] zombieFrames = new Rectangle[]
            {
                 new (0, 0, 872, 1157), // Adjust these rectangles based on actual sprite positions
                 new (872, 0, 872, 1157),
                 new (1744, 0, 872, 1157)
            };

            // Initialize the zombie with a scale factor
            Vector2 zombieStartPosition = new(180, 327); // Adjust as needed
            Vector2 zombieSpeed = new(1.0f, 0); // Adjust speed as needed
            const float zombieScale = 0.08f; // Adjust scale to make the zombie smaller
            enemy = new(zombieTexture, zombieFrames, zombieStartPosition, zombieSpeed, zombieScale);
        }

        private void LoadLevel2()
        {
            coins = new();
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

            const int flagTileX = 0;
            const int flagTileY = 1;
            flagPosition = new Vector2(flagTileX * tileWidth, flagTileY * tileHeight - flagTexture.Height / 2);
            coins.Add(new Coin(coinTexture, new Vector2(100, 250), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(200, 250), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(300, 250), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(320, 380), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(370, 380), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(420, 380), coinScale, coinFrameTime));
            Texture2D pterodactylTexture = content.Load<Texture2D>("enemy2"); // Load the zombie sprite sheet


            // Assuming the sprite sheet is 2618x1157 with 3 images (adjust accordingly)
            Rectangle[] pterodactylFrames = new Rectangle[]
            {
                 new (0, 0, 1414, 1482), // Adjust these rectangles based on actual sprite positions
                 new (1414, 0, 1414, 1482),
            };

            // Initialize the zombie with a scale factor
            Vector2 pterodactylStartPosition = new(350, 304); // Adjust as needed
            Vector2 pterodactylSpeed = new(0, 1.0f); // Adjust speed as needed
            const float pterodactylScale = 0.05f; // Adjust scale to make the zombie smaller
            enemy = new(pterodactylTexture, pterodactylFrames, pterodactylStartPosition, pterodactylSpeed, pterodactylScale);
        }

        private void LoadLevel3()
        {
            coins = new();
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

            const int flagTileX = 0;
            const int flagTileY = 1;
            flagPosition = new Vector2(flagTileX * tileWidth, flagTileY * tileHeight - flagTexture.Height / 2);
            coins.Add(new Coin(coinTexture, new Vector2(50, 100), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(300, 150), coinScale, coinFrameTime));
            coins.Add(new Coin(coinTexture, new Vector2(700, 200), coinScale, coinFrameTime));
            Texture2D beetleTexture = content.Load<Texture2D>("enemy3"); // Load the zombie sprite sheet


            // Assuming the sprite sheet is 2618x1157 with 3 images (adjust accordingly)
            Rectangle[] beetleFrames = new Rectangle[]
            {
                 new (0, 0, 1171, 725), // Adjust these rectangles based on actual sprite positions
                 new (1171, 0, 1171, 725),
                 new (2342, 0, 1171, 725)
            };

            // Initialize the zombie with a scale factor
            Vector2 beetleStartPosition = new(400, 383); // Adjust as needed
            Vector2 beetleSpeed = new(4.0f, 0); // Adjust speed as needed
            const float beetleScale = 0.05f; // Adjust scale to make the zombie smaller
            enemy = new(beetleTexture, beetleFrames, beetleStartPosition, beetleSpeed, beetleScale);
        }
        private void SetTileMap(int[,] tileMap)
        {
            gameboard = new Tile[8, 8];
            for (int y = 0; y < tilemapHeightInTiles; y++)
            {
                for (int x = 0; x < tilemapWidthInTiles; x++)
                {
                    int tileIndex = tileMap[y, x];
                    Tile tile = tileIndex switch
                    {
                        1 => new Tile(tilesTexture, TileType.Impassable, new Rectangle(10, 0, 75, 64)),
                        2 => new Tile(tilesTexture, TileType.Platform, new Rectangle(96, 96, 32, 32)),
                        3 => new Tile(tilesTexture, TileType.Impassable, new Rectangle(0, 0, 96, 64)),
                        4 => new Tile(tilesTexture, TileType.Impassable, new Rectangle(10, 32, 74, 55)),
                        _ => null,
                    };
                    gameboard[y, x] = tile;
                }
            }
        }

        public State Update(GameTime gameTime, Rectangle heroHitbox)
        {
            for (int i = coins.Count - 1; i >= 0; i--)
            {
                coins[i].Update(gameTime);
                if (coins[i].IsCollected(heroHitbox))
                {
                    coins.RemoveAt(i);
                    score += 10;
                }
            }
            Rectangle flagHitbox = new Rectangle((int)flagPosition.X, (int)flagPosition.Y, (int)(flagTexture.Width * flagScale), (int)(flagTexture.Height * flagScale));
            if (heroHitbox.Intersects(flagHitbox))
            {
                currentLevel++;
                if (currentLevel > 3)
                {
                    // if won
                    return State.FinishedGame;
                }
                else
                {
                    // if won finished level
                    LoadCurrentLevel();
                    return State.FinishedLevel;
                }
            }
            enemy?.Update(gameTime, gameboard);
            // if not won
            return State.Default;
        }

        public bool CheckEnemyCollision(Rectangle heroPosition)
        {
            if (enemy != null)
            {
                return enemy.GetBoundingBox().Intersects(heroPosition);
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            int tileWidth = screenWidth / tilemapWidthInTiles;
            int tileHeight = screenHeight / tilemapHeightInTiles;

            for (int y = 0; y < tilemapHeightInTiles; y++)
            {
                for (int x = 0; x < tilemapWidthInTiles; x++)
                {
                    Tile tile = gameboard[y, x];
                    if (tile?.Texture != null)
                    {
                        Rectangle destinationRectangle = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
                        spriteBatch.Draw(tile.Texture, destinationRectangle, tile.SourceRectangle, Color.White);
                    }
                }
            }

            foreach (var coin in coins)
            {
                coin.Draw(spriteBatch);
            }

            spriteBatch.Draw(flagTexture, flagPosition, null, Color.White, 0, Vector2.Zero, flagScale, SpriteEffects.None, 0);

            // Draw the zombie
            enemy?.Draw(spriteBatch);

            // debug boundingbox
            // Texture2D black = content.Load<Texture2D>("black");
            // spriteBatch.Draw(black, enemy.GetBoundingBox(), Color.White);
        }
        public enum State
        {
            Default,
            FinishedLevel,
            FinishedGame
        }
    }
}
