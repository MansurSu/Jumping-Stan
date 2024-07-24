using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TheGameGame.interfaces;
using TheGameGame.Animation;
using TheGameGame.Input;
using System.Diagnostics;

namespace TheGameGame
{
    public class Hero : IGameObject
    {
        Texture2D heroTexture;
        Animatie runAnimatie;
        Animatie idleAnimatie;
        Animatie jumpAnimatie;
        Animatie currentAnimatie;
        private Vector2 positie;
        private Vector2 snelheid;
        private float gravity = 0.5f;
        private float jumpStrength = -10f;
        private bool isJumping = false;
        private bool isOnGround = true;
        private bool facingRight = true; // Boolean to track the direction the hero is facing
        IInputReader inputReader;
        private int heroSize = 90;

        public Hero(Texture2D texture, IInputReader reader, Vector2 initialPosition)
        {
            heroTexture = texture;
            jumpAnimatie = new Animatie();
            jumpAnimatie.AddFrame(new AnimationFrame(new Rectangle(900, 0, 300, 300)));
            idleAnimatie = new Animatie();
            idleAnimatie.AddFrame(new AnimationFrame(new Rectangle(0, 0, 300, 300)));
            runAnimatie = new Animatie();
            runAnimatie.AddFrame(new AnimationFrame(new Rectangle(0, 300, 300, 300)));
            runAnimatie.AddFrame(new AnimationFrame(new Rectangle(300, 300, 300, 300)));
            runAnimatie.AddFrame(new AnimationFrame(new Rectangle(600, 300, 300, 300)));
            positie = initialPosition;
            snelheid = new Vector2(6, 0); // Horizontal speed
            this.inputReader = reader;
        }

        public void Update(GameTime gameTime)
        {
            var direction = inputReader.ReadInput();

            // Update facing direction
            if (direction.X < 0)
            {
                facingRight = false;
            }
            else if (direction.X > 0)
            {
                facingRight = true;
            }

            // Horizontal movement
            positie.X += direction.X * snelheid.X;

            // Apply gravity only if not on the ground
            if (!isOnGround)
            {
                snelheid.Y += gravity;
            }

            // Jumping logic
            if (direction.Y == -1 && isOnGround)
            {
                snelheid.Y = jumpStrength;
                isJumping = true;
                isOnGround = false;
            }

            // Update position with vertical speed
            positie.Y += snelheid.Y;
            if (positie.Y < 0)
            {
                positie.Y = 0;
            }

            if (isOnGround && snelheid.Y >= 0)
            {
                positie.Y = (float)System.Math.Floor(positie.Y / 60) * 60 + 15;
                snelheid.Y = 0;
                isJumping = false;
            }

            // Check if hero has landed
            if (positie.Y >= 480) // Assuming 600 is the ground level
            {
                positie.Y = 479;
                snelheid.Y = 0;
                isOnGround = true;
                isJumping = false;
            }

            // Update animation
            if (direction == Vector2.Zero)
            {
                currentAnimatie = idleAnimatie;
            }
            else if (isJumping)
            {
                currentAnimatie = jumpAnimatie;
            }
            else
            {
                currentAnimatie = runAnimatie;
            }
            currentAnimatie.Update(gameTime);

            // Check horizontal boundaries and keep the hero within bounds
            if (positie.X > 799 - 20) positie.X = 799 - 20;
            if (positie.X < 0) positie.X = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Determine the sprite effects based on the facing direction
            SpriteEffects spriteEffects = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Specify the scale factor
            float scale = 0.3f;

            // Calculate the origin to maintain the position
            Vector2 origin = new Vector2(currentAnimatie.CurrentFrame.SourceRectangle.Width / 2, currentAnimatie.CurrentFrame.SourceRectangle.Height / 2);

            spriteBatch.Draw(heroTexture, positie, currentAnimatie.CurrentFrame.SourceRectangle, Color.White, 0, origin, scale, spriteEffects, 0);
        }

        public Vector2 GetPositie()
        {
            return positie;
        }

        public void UpdateIsOnGround(bool value)
        {
            isOnGround = value;
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle(
                (int)positie.X,
                (int)positie.Y,
                (int)(currentAnimatie.CurrentFrame.SourceRectangle.Width * 0.3f),
                (int)(currentAnimatie.CurrentFrame.SourceRectangle.Height * 0.3f));
        }
    }
}
