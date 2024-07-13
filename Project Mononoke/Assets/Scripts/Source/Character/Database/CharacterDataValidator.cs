using System;
using System.Collections.Generic;
using Base.Databases;
using UnityEngine;

namespace Source.Character.Database
{
    public class CharacterDataValidator : IDatabaseItemValidator<CharacterData>

    {
        private readonly float _minimalFloatValue = 0f;
        private static readonly List<string> _warningsBuffer = new();

        public CharacterDataValidator(float minimalFloatValue = 0.01f)
        {
            _minimalFloatValue = minimalFloatValue;
        }

        public bool CheckDataCorrectness(CharacterData data, IReadOnlyDictionary<string, CharacterData> database)
        {
            _warningsBuffer.Clear();
            var itemName = data.Name;

            if (!CheckHPCorrectness(data.HP))
            {
                _warningsBuffer.Add(GenerateDataCorrectnessWarning("HP", itemName));
            }

            if (!CheckUnarmedDamageCorrectness(data.UnarmedDamage))
            {
                _warningsBuffer.Add(GenerateDataCorrectnessWarning("UnarmedDamage", itemName));
            }

            if (!CheckManipulatorStrengthCorrectness(data.ManipulatorStrength))
            {
                _warningsBuffer.Add(GenerateDataCorrectnessWarning("ManipulatorStrength", itemName));
            }

            if (!CheckManipulatorCapacityCorrectness(data.ManipulatorCapacity))
            {
                _warningsBuffer.Add(GenerateDataCorrectnessWarning("ManipulatorCapacity", itemName));
            }

            if (!CheckInventoryWeightCapacityCorrectness(data.InventoryWeightCapacity))
            {
                _warningsBuffer.Add(GenerateDataCorrectnessWarning("InventoryWeightCapacity", itemName));
            }

            if (!CheckInventoryVolumeCapacityCorrectness(data.InventoryVolumeCapacity))
            {
                _warningsBuffer.Add(GenerateDataCorrectnessWarning("InventoryVolumeCapacity", itemName));
            }

            if (!IsIDUnique(data.ID, data.Name, database))
            {
                _warningsBuffer.Add(
                    $"All characters in database must have unique ID, ID of {data.Name} already registered for another character, {data.Name} will be excluded and unavailable to game entities from database to avoid possible conflicts");
            }

            return _warningsBuffer.Count <= 0;
#if DEBUG
            Debug.LogWarning(
                $"Item {data.Name} in database has the following issues:\n{string.Join("\n", _warningsBuffer)}");
#endif
        }

        private bool CheckHPCorrectness(float HP)
        {
            return HP >= _minimalFloatValue;
        }

        private bool CheckUnarmedDamageCorrectness(float unarmedDamage)
        {
            return unarmedDamage >= _minimalFloatValue;
        }

        private bool CheckManipulatorStrengthCorrectness(float manipulatorStrength)
        {
            return manipulatorStrength >= _minimalFloatValue;
        }

        private bool CheckManipulatorCapacityCorrectness(float manipulatorCapacity)
        {
            return manipulatorCapacity >= _minimalFloatValue;
        }

        private bool CheckInventoryWeightCapacityCorrectness(float inventoryWeightCapacity)
        {
            return inventoryWeightCapacity >= _minimalFloatValue;
        }

        private bool CheckInventoryVolumeCapacityCorrectness(float inventoryVolumeCapacity)
        {
            return inventoryVolumeCapacity >= _minimalFloatValue;
        }

        private bool IsIDUnique(string ID, string name, IReadOnlyDictionary<string, CharacterData> database)
        {
            return !database.ContainsKey(ID) || database[ID].Name == name;
        }

        private string GenerateDataCorrectnessWarning(string attribute, string itemName,
            string comparison = "is greater or equal", string actualCompressionWithBorderValue = "lesser",
            float boundaryValue = 0.01f)
        {
            return
                $"All characters in database must have {attribute} {comparison} {boundaryValue}, {attribute} of item {itemName} is {actualCompressionWithBorderValue} than {boundaryValue}, {itemName} will be excluded and unavailable to game entities from database to avoid possible conflicts";
        }
    }
}