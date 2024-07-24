using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGameGame
{
    public class Flag
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle boundingBox;

        public Flag(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            this.boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public Rectangle GetBoundingBox()
        {
            return boundingBox;
        }
    }
}
