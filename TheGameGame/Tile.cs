using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGameGame
{
    public enum TileType
    {
        Impassable,
        Platform
    }

    public class Tile
    {
        public Texture2D Texture { get; set; }
        public TileType TileType { get; set; }
        public Rectangle SourceRectangle { get; set; }

        public Tile(Texture2D texture, TileType tileType, Rectangle sourceRectangle)
        {
            Texture = texture;
            TileType = tileType;
            SourceRectangle = sourceRectangle;
        }
    }
}
