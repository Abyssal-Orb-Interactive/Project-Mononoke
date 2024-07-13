using System;
using Base.Databases;
using UnityEngine;
using UnityEngine.U2D;

namespace Source.ItemsModule
{
    [Serializable]
    public class ItemData : IItemData
    {
        
        [field: SerializeField] public string Name { get; private set; } = "Unknown";
        [field: SerializeField] public string ID { get; private set; } = "Unknown";
        [field: SerializeField] public GameObject Prefab { get; private set; } = null;
        [field: SerializeField, Range(0.01f, float.MaxValue)] public float Weight { get; private set; } = -1;
        [field: SerializeField, Range(0.01f, float.MaxValue)] public float Volume { get; private set; } = -1;
        [field: SerializeField, Range(0.01f, float.MaxValue)] public float Price { get; private set; } = -1;
        [field: SerializeField, Range(0.01f, float.MaxValue)] public float Durability { get; private set; } = -1;
        [field: SerializeField, Range(1, 1024)] public int MaxStackCapacity {get; private set;} = 1;
        public UseBehaviour UseBehaviour => ItemsUseBehaviourFabric.GetBehaviour(ID);
        [field: SerializeField] public UIItemData UIData {get; private set;} = null;

        public ItemData(string id, string name, float weight, float volume, float price, float durability, int stackCapacity, string spriteName, SpriteAtlas atlas, string description)
        {
            ID = id;
            Name = name;
            Weight = weight;
            Volume = volume;
            Price = price;
            Durability = durability;
            MaxStackCapacity = stackCapacity;
            UIData = new UIItemData(spriteName, atlas, description);
        }

        public override string ToString()
        {
            return $"\t ID : {ID}\n\t Name : {Name}\n\t Weight : {Weight}\n\t Volume : {Volume}\n\t Price : {Price}\n\t Durability : {Durability}\n\t MaxStackSize : {MaxStackCapacity}\n\t UIData :\n\t\t {UIData} ";
        }
        
        [Serializable]
        public class UIItemData
        {
            [field: SerializeField] private SpriteAtlas _iconAtlas = null;
            [field: SerializeField] private string _spriteName  = "Missing";
            [field: SerializeField] [field: TextArea] public string Description {get; private set;} = "Missing";

            public Sprite Icon => _iconAtlas.GetSprite(_spriteName);

            public UIItemData(string spriteName, SpriteAtlas atlas, string description)
            {
                _spriteName = spriteName;
                _iconAtlas = atlas;
                Description = description;
            }

            public override string ToString()
            {
                return $"Icon Sprite Name : {_spriteName}\n\t Icon Atlas : {_iconAtlas}\n\t Description : {Description} ";
            }
        } 
    }
}