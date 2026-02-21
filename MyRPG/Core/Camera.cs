using Microsoft.Xna.Framework;

namespace MyRPG.Core
{
    public class Camera
    {
        // This is the magic math that we pass to the SpriteBatch
        public Matrix Transform { get; private set; }

        public void Follow(Vector2 target)
        {
            // Center the camera on the target (the player)
            // 400 and 300 assume a default 800x600 window. 
            // We subtract these to keep the player in the middle.
            var position = Matrix.CreateTranslation(
                -target.X,
                -target.Y,
                0);

            var offset = Matrix.CreateTranslation(
                400, 
                300, 
                0);

            Transform = position * offset;
        }
    }
}