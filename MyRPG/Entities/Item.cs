using Microsoft.Xna.Framework.Graphics;

namespace MyRPG.Entities
{
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Texture2D Icon { get; set; }
        public int Value { get; set; }

        public Item(string name, string description, Texture2D icon, int value = 0)
        {
            Name = name;
            Description = description;
            Icon = icon;
            Value = value;
        }
    }
}
