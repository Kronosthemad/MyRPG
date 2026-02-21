using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyRPG.Data;

namespace MyRPG.Core
{
    public class StatsMenu
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

        public static void Draw(SpriteBatch spriteBatch, Stats stats)
        {
            if (!_isVisible || _font == null) return;

            string[] statLines = new string[]
            {
                "=== CHARACTER STATS ===",
                "",
                "--- Base Attributes ---",
                $"Strength:     {stats.Strength}",
                $"Perception:   {stats.Perception}",
                $"Intelligence: {stats.Intelligence}",
                $"Wisdom:       {stats.Wisdom}",
                $"Endurance:    {stats.Endurance}",
                $"Dexterity:    {stats.Dexterity}",
                $"Charisma:     {stats.Charisma}",
                $"Luck:         {stats.Luck}",
                "",
                "--- Combat Stats ---",
                $"Damage:   {stats.Damage}",
                $"Defense:  {stats.Defense}",
                "",
                "--- Resources ---",
                $"Health:   {stats.CurrentHealth} / {stats.MaxHealth}",
                $"Stamina:  {stats.CurrentStamina} / {stats.MaxStamina}",
                $"Magic:    {stats.CurrentMagic} / {stats.MaxMagic}",
                $"Money:    {stats.Money} gold"
            };

            float lineHeight = _font.MeasureString("A").Y;
            float boxWidth = 280;
            float boxHeight = statLines.Length * lineHeight + 20;
            float boxX = 20;
            float boxY = 20;

            // Draw semi-transparent background
            Texture2D blank = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            blank.SetData(new Color[] { new Color(0, 0, 0, 180) });
            spriteBatch.Draw(blank, new Rectangle((int)boxX, (int)boxY, (int)boxWidth, (int)boxHeight), Color.White);

            // Draw border
            spriteBatch.Draw(blank, new Rectangle((int)boxX, (int)boxY, (int)boxWidth, 2), Color.White);
            spriteBatch.Draw(blank, new Rectangle((int)boxX, (int)boxY + (int)boxHeight - 2, (int)boxWidth, 2), Color.White);
            spriteBatch.Draw(blank, new Rectangle((int)boxX, (int)boxY, 2, (int)boxHeight), Color.White);
            spriteBatch.Draw(blank, new Rectangle((int)boxX + (int)boxWidth - 2, (int)boxY, 2, (int)boxHeight), Color.White);

            // Draw text
            for (int i = 0; i < statLines.Length; i++)
            {
                spriteBatch.DrawString(_font, statLines[i], new Vector2(boxX + 10, boxY + 10 + i * lineHeight), Color.White);
            }
        }
    }
}
