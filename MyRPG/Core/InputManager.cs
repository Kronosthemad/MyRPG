using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MyRPG.Core
{
    public static class InputManager
    {
        private static KeyboardState _lastKeyState;
        private static KeyboardState _currentKeyState;

        public static Vector2 Direction { get; private set; }

        public static void Update()
        {
            _lastKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();

            // Calculate movement vector
            Vector2 dir = Vector2.Zero;
            if (_currentKeyState.IsKeyDown(Keys.W)) dir.Y -= 1;
            if (_currentKeyState.IsKeyDown(Keys.S)) dir.Y += 1;
            if (_currentKeyState.IsKeyDown(Keys.A)) dir.X -= 1;
            if (_currentKeyState.IsKeyDown(Keys.D)) dir.X += 1;

            // Normalize so diagonal movement isn't faster
            if (dir != Vector2.Zero) dir.Normalize();
            Direction = dir;
        }

        // Handy helper to check if a key was JUST pressed this frame
        public static bool IsKeyPressed(Keys key)
        {
            return _currentKeyState.IsKeyDown(key) && _lastKeyState.IsKeyUp(key);
        }
    }
}