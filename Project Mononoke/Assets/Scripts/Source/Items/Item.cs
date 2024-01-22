using System;

namespace Source.ItemsModule
{
    public class Item : IItem
    {
        public float Weight { get; } = 0f;

        public float Volume { get; } = 0f;

        public float Price { get; } = 0f;

        public float Durability { get; } = 0f; 

        public Item(float weight, float volume, float price, float durability)
        {
            Weight = weight;
            Volume = volume;
            Price = price;
            Durability = durability;
        }

        public void TakeDamage()
        {
            throw new NotImplementedException();
        }
    }
}
