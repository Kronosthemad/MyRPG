using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyRPG.World;

namespace MyRPG.Data
{
    public static class RoomFactory
    {
        public static RoomManager CreateRooms(Texture2D grassTex, Texture2D wallTex, Texture2D doorTex)
        {
            var manager = new RoomManager();

            manager.AddRoom("hallway", CreateHallway(grassTex, wallTex, doorTex));
            manager.AddRoom("treasure", CreateTreasureRoom(grassTex, wallTex, doorTex));
            manager.AddRoom("armory", CreateArmoryRoom(grassTex, wallTex, doorTex));
            manager.AddRoom("boss", CreateBossRoom(grassTex, wallTex, doorTex));

            manager.SetCurrentRoom("hallway");
            return manager;
        }

        private static Room CreateHallway(Texture2D grassTex, Texture2D wallTex, Texture2D doorTex)
        {
            int width = 12;
            int height = 8;
            var tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isBorder = (x == 0 || y == 0 || x == width - 1 || y == height - 1);
                    tiles[x, y] = new Tile(isBorder ? wallTex : grassTex, isBorder);
                }
            }

            var room = new Room(width, height, "hallway", tiles);
            room.DoorTexture = doorTex;
            room.Doors.Add(new Door("treasure", new Vector2(0, 3 * 64), new Vector2(5 * 64, 3 * 64), 64, 64));
            room.Doors.Add(new Door("armory", new Vector2(11 * 64, 3 * 64), new Vector2(6 * 64, 3 * 64), 64, 64));
            room.Doors.Add(new Door("boss", new Vector2(5 * 64, 0), new Vector2(5 * 64, 4 * 64), 64, 64));

            return room;
        }

        private static Room CreateTreasureRoom(Texture2D grassTex, Texture2D wallTex, Texture2D doorTex)
        {
            int width = 8;
            int height = 8;
            var tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isBorder = (x == 0 || y == 0 || x == width - 1 || y == height - 1);
                    tiles[x, y] = new Tile(isBorder ? wallTex : grassTex, isBorder);
                }
            }

            var room = new Room(width, height, "treasure", tiles);
            room.DoorTexture = doorTex;
            room.Doors.Add(new Door("hallway", new Vector2(7 * 64, 3 * 64), new Vector2(1 * 64, 3 * 64), 64, 64));

            return room;
        }

        private static Room CreateArmoryRoom(Texture2D grassTex, Texture2D wallTex, Texture2D doorTex)
        {
            int width = 8;
            int height = 8;
            var tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isBorder = (x == 0 || y == 0 || x == width - 1 || y == height - 1);
                    tiles[x, y] = new Tile(isBorder ? wallTex : grassTex, isBorder);
                }
            }

            var room = new Room(width, height, "armory", tiles);
            room.DoorTexture = doorTex;
            room.Doors.Add(new Door("hallway", new Vector2(0, 3 * 64), new Vector2(6 * 64, 3 * 64), 64, 64));

            return room;
        }

        private static Room CreateBossRoom(Texture2D grassTex, Texture2D wallTex, Texture2D doorTex)
        {
            int width = 10;
            int height = 8;
            var tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isBorder = (x == 0 || y == 0 || x == width - 1 || y == height - 1);
                    tiles[x, y] = new Tile(isBorder ? wallTex : grassTex, isBorder);
                }
            }

            var room = new Room(width, height, "boss", tiles);
            room.DoorTexture = doorTex;
            room.Doors.Add(new Door("hallway", new Vector2(4 * 64, 7 * 64), new Vector2(5 * 64, 1 * 64), 64, 64));

            return room;
        }
    }
}
