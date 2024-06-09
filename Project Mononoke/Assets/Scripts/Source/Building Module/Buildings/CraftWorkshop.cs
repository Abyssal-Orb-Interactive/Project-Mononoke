using System.Collections.Generic;
using System.Linq;
using Scripts.Source.Craft_Module;
using Source.InventoryModule;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Source.BuildingModule.Buildings
{
    public class CraftWorkshop : Container
    {
        private IEnumerable<Receipt> _receipts = null;

        public void Initialize(Inventory inventory, IEnumerable<Receipt> receipts)
        {
            base.Initialize(inventory);
            _receipts = receipts;
        }

        public void Craft(Receipt receipt)
        {
            var listOfPreparedIngredients = new List<List<Item>>();
            foreach (var ingredient in receipt.Ingredients)
            {
                var preparedIngredients = new List<Item>();
                listOfPreparedIngredients.Add(preparedIngredients);
                while (preparedIngredients.Count < ingredient.Quantity)
                {
                    if (!_inventory.TryGetItem(ingredient.ItemId, out var item))
                    {
                        foreach (var preparedIngredient in listOfPreparedIngredients.SelectMany(preparedIngredientsList => preparedIngredientsList))
                        {
                            _inventory.TryAddItem(preparedIngredient);
                        }
                        break;
                    }
                    preparedIngredients.Add(item);
                }
            }

            foreach (var craftedItem in receipt.Results)
            {
                Debug.Log($"Crafted {craftedItem.ItemId}");
            }
        }

        public override void StartInteractiveAction(PickUpper pickUpper)
        {
            throw new System.NotImplementedException();
        }
    }
}