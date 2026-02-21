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
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //Texture2D playerTexture = Content.Load<Texture2D>("player_sprite");
            //Texture2D grass = Content.Load<Texture2D>("grass_tile");
            //Texture2D wall = Content.Load<Texture2D>("wall_tile");
            _roomManager = RoomFactory.CreateRooms(grass, wall, doorTex);

            SpriteFont font = Content.Load<SpriteFont>("Fonts/default");
            DialogueSystem.LoadFont(font);
            StatsMenu.LoadFont(font);
            //Texture2D npcTex = Content.Load<Texture2D>("npc_sprite");

            _npcs.Add(new NPC(npcTex, new Vector2(200, 200), "Hello, traveler! Dark times are ahead."));
            _npcs.Add(new NPC(npcTex, new Vector2(400, 200), "I've lost my favorite C++ compiler... have you seen it?"));
            Stats playerStats = new Stats(10, 12, 14, 8, 15, 11, 9, 13, 100, 50, 30, 50, 15, 8);
            _player = new Player(playerTexture, new Vector2(128, 128), playerStats);

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
            InputManager.Update();
            if (InputManager.IsKeyPressed(Keys.I))
            {
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
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
