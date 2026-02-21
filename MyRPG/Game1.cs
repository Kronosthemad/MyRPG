using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyRPG.Core;
using MyRPG.Data;
using MyRPG.Entities;
using MyRPG.World;
using System.Collections.Generic;

namespace MyRPG
{
    public class MyRPG : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;
        private RoomManager _roomManager;
        private Camera _camera;
        private List<NPC> _npcs = new List<NPC>();
        private List<Boss> _boss = new List<Boss>();

        public MyRPG()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            
            Window.IsBorderless = false;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            _camera = new Camera();
        }

        private Texture2D CreatePlaceholderTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; i++) data[i] = color;
            texture.SetData(data);
            return texture;
        }
        protected override void LoadContent()
        { 
            // No files required with this method!
            Texture2D playerTexture = CreatePlaceholderTexture(32, 32, Color.Blue);
            Texture2D grass = CreatePlaceholderTexture(64, 64, Color.DarkGreen);
            Texture2D wall = CreatePlaceholderTexture(64, 64, Color.Gray);
            Texture2D doorTex = CreatePlaceholderTexture(64, 64, Color.Brown);
            Texture2D npcTex = CreatePlaceholderTexture(32, 32, Color.Green);
            Texture2D itemTex = CreatePlaceholderTexture(16, 16, Color.Yellow);
            Texture2D bossTex = CreatePlaceholderTexture(64, 64, Color.Red);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _roomManager = RoomFactory.CreateRooms(grass, wall, doorTex);

            SpriteFont font = Content.Load<SpriteFont>("Fonts/default");
            DialogueSystem.LoadFont(font);
            StatsMenu.LoadFont(font);
            InventoryMenu.LoadFont(font);
            _boss.Add(new Boss(bossTex, new Vector2(300, 300), "I am the mighty boss! Prepare to be defeated!", "bossroom", 200));
            _npcs.Add(new NPC(npcTex, new Vector2(200, 200), "Hello, traveler! Dark times are ahead.", "hallway"));
            _npcs.Add(new NPC(npcTex, new Vector2(400, 200), "I've lost my favorite C++ compiler... have you seen it?", "hallway"));
            Stats playerStats = new Stats(10, 12, 14, 8, 15, 11, 9, 13, 100, 50, 30, 50, 15, 8);
            _player = new Player(playerTexture, new Vector2(128, 128), playerStats);
            _player.Inventory.Add(new Item("Gold Coin", "A shiny gold coin", npcTex, 10));
            _player.Inventory.Add(new Item("Health Potion", "Restores 50 health", npcTex, 25));
            _player.Inventory.Add(new Item("Rusty Sword", "An old but serviceable sword", npcTex, 15));

            Globals.Content = Content;
            Globals.SpriteBatch = _spriteBatch;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            foreach (var npc in _npcs)
            {
                if (npc.RoomName != _roomManager.CurrentRoom.Name) continue;

                npc.Update(_roomManager.CurrentRoom, _player, _npcs);

                // Check distance between Player and NPC (Simple Pythagorean theorem)
                float distance = Vector2.Distance(_player.Position, npc.Position);

                if (distance < 50f && InputManager.IsKeyPressed(Keys.Space))
                {
                    DialogueSystem.Show(npc.Dialogue);
                }
            }
            // TODO: Add your update logic here
            Globals.Update(gameTime);
            
            //The Key Check For Inventory and Stats Menu is here,
            //we want it to be checked before the player movement
            //so that the player doesn't move when opening the menu
            InputManager.Update();
            if (InputManager.IsKeyPressed(Keys.I))
            {
                InventoryMenu.Toggle();
                StatsMenu.Toggle();
            }


            DialogueSystem.Update(Globals.Time);
            
            _player.Update(_roomManager.CurrentRoom, _npcs);

            string targetRoom = _roomManager.GetTargetRoom(_player.Position, _player.Width, _player.Height);
            if (targetRoom != null)
            {
                string currentRoomName = _roomManager.CurrentRoom.Name;
                _roomManager.SetCurrentRoom(targetRoom);
                _player.Position = _roomManager.GetSpawnPosition(targetRoom, currentRoomName);
            }


            _camera.Follow(_player.Position);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(transformMatrix: _camera.Transform);
            _roomManager.CurrentRoom.Draw();
            foreach (var npc in _npcs)
            {
                if (npc.RoomName == _roomManager.CurrentRoom.Name)
                    npc.Draw();
            }
            _player.Draw(_spriteBatch); // Tell the player to draw itself
            _spriteBatch.End();
            _spriteBatch.Begin();
            DialogueSystem.Draw(_spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            StatsMenu.Draw(_spriteBatch, _player.Stats);
            InventoryMenu.Draw(_spriteBatch, _player.Inventory);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
