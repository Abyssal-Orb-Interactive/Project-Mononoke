using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Source.Craft_Module
{
    [Serializable]
    public class Receipt
    {
        [field: SerializeField] public string ReceiptID = null;
        [field: SerializeField] public List<Ingredient> Ingredients { get; private set; } = null;
        [field: SerializeField] public List<Ingredient> Results { get; private set; } = null;

        public Receipt(string receiptID, List<Ingredient> ingredients, List<Ingredient> results)
        {
            ReceiptID = receiptID;
            Ingredients = ingredients;
            Results = results;
        }
    }
}