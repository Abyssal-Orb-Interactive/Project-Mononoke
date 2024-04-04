using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

namespace Source.ItemsModule
{
    [Serializable]
    public class SeedData : ItemData
    {
        [field: SerializeField] private int MinGrownTimeInSeconds { get; set; } = 1;
        [field: SerializeField] private int MaxGrownTimeInSeconds { get; set; } = 1;
        [field: SerializeField] public List<Sprite> PlantGrowthStagesSprites { get; private set; } = null;
        public int GrownTimeInSeconds => Random.Range(MinGrownTimeInSeconds - 1, MaxGrownTimeInSeconds + 1);
        
        public SeedData(string id, string name, float weight, float volume, float price, float durability, int stackCapacity, string spriteName, SpriteAtlas atlas, string description, int minGrownTimeInSeconds, int maxGrownTimeInSeconds, List<Sprite> plantGrowthStagesSprites) : base(id, name, weight, volume, price, durability, stackCapacity, spriteName, atlas, description)
        {
            MinGrownTimeInSeconds = minGrownTimeInSeconds;
            MaxGrownTimeInSeconds = maxGrownTimeInSeconds;
            PlantGrowthStagesSprites = plantGrowthStagesSprites;
        }
    }
}