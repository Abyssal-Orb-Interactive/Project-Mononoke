using System;
using UnityEngine;
using UnityEngine.U2D;

namespace Source.ItemsModule
{
    [Serializable]
    public class SeedData : ItemData
    {
        [field: SerializeField] public int MinGrownTimeInSeconds { get; private set; } = 1;
        [field: SerializeField] public int MaxGrownTimeInSeconds { get; private set; } = 1;
        
        public SeedData(string id, string name, float weight, float volume, float price, float durability, int stackCapacity, string spriteName, SpriteAtlas atlas, string description, int minGrownTimeInSeconds, int maxGrownTimeInSeconds) : base(id, name, weight, volume, price, durability, stackCapacity, spriteName, atlas, description)
        {
            MinGrownTimeInSeconds = minGrownTimeInSeconds;
            MaxGrownTimeInSeconds = maxGrownTimeInSeconds;
        }
    }
}