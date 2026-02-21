using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyRPG.World
{
    public class Tile
    {
        public Texture2D Texture;
        public bool IsBlocked; // True if it's a wall

        public Tile(Texture2D texture, bool isBlocked)
        {
            Texture = texture;
            IsBlocked = isBlocked;
        }
    }
}