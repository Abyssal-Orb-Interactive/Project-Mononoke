using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.ItemsModule
{
    [CreateAssetMenu]
    public class ItemsDatabaseSO : ScriptableObject
    {
        private const float MINIMAL_FLOAT_VALUE = 0.01f;
        private const int MINIMAL_ID_VALUE = 0;
        [SerializeField] private List<ItemData> _itemsData = null;
        private Dictionary<int, ItemData> _itemsDataFast = new();

        private void OnValidate() 
        {
            _itemsDataFast.Clear();

            foreach(var itemData in _itemsData)
            {
                if(!CheckDataCorrectness(itemData)) 
                {
                    continue;
                }
                _itemsDataFast.Add(itemData.ID, itemData);

            }  
        }

        private void Start()
        {
            _itemsData = null;
        }

        private bool CheckDataCorrectness(ItemData data)
        {
            var result = false;

            result =  CheckIDCorrectness(data.ID);
            if(!result)
            {
                Debug.LogWarning($"All Items in {name} database must have ID greater or equal {MINIMAL_ID_VALUE}, ID of item {data.Name} is lesser than {MINIMAL_ID_VALUE}, {data.Name} will be excluded and unavailable to game entities from {name} database to avoid possible conflicts");
            }

            result = CheckWeightCorrectness(data.Weight);
            if(!result)
            {
                Debug.LogWarning($"All Items in {name} database must have weight greater or equal {MINIMAL_FLOAT_VALUE}, weight of item {data.Name} is lesser than {MINIMAL_FLOAT_VALUE}, {data.Name} will be excluded and unavailable to game entities from {name} database to avoid possible conflicts");
            }

            result = CheckVolumeCorrectness(data.Volume);
            if(!result)
            {
                Debug.LogWarning($"All Items in {name} database must have volume greater or equal {MINIMAL_FLOAT_VALUE}, volume of item {data.Name} is lesser than {MINIMAL_FLOAT_VALUE}, {data.Name} will be excluded and unavailable to game entities from {name} database to avoid possible conflicts");
            }

            result = CheckPriceCorrectness(data.Price);
            if(!result)
            {
                Debug.LogWarning($"All Items in {name} database must have price greater or equal {MINIMAL_FLOAT_VALUE}, price of item {data.Name} is lesser than {MINIMAL_FLOAT_VALUE}, {data.Name} will be excluded and unavailable to game entities from {name} database to avoid possible conflicts");
            }

            result = CheckDurabilityCorrectness(data.Durability);
            if(!result)
            {
                Debug.LogWarning($"All Items in {name} database must have durability greater or equal {MINIMAL_FLOAT_VALUE}, durability of item {data.Name} is lesser than {MINIMAL_FLOAT_VALUE}, {data.Name} will be excluded and unavailable to game entities from {name} database to avoid possible conflicts");
            }

            result = IsIDUnique(data.ID);
            if(!result)
            {
                Debug.LogWarning( $"All Items in {name} database must have unique ID, ID of {data.Name} already registered for another item, {data.Name} will be excluded and unavailable to game entities from {name} database to avoid possible conflicts");
            }

            return result;
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
        private bool IsIDUnique(int ID)
        {
            return !_itemsDataFast.ContainsKey(ID);
        }  

        public bool TryGetItemDataBy (int ID, ref ItemData value)
        {
            if(!_itemsDataFast.ContainsKey(ID))
            {
                Debug.LogWarning($"{name} database doesn't contains item data with {ID} ID");
                return false;
            }

            value = GetItemDataBy(ID);
            return true;
        }

        private ItemData GetItemDataBy(int ID)
        {
            return _itemsDataFast[ID];
        }  

        [Serializable]
        public class ItemData 
        {
            [field: SerializeField] public string Name { get; private set; } = null;
            [field: SerializeField] public int ID { get; private set; } = -1;
            [field: SerializeField] public GameObject Prefab { get; private set; } = null;
            [field: SerializeField] public float Weight { get; private set; } = -1;
            [field: SerializeField] public float Volume { get; private set; } = -1;
            [field: SerializeField] public float Price { get; private set; } = -1;
            [field: SerializeField] public float Durability { get; private set; } = -1;
            [field: SerializeField] public UIItemData UIData {get; private set;} = null;
        }

        [Serializable]
        public class UIItemData
        {
            [field: SerializeField] public Sprite Icon { get; private set; } = null;
            [field: SerializeField] public String Description {get; private set;} = null;

        }    
    }
}
