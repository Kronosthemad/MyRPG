using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyRPG.Core;
using MyRPG.Entities;

namespace MyRPG.Core
{
    public class InventoryMenu
    {
        private static SpriteFont _font;
        private static bool _isVisible;
        public static bool IsVisible => _isVisible;

        public static void LoadFont(SpriteFont font)
        {
            _font = font;
        }

        public static void Toggle()
        {
            _isVisible = !_isVisible;
        }

        public static void Draw(SpriteBatch spriteBatch, Inventory inventory)
        {
            if (!_isVisible || _font == null) return;

            var items = inventory.GetAll();
            int itemCount = items.Count;
            int maxItems = inventory.MaxSize;

            string[] lines = new string[itemCount + 3];
            lines[0] = $"=== INVENTORY ({itemCount}/{maxItems}) ===";
            lines[1] = "";

            for (int i = 0; i < itemCount; i++)
            {
                lines[i + 2] = $"[{i + 1}] {items[i].Name} - {items[i].Description}";
            }

            if (itemCount == 0)
            {
                lines[2] = "(Empty)";
            }

            lines[lines.Length - 1] = "Press I to close";

            float lineHeight = _font.MeasureString("A").Y;
            float boxWidth = 450;
            float boxHeight = lines.Length * lineHeight + 20;
            float boxX = 600 - boxWidth / 2;
            float boxY = 250 - boxHeight / 2;

            Texture2D blank = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            blank.SetData(new Color[] { new Color(0, 0, 0, 200) });
            spriteBatch.Draw(blank, new Rectangle((int)boxX, (int)boxY, (int)boxWidth, (int)boxHeight), Color.White);

            spriteBatch.Draw(blank, new Rectangle((int)boxX, (int)boxY, (int)boxWidth, 2), Color.White);
            spriteBatch.Draw(blank, new Rectangle((int)boxX, (int)boxY + (int)boxHeight - 2, (int)boxWidth, 2), Color.White);
            spriteBatch.Draw(blank, new Rectangle((int)boxX, (int)boxY, 2, (int)boxHeight), Color.White);
            spriteBatch.Draw(blank, new Rectangle((int)boxX + (int)boxWidth - 2, (int)boxY, 2, (int)boxHeight), Color.White);

            for (int i = 0; i < lines.Length; i++)
            {
                spriteBatch.DrawString(_font, lines[i], new Vector2(boxX + 10, boxY + 10 + i * lineHeight), Color.White);
            }
        }
    }
}
