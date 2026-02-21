using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyRPG.Core
{
    public static class Globals
    {
        // Time elapsed since the last frame (delta time)
        public static float Time { get; set; }
        
        // The main tools for drawing and loading
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }

        // Update the time in your main MyRPG.Update loop
        public static void Update(GameTime gameTime)
        {
            Time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}