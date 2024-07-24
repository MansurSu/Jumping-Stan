﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheGameGame
{
    public class Coin
    {
        private Texture2D texture;
        private Vector2 position;
        private float scale;
        private List<Rectangle> frames;
        private int currentFrame;
        private double timeSinceLastFrame;
        private double frameTime; // Time per frame in seconds

        public Coin(Texture2D texture, Vector2 position, float scale, double frameTime)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;
            this.frameTime = frameTime;
            this.frames = new List<Rectangle>
            {
                new Rectangle(0, 0, 171, 171),
                new Rectangle(171, 0, 171, 171),
                new Rectangle(342, 0, 171, 171),
                new Rectangle(513, 0, 171, 171),
                new Rectangle(684, 0, 171, 171),
                new Rectangle(855, 0, 171, 171)
            };
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFrame >= frameTime)
            {
                currentFrame = (currentFrame + 1) % frames.Count;
                timeSinceLastFrame -= frameTime;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, frames[currentFrame], Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)(frames[currentFrame].Width * scale),
                (int)(frames[currentFrame].Height * scale));
        }

        public bool IsCollected(Hero hero)
        {
            return GetBoundingBox().Intersects(hero.GetBoundingBox());
        }
    }
}
