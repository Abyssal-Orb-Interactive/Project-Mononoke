using System.Collections.Generic;
using UnityEngine;

namespace Source.ItemsModule
{
    public static class ItemDataValidator
    {
        private const float MINIMAL_FLOAT_VALUE = 0.01f;
        private const int MINIMAL_STACK_CAPACITY = 1;
        private const int MAX_WARNINGS_NUMBER = 7;
        private  static readonly List<string> _warningsBuffer = new(MAX_WARNINGS_NUMBER);
        
        public static bool CheckDataCorrectness<T>(IItemData data, IReadOnlyDictionary<string, T> database) where T : IItemData
        {
            _warningsBuffer.Clear();
            var itemName = data.Name;

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
               _warningsBuffer.Add(GenerateDataCorrectnessWarning("MaxStackCapacity", itemName, boundaryValue: data.MaxStackCapacity));
            }

            if(!IsIDUnique(data.ID, data.Name, database))
            {
                _warningsBuffer.Add( $"All Items in database must have unique ID, ID of {data.Name} already registered for another item, {data.Name} will be excluded and unavailable to game entities from database to avoid possible conflicts");
            }

            if (_warningsBuffer.Count <= 0) return true;
            #if DEBUG
            Debug.LogWarning($"Item {data.Name} in database has the following issues:\n{string.Join("\n", _warningsBuffer)}");
            #endif
            return false;

        }

        private static string GenerateDataCorrectnessWarning(string attribute, string itemName, string comparison = "is greater or equal", string actualCompressionWithBorderValue = "lesser" ,float boundaryValue = MINIMAL_FLOAT_VALUE)
        {
            return $"All Items in database must have {attribute} {comparison} {boundaryValue}, {attribute} of item {itemName} is {actualCompressionWithBorderValue} than {boundaryValue}, {itemName} will be excluded and unavailable to game entities from database to avoid possible conflicts";
        }

        private static bool CheckWeightCorrectness(float weight)
        {
            return weight >= MINIMAL_FLOAT_VALUE;
        }

        private static bool CheckVolumeCorrectness(float volume)
        {
            return volume >= MINIMAL_FLOAT_VALUE;
        }

        private static bool CheckPriceCorrectness(float price)
        {
            return price >= MINIMAL_FLOAT_VALUE;
        }

        private static bool CheckDurabilityCorrectness(float durability)
        {
            return durability >= MINIMAL_FLOAT_VALUE;
        }

        private static bool CheckStackSizeCorrectness(int maxStackSize)
        {
            return maxStackSize >= MINIMAL_STACK_CAPACITY;
        }

        private static bool IsIDUnique<T>(string ID, string name, IReadOnlyDictionary<string, T> database) where T : IItemData
        {
            return !database.ContainsKey(ID) || database[ID].Name == name;
        }
    }
}