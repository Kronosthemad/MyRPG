using System;

namespace MyRPG.Data
{
    public class Stats
    {
        // D&D style base attributes
        public int Strength { get; set; }
        public int Perception { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Endurance { get; set; }
        public int Dexterity { get; set; }
        public int Charisma { get; set; }
        public int Luck { get; set; }

        // Dynamic stats
        private int _currentHealth;
        public int MaxHealth { get; set; }
        public int CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = Math.Clamp(value, 0, MaxHealth);
        }

        private int _currentStamina;
        public int MaxStamina { get; set; }
        public int CurrentStamina
        {
            get => _currentStamina;
            set => _currentStamina = Math.Clamp(value, 0, MaxStamina);
        }

        private int _currentMagic;
        public int MaxMagic { get; set; }
        public int CurrentMagic
        {
            get => _currentMagic;
            set => _currentMagic = Math.Clamp(value, 0, MaxMagic);
        }

        private int _currentMoney;
        public int Money { get; set; }
        public int CurrentMoney
        {
            get => _currentMoney;
            set => _currentMoney = Math.Clamp(value, 0, Money);
        }

        // Combat stats
        public int Damage { get; set; }
        public int Defense { get; set; }

        public Stats(int strength, int perception, int intelligence, int wisdom, 
            int endurance, int dexterity, int charisma, int luck,
            int health, int stamina, int magic, int money, int damage, int defense)
        {
            Strength = strength;
            Perception = perception;
            Intelligence = intelligence;
            Wisdom = wisdom;
            Endurance = endurance;
            Dexterity = dexterity;
            Charisma = charisma;
            Luck = luck;

            MaxHealth = health;
            _currentHealth = health;
            MaxStamina = stamina;
            _currentStamina = stamina;
            MaxMagic = magic;
            _currentMagic = magic;
            Money = money;
            _currentMoney = money;
            Damage = damage;
            Defense = defense;
        }
    }
}
