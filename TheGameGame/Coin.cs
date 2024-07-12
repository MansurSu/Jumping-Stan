using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheGameGame
{
    public class Coin
    {
        private Texture2D texture;
        private List<Rectangle> frames;
        private int currentFrame;
        private double timeCounter;
        private double timePerFrame;
        private Vector2 position;
        private float scale;

        public Coin(Texture2D texture, Vector2 position, float scale = 0.25f) // Adjust the scale factor as needed
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;

            frames = new List<Rectangle>
            {
                new Rectangle(0, 0, 192, 171),
                new Rectangle(192, 0, 192, 171),
                new Rectangle(384, 0, 192, 171),
                new Rectangle(576, 0, 192, 171),
                new Rectangle(768, 0, 192, 171),
                new Rectangle(960, 0, 192, 171)
            };

            currentFrame = 0;
            timeCounter = 0;
            timePerFrame = 0.1; // Adjust the frame rate as needed
        }

        public void Update(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeCounter >= timePerFrame)
            {
                currentFrame++;
                if (currentFrame >= frames.Count)
                {
                    currentFrame = 0;
                }
                timeCounter -= timePerFrame;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, frames[currentFrame], Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)(32 * scale), (int)(32 * scale));
        }
    }
}
