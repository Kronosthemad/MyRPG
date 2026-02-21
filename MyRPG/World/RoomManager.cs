using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyRPG.Core;
using System.Collections.Generic;

namespace MyRPG.World
{
    public class Room
    {
        private readonly Tile[,] _tiles;
        private readonly int _tileSize = 64;
        public readonly int Width;
        public readonly string Name;
        public int Height => _tiles.GetLength(1);

        public Texture2D DoorTexture { get; set; }
        public List<Door> Doors { get; }

        public Room(int width, int height, string name, Tile[,] tiles)
        {
            Width = width;
            Name = name;
            _tiles = tiles;
            Doors = new List<Door>();
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return null;
            return _tiles[x, y];
        }

        public bool IsTileBlocked(Vector2 pixelPos)
        {
            int x = (int)(pixelPos.X / _tileSize);
            int y = (int)(pixelPos.Y / _tileSize);

            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return true;

            return _tiles[x, y].IsBlocked;
        }

        public bool IsRectangleBlocked(Vector2 position, int width, int height)
        {
            foreach (var door in Doors)
            {
                if (door.Intersects(position, width, height))
                    return false;
            }

            Vector2[] corners = new Vector2[]
            {
                new Vector2(position.X, position.Y),
                new Vector2(position.X + width - 1, position.Y),
                new Vector2(position.X, position.Y + height - 1),
                new Vector2(position.X + width - 1, position.Y + height - 1)
            };

            foreach (var corner in corners)
            {
                if (IsTileBlocked(corner))
                    return true;
            }

            return false;
        }

        public void Draw()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < _tiles.GetLength(1); y++)
                {
                    Vector2 pos = new Vector2(x * _tileSize, y * _tileSize);
                    Globals.SpriteBatch.Draw(_tiles[x, y].Texture, pos, Color.White);
                }
            }

            foreach (var door in Doors)
            {
                Color doorColor = new Color(139, 69, 19);
                Globals.SpriteBatch.Draw(DoorTexture, door.Position, doorColor);
            }
        }
    }

    public class Door
    {
        public string TargetRoom { get; }
        public Vector2 Position { get; }
        public Vector2 SpawnPosition { get; }
        public int Width { get; }
        public int Height { get; }

        public Door(string targetRoom, Vector2 position, Vector2 spawnPosition, int width = 64, int height = 64)
        {
            TargetRoom = targetRoom;
            Position = position;
            SpawnPosition = spawnPosition;
            Width = width;
            Height = height;
        }

        public bool Intersects(Vector2 playerPos, int playerWidth, int playerHeight)
        {
            return playerPos.X < Position.X + Width &&
                   playerPos.X + playerWidth > Position.X &&
                   playerPos.Y < Position.Y + Height &&
                   playerPos.Y + playerHeight > Position.Y;
        }
    }

    public class RoomManager
    {
        private Dictionary<string, Room> _rooms;
        private Room _currentRoom;
        public Room CurrentRoom => _currentRoom;

        public RoomManager()
        {
            _rooms = new Dictionary<string, Room>();
        }

        public void AddRoom(string name, Room room)
        {
            _rooms[name] = room;
        }

        public void SetCurrentRoom(string name)
        {
            if (_rooms.ContainsKey(name))
                _currentRoom = _rooms[name];
        }

        public string GetTargetRoom(Vector2 playerPos, int playerWidth, int playerHeight)
        {
            foreach (var door in _currentRoom.Doors)
            {
                if (door.Intersects(playerPos, playerWidth, playerHeight))
                {
                    return door.TargetRoom;
                }
            }
            return null;
        }

        public Vector2 GetSpawnPosition(string targetRoom, string fromRoom)
        {
            if (!_rooms.ContainsKey(targetRoom)) return new Vector2(128, 128);
            
            foreach (var door in _rooms[targetRoom].Doors)
            {
                if (door.TargetRoom == fromRoom)
                {
                    return door.SpawnPosition;
                }
            }
            
            return new Vector2(128, 128);
        }

        public Door GetDoorForRoom(string roomName, string fromRoom)
        {
            if (!_rooms.ContainsKey(roomName)) return null;
            
            foreach (var door in _rooms[roomName].Doors)
            {
                if (door.TargetRoom == fromRoom)
                    return door;
            }
            return null;
        }
    }
}
