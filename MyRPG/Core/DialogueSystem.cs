using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyRPG.Core
{
    public class DialogueSystem
    {
        private static SpriteFont _font;
        private static string _currentDialogue;
        private static float _timer;
        private static float _displayTime = 3f;

        public static void LoadFont(SpriteFont font)
        {
            _font = font;
        }

        public static void Show(string dialogue)
        {
            _currentDialogue = dialogue;
            _timer = _displayTime;
        }

        public static void Update(float deltaTime)
        {
            if (_timer > 0)
            {
                _timer -= deltaTime;
                if (_timer <= 0)
                {
                    _currentDialogue = null;
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            if (_currentDialogue != null && _font != null)
            {
                Vector2 textSize = _font.MeasureString(_currentDialogue);
                Vector2 position = new Vector2(screenWidth / 2 - textSize.X / 2, screenHeight - 100);

                spriteBatch.DrawString(_font, _currentDialogue, position + new Vector2(2, 2), Color.Black);
                spriteBatch.DrawString(_font, _currentDialogue, position, Color.White);
            }
        }
    }
}
