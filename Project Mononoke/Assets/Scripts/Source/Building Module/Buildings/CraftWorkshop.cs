using System.Collections.Generic;
using System.Linq;
using Base.DIContainer;
using Base.GameLoop;
using Scripts.Source.Craft_Module;
using Source.BattleSystem.UI;
using Source.Character.CharacterFabrics;
using Source.Character.Database;
using Source.Formations;
using Source.InventoryModule;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Source.BuildingModule.Buildings
{
    [RequireComponent(typeof(Collider2D))]
    public class CraftWorkshop : Container
    { 
        [SerializeField] private List<Receipt> _receipts = null;
        
        private TestArmy _playersArmy = null;
        private GameLifetimeScope _scope = null;
        private OnGridObjectPlacer _onGridObjectPlacer = null;
        private GameLoop _gameLoop = null;
        private Transform _minionsHolder = null;
        private HealthBarsCanvas _healthBarsCanvas = null;

        public void Initialize(Inventory inventory, IEnumerable<Receipt> receipts, TestArmy playerArmy, GameLifetimeScope scope, OnGridObjectPlacer placer, GameLoop gameLoop, Transform minionsHolder, HealthBarsCanvas healthBarsCanvas)
        {
            base.Initialize(inventory, null);
            _receipts ??= receipts.ToList();
            _playersArmy = playerArmy;
            _scope = scope;
            _onGridObjectPlacer = placer;
            _gameLoop = gameLoop;
            _minionsHolder = minionsHolder;
            _healthBarsCanvas = healthBarsCanvas;
        }
        
        

        public void Craft(Receipt receipt)
        {
            if(!_inventory.HasAllItemsFor(receipt)) return;
            
                foreach (var result in receipt.Results)
                {
                    if (receipt.ReceiptType == ReceiptType.Item)
                    {
                        var itemDatabase = result.GetDatabase<ItemData>();
                        if(itemDatabase.TryGetItemDataBy(result.ItemId, out var itemData)) return;
                        var item = new Item(itemData);
                        ItemViewFabric.Create(item, transform.position); 
                    }
                    else
                    {
                        var charactersDatabase = result.GetDatabase<CharacterData>();
                        if(!charactersDatabase.TryGetItemDataBy(result.ItemId, out var characterData)) return;
                        if(!_playersArmy.TryGetFreePosition(out var inFormationPosition)) return;
                        var characterFabric = new CharactersFabric(characterData, _scope, _onGridObjectPlacer, _minionsHolder, _gameLoop, _healthBarsCanvas);
                        var unit = characterFabric.CreateAt(transform.position);
                        _playersArmy.TryAddToArmy(unit, inFormationPosition);
                    }
                    
                }

            DeleteAllIngredientsFromInventoryUsing(receipt);
        }

        private void DeleteAllIngredientsFromInventoryUsing(Receipt receipt)
        {
            foreach (var ingredient in receipt.Ingredients)
            {
                _inventory.TryGetItem(ingredient.ItemId, out var item);
            }
        }

        public override void StartInteractiveAction(PickUpper pickUpper)
        {
            if(_receipts == null) return;
            
            Craft(_receipts.First());
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.TryGetComponent<ItemView>(out var droppedItemView)) return;
            
            if(_inventory == null) return;
                
            if(_receipts.Any(receipt => receipt.Ingredients.TakeWhile(ingredient => ingredient.ItemId != droppedItemView.Item.Data.ID).Any()))
            {
                return;
            }
                
            _inventory.TryAddItem(droppedItemView.Item);
            droppedItemView.BeginPickUp();
        }
    }
}