﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TheGameGame.interfaces;
using TheGameGame.Animation;
using TheGameGame.Input;

namespace TheGameGame
{
    public class Hero : IGameObject
    {
        Texture2D heroTexture;
        Animatie runAnimatie;
        Animatie idleAnimatie;
        Animatie currentAnimatie;
        private Vector2 positie;
        IInputReader inputReader;

        public Hero(Texture2D texture, IInputReader reader)
        {
            heroTexture = texture;
            idleAnimatie = new Animatie();
            idleAnimatie.AddFrame(new AnimationFrame(new Rectangle(0, 0, 300, 300)));
            runAnimatie = new Animatie();
            runAnimatie.AddFrame(new AnimationFrame(new Rectangle(0, 300, 300, 300)));
            runAnimatie.AddFrame(new AnimationFrame(new Rectangle(300, 300, 300, 300)));
            runAnimatie.AddFrame(new AnimationFrame(new Rectangle(600, 300, 300, 300)));
            positie = new Vector2(10, 10);
            this.inputReader = reader;
        }

        public void Update(GameTime gameTime)
        {
            var direction = inputReader.ReadInput();
            direction *= 6;
            positie += direction;

            // Update animation
            runAnimatie.Update(gameTime);
            if(direction.ToString() == "<0. 0>")
            {
                currentAnimatie = idleAnimatie;
            }
            else
            {
                currentAnimatie = runAnimatie;
            }

            // Check boundaries and keep the hero within bounds
            if (positie.X > 600) positie.X = 600;
            if (positie.X < 0) positie.X = 0;
            if (positie.Y > 600) positie.Y = 600;
            if (positie.Y < 0) positie.Y = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(heroTexture, positie, currentAnimatie.CurrentFrame.SourceRectangle, Color.White, 0, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);
        }
    }
}