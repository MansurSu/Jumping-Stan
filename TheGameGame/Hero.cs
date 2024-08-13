// Updated Hero class with SetPosition method
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TheGameGame.interfaces;
using TheGameGame.Animation;
using TheGameGame.Input;
using System;
using System.Diagnostics;
namespace TheGameGame
{
    public class Hero : IGameObject
    {
        Texture2D heroTexture;
        Texture2D debug;
        Animatie runAnimatie;
        Animatie idleAnimatie;
        Animatie jumpAnimatie;
        Animatie currentAnimatie;
        private Vector2 positie;
        private Vector2 snelheid;
        private float acceleratie = 0.2f;
        private float maximumSnelheid = 6;
        private float gravity = 0.5f;
        private float jumpStrength = -10f;
        private bool isJumping = false;
        private bool isOnGround = true;
        private bool facingRight = true; // Boolean to track the direction the hero is facing
        IInputReader inputReader;
        private const float scale = 0.3f;

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
            currentAnimatie = idleAnimatie;
        }
        public Hero(Texture2D texture, IInputReader reader, Vector2 initialPosition, Texture2D debug)
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
            snelheid = new Vector2(0, 0); // Horizontal speed
            this.inputReader = reader;
            currentAnimatie = idleAnimatie;
            this.debug = debug;
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

            if (direction.X != 0)
            {
                snelheid.X += direction.X * acceleratie;
            }
            else
            {
                if (Math.Abs(snelheid.X)< acceleratie)
                {
                    snelheid.X = 0;
                }
                else if (snelheid.X > 0)
                {
                    snelheid.X -= acceleratie;
                }
                else if(snelheid.X < 0)
                {
                    snelheid.X += acceleratie;
                }
            }

            if(snelheid.X > maximumSnelheid)
            {
                snelheid.X = maximumSnelheid;
            }
            else if(snelheid.X < -maximumSnelheid)
            {
                snelheid.X = -maximumSnelheid;
            }
            // Horizontal movement
            positie.X += snelheid.X;
            Debug.WriteLine(snelheid.X);

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
            if (positie.Y >= 480) // Assuming 480 is the ground level
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
            

            // Calculate the origin to maintain the position
            Vector2 origin = new Vector2(currentAnimatie.CurrentFrame.SourceRectangle.Width / 2, currentAnimatie.CurrentFrame.SourceRectangle.Height / 2);

            spriteBatch.Draw(heroTexture, positie, currentAnimatie.CurrentFrame.SourceRectangle, Color.White, 0, origin, scale, spriteEffects, 0);
            
            // debug boundingbox
            // spriteBatch.Draw(debug, GetBoundingBox(), Color.White);
        }

        public Vector2 GetPositie()
        {
            return positie;
        }

        public void UpdateIsOnGround(bool value)
        {
            isOnGround = value;
        }

        // public void UpdateSuroundingTiles()
        // {
        // 
        // }

        public Rectangle GetBoundingBox()
        {
            int halfWidth = (int)(currentAnimatie.CurrentFrame.SourceRectangle.Width / 4 * scale);
            int halfHeight = (int)(currentAnimatie.CurrentFrame.SourceRectangle.Height / 2 * scale);
            return new Rectangle((int)positie.X - halfWidth, (int)positie.Y - halfHeight, halfWidth * 2, halfHeight * 2);
        }

        public void SetPosition(Vector2 newPosition)
        {
            positie = newPosition;
        }
    }
}