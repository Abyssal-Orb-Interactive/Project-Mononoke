using System;
using System.Collections.Generic;
using Base.Databases;
using UnityEngine;

namespace Scripts.Source.Craft_Module
{
    [Serializable]
    public class Receipt : IDatabaseItem
    {
        [field: SerializeField] public string ID { get; private set; } = null;
        [field: SerializeField] public List<Ingredient> Ingredients { get; private set; } = null;
        [field: SerializeField] public List<Ingredient> Results { get; private set; } = null;
        [field: SerializeField] public ReceiptType ReceiptType { get; private set; } = ReceiptType.Item;

        public Receipt(string id, List<Ingredient> ingredients, List<Ingredient> results, ReceiptType receiptType)
        {
            ID = id;
            Ingredients = ingredients;
            Results = results;
            ReceiptType = receiptType;
        }
    }
}