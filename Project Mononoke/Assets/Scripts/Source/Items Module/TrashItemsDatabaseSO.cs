using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;

namespace Source.ItemsModule
{
    [CreateAssetMenu(fileName = "TrashItemsDatabase", menuName = "Databases/Create trash items database")]
    public class TrashItemsDatabaseSO : PickUpableDatabase
    {
        private const float MINIMAL_FLOAT_VALUE = 0.01f;
        private const int MINIMAL_ID_VALUE = 0;
        private const int MINIMAL_STACK_CAPACITY = 1;
        private const int MAX_WARNINGS_NUMBER = 7;

        [SerializeField] private List<ItemData> _savedData = new();

        private Dictionary<int, ItemData> _database = new();

        private readonly List<string> _warningsBuffer = new(MAX_WARNINGS_NUMBER);

        public void ReplaceDatabaseWith(IReadOnlyCollection<ItemData> data)
        {
            _savedData.Clear();

            foreach(var itemData in data)
            {
                _savedData.Add(itemData);
            }

            AddOrOverwriteItemsData(_savedData);
        }

        private void InitializeDatabase()
        {
            AddOrOverwriteItemsData(_savedData);
        }

        private bool IsDatabaseEmpty()
        {
            return _database == null || _database.Count == 0;
        }

        public override bool TryGetItemDataBy (int ID, out ItemData value)
        {
            if(IsDatabaseEmpty()) InitializeDatabase();
            if(_database.TryGetValue(ID, out value)) return true;
            
            #if DEBUG
            Debug.LogWarning($"{name} database doesn't contains item data with {ID} ID");
            #endif
            return false;
        }

        private void AddOrOverwriteItemsData(IReadOnlyCollection<ItemData> itemsData)
        {
            foreach(var data in itemsData)
            {
                TryAddOrOverwriteItemData(data);
            }
        }

        private bool TryAddOrOverwriteItemData (ItemData data)
        {
            _database ??= new();

            if(!CheckDataCorrectness(data)) return false;

            AddOrOverwriteItemData(data);
            return true;
        }

        private void AddOrOverwriteItemData(ItemData data)
        {
            if(_database.ContainsKey(data.ID))
            {
                _database[data.ID] = data;
                return;
            } 
            _database.Add(data.ID, data);
        }

        private bool CheckDataCorrectness(ItemData data)
        {
            _warningsBuffer.Clear();
            var itemName = data.Name;

            if(!CheckIDCorrectness(data.ID))
            {
                _warningsBuffer.Add(GenerateDataCorrectnessWarning("ID", itemName, boundaryValue: MINIMAL_ID_VALUE));
            }

            if(!CheckWeightCorrectness(data.Weight))
            {
                _warningsBuffer.Add(GenerateDataCorrectnessWarning("Weight", itemName));
            }

            if(!CheckVolumeCorrectness(data.Volume))
            {
                _warningsBuffer.Add(GenerateDataCorrectnessWarning("Volume", itemName));
            }

            if(!CheckPriceCorrectness(data.Price))
            {
                _warningsBuffer.Add(GenerateDataCorrectnessWarning("Price", itemName));
            }

            if(!CheckDurabilityCorrectness(data.Durability))
            {
                _warningsBuffer.Add(GenerateDataCorrectnessWarning("Durability", itemName));
            }

            if(!CheckStackSizeCorrectness(data.MaxStackCapacity))
            {
               _warningsBuffer.Add(GenerateDataCorrectnessWarning("MaxStackCapacity", itemName, boundaryValue: MINIMAL_STACK_CAPACITY));
            }

            if(!IsIDUnique(data.ID, data.Name))
            {
                _warningsBuffer.Add( $"All Items in {name} database must have unique ID, ID of {data.Name} already registered for another item, {data.Name} will be excluded and unavailable to game entities from {name} database to avoid possible conflicts");
            }

            if(_warningsBuffer.Count > 0)
            {
                #if DEBUG
                Debug.LogWarning($"Item {data.Name} in {name} database has the following issues:\n{string.Join("\n", _warningsBuffer)}");
                #endif
                return false;
            }

            return true;
        }

        private string GenerateDataCorrectnessWarning(string attribute, string itemName, string comparison = "is greater or equal", string actualCompressionWithBorderValue = "lesser" ,float boundaryValue = MINIMAL_FLOAT_VALUE)
        {
            return $"All Items in {name} database must have {attribute} {comparison} {boundaryValue}, {attribute} of item {itemName} is {actualCompressionWithBorderValue} than {boundaryValue}, {itemName} will be excluded and unavailable to game entities from {name} database to avoid possible conflicts";
        }

        private bool CheckIDCorrectness(int ID)
        {
            return ID >= MINIMAL_ID_VALUE;
        }

        private bool CheckWeightCorrectness(float weight)
        {
            return weight >= MINIMAL_FLOAT_VALUE;
        }

        private bool CheckVolumeCorrectness(float volume)
        {
            return volume >= MINIMAL_FLOAT_VALUE;
        }

        private bool CheckPriceCorrectness(float price)
        {
            return price >= MINIMAL_FLOAT_VALUE;
        }

        private bool CheckDurabilityCorrectness(float durability)
        {
            return durability >= MINIMAL_FLOAT_VALUE;
        }

        private bool CheckStackSizeCorrectness(int maxStackSize)
        {
            return maxStackSize >= MINIMAL_STACK_CAPACITY;
        }

        private bool IsIDUnique(int ID, string name)
        {
            return !_database.ContainsKey(ID) || _database[ID].Name == name;
        }
        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            strBuilder.Append($"{this.name} Database: ");

            foreach(var data in _database.Values)
            {
                strBuilder.AppendLine("\t----------------------------------");
                strBuilder.AppendLine(data.ToString());
                strBuilder.AppendLine("\t----------------------------------");
            }

            return strBuilder.ToString();
        }

        [Serializable]
        public class ItemData 
        {
            [field: SerializeField] public string Name { get; private set; } = "Unknown";
            [field: SerializeField, Range(MINIMAL_ID_VALUE, int.MaxValue)] public int ID { get; private set; } = -1;
            [field: SerializeField] public GameObject Prefab { get; private set; } = null;
            [field: SerializeField, Range(MINIMAL_FLOAT_VALUE, float.MaxValue)] public float Weight { get; private set; } = -1;
            [field: SerializeField, Range(MINIMAL_FLOAT_VALUE, float.MaxValue)] public float Volume { get; private set; } = -1;
            [field: SerializeField, Range(MINIMAL_FLOAT_VALUE, float.MaxValue)] public float Price { get; private set; } = -1;
            [field: SerializeField, Range(MINIMAL_FLOAT_VALUE, float.MaxValue)] public float Durability { get; private set; } = -1;
            [field: SerializeField, Range(MINIMAL_STACK_CAPACITY, int.MaxValue)] public int MaxStackCapacity {get; private set;} = 1;
            [field: SerializeField] public UIItemData UIData {get; private set;} = null;

             public ItemData(int id, string name, float weight, float volume, float price, float durability, int stackCapacity, string spriteName, SpriteAtlas atlas, string description)
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
        }

        [Serializable]
        public class UIItemData
        {
            [field: SerializeField] private SpriteAtlas _iconAtlas = null;
            [field: SerializeField] private string _spriteName  = "Missing";
            [field: SerializeField] [field: TextArea] public String Description {get; private set;} = "Missing";

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
