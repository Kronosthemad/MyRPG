using System.Collections.Generic;
using MyRPG.Entities;

namespace MyRPG.Core
{
    public class Inventory
    {
        private List<Item> _items;
        private int _maxSize;

        public int Count => _items.Count;
        public int MaxSize => _maxSize;

        public Inventory(int maxSize = 20)
        {
            _maxSize = maxSize;
            _items = new List<Item>();
        }

        public bool Add(Item item)
        {
            if (_items.Count >= _maxSize)
                return false;
            
            _items.Add(item);
            return true;
        }

        public bool Remove(Item item)
        {
            return _items.Remove(item);
        }

        public Item Get(int index)
        {
            if (index >= 0 && index < _items.Count)
                return _items[index];
            return null;
        }

        public List<Item> GetAll()
        {
            return new List<Item>(_items);
        }
    }
}
