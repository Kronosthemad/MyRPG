using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyRPG.Core;

namespace MyRPG.World
{
    public class Map
    {
        private readonly Tile[,] _tiles;
        private readonly int _tileSize = 64; // Each square is 64x64 pixels

        public Map(int width, int height, Texture2D grassTex, Texture2D wallTex)
        {
            _tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isBorder = (x == 0 || y == 0 || x == width - 1 || y == height - 1);
                    bool isWall = isBorder || IsWallPosition(x, y, width, height);
                    _tiles[x, y] = new Tile(isWall ? wallTex : grassTex, isWall);
                }
            }
        }

        private bool IsWallPosition(int x, int y, int width, int height)
        {
            // Create multiple rooms with walls
            // Room 1: Left room (middle section)
            if (x >= 4 && x <= 5 && y >= 3 && y <= 8) return true;
            
            // Room 2: Top middle room
            if (x >= 8 && x <= 11 && y >= 3 && y <= 4) return true;
            
            // Room 3: Right room vertical wall
            if (x >= 12 && x <= 13 && y >= 2 && y <= 7) return true;
            
            // Room 4: Bottom section
            if (x >= 6 && x <= 10 && y >= 7 && y <= 8) return true;
            
            // Additional walls for more interesting layout
            if (x >= 2 && x <= 3 && y >= 5 && y <= 6) return true;
            
            return false;
        }

        public void Draw()
        {
            for (int x = 0; x < _tiles.GetLength(0); x++)
            {
                for (int y = 0; y < _tiles.GetLength(1); y++)
                {
                    Vector2 pos = new Vector2(x * _tileSize, y * _tileSize);
                    Globals.SpriteBatch.Draw(_tiles[x, y].Texture, pos, Color.White);
                }
            }
        }

        public bool IsTileBlocked(Vector2 pixelPos)
        {
            // Convert pixel coordinates (e.g., 128, 64) to grid indices (e.g., 2, 1)
            int x = (int)(pixelPos.X / _tileSize);
            int y = (int)(pixelPos.Y / _tileSize);

            // 1. Boundary Check: Don't let the game crash if we look outside the array
            if (x < 0 || x >= _tiles.GetLength(0) || y < 0 || y >= _tiles.GetLength(1))
                return true;

            // 2. Return whether that specific tile is marked as blocked
            return _tiles[x, y].IsBlocked;
        }

        public bool IsRectangleBlocked(Vector2 position, int width, int height)
        {
            // Check all four corners of the player's bounding box
            Vector2[] corners = new Vector2[]
            {
                new Vector2(position.X, position.Y),                          // Top-left
                new Vector2(position.X + width - 1, position.Y),              // Top-right
                new Vector2(position.X, position.Y + height - 1),              // Bottom-left
                new Vector2(position.X + width - 1, position.Y + height - 1)  // Bottom-right
            };

            foreach (var corner in corners)
            {
                if (IsTileBlocked(corner))
                    return true;
            }

            return false;
        }
    }
}