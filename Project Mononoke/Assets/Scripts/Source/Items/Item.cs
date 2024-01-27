using System;
using UnityEngine;

namespace Source.ItemsModule
{
    public class Item : IItem
    {
        public float Weight { get; } = 0f;

        public float Volume { get; } = 0f;

        public float Price { get; } = 0f;

        public float Durability { get; } = 0f;

        public Sprite Icon { get; } = null;

        public string Description { get; } = null;

        public Item(float weight, float volume, float price, float durability, Sprite icon, string description)
        {
            Weight = weight;
            Volume = volume;
            Price = price;
            Durability = durability;
            Icon = icon;
            Description = description;
        }

        public void TakeDamage()
        {
            throw new NotImplementedException();
        }
    }
}
