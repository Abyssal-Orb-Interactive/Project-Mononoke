using System;
using System.Collections.Generic;
using Base.Databases;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

namespace Source.ItemsModule
{
    [Serializable]
    public class SeedData : ItemData
    {
        [field: SerializeField, Range(1, 1024)] private int MinGrownTimeInSeconds { get; set; } = 1;
        [field: SerializeField, Range(1, 1024)] private int MaxGrownTimeInSeconds { get; set; } = 1;
        [field: SerializeField, Range(1, 1024)] private int MinGrownFruits { get; set; } = 1;
        [field: SerializeField, Range(1, 1024)] private int MaxGrownFruits { get; set; } = 1;
        [field: SerializeField, Range(1, 1024)] private int MinSeedsFromFruit { get; set; } = 1;
        [field: SerializeField, Range(1, 1024)] private int MaxSeedsFromFruit { get; set; } = 1;
        [field: SerializeField] public List<Sprite> PlantGrowthStagesSprites { get; private set; } = null;
        [field: SerializeField] public DatabaseSO<ItemData> FruitDatabase { get; private set; } = null;
        [field: SerializeField] public string FruitDatabaseID { get; private set; } = null;
        public int GrownTimeInSeconds => Random.Range(MinGrownTimeInSeconds, MaxGrownTimeInSeconds);
        public int GrownFruits => Random.Range(MinGrownFruits, MaxGrownFruits);
        public int GrownSeeds => Random.Range(MinSeedsFromFruit, MaxSeedsFromFruit);
        public int NumberOfGrowthStages => PlantGrowthStagesSprites.Count;
        
        public SeedData(string id, string name, float weight, float volume, float price, float durability, int stackCapacity, string spriteName, SpriteAtlas atlas, string description, int minGrownTimeInSeconds, int maxGrownTimeInSeconds, List<Sprite> plantGrowthStagesSprites, int minGrownFruits, int maxGrownFruits, DatabaseSO<ItemData> fruitDatabase, string fruitDatabaseID) : base(id, name, weight, volume, price, durability, stackCapacity, spriteName, atlas, description)
        {
            MinGrownTimeInSeconds = minGrownTimeInSeconds;
            MaxGrownTimeInSeconds = maxGrownTimeInSeconds;
            PlantGrowthStagesSprites = plantGrowthStagesSprites;
            MinGrownFruits = minGrownFruits;
            MaxGrownFruits = maxGrownFruits;
            FruitDatabase = fruitDatabase;
            FruitDatabaseID = fruitDatabaseID;
        }
    }
}