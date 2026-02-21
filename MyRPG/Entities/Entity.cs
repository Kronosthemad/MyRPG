using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyRPG.Core;
using MyRPG.Data;

namespace MyRPG.Entities
{
    public abstract class Entity
    {
        protected Texture2D _texture;
        public Vector2 Position;
        public float Speed;
        public int Width => _texture?.Width ?? 32;
        public int Height => _texture?.Height ?? 32;

        public Stats Stats { get; set; }

        public Entity(Texture2D texture, Vector2 startPosition)
        {
            _texture = texture;
            Position = startPosition;
            Speed = 200f; // Default speed
        }

        // 'virtual' allows child classes to override this logic
        public virtual void Update()
        {
            // Base entities don't do much by default
        }

        public virtual void Draw()
        {
            // We use the Global SpriteBatch so we don't have to pass it in
            Globals.SpriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}