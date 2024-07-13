using System;
using Source.BattleSystem;
using Source.ItemsModule;
using UnityEngine;

namespace Source.PickUpModule
{
    [RequireComponent(typeof(PickUpper))]
    [RequireComponent(typeof(Damageable))]
    [RequireComponent(typeof(ItemLauncher))]
    public class ItemDropper : MonoBehaviour, IDisposable
    {
        private PickUpper _pickUpper = null;
        private Damageable _damageable = null;
        private ItemLauncher _itemLauncher = null;

        public void Initializie()
        {
            _pickUpper ??= GetComponent<PickUpper>();
            _damageable ??= GetComponent<Damageable>();
            _itemLauncher ??= GetComponent<ItemLauncher>();

            _damageable.Death += OnDeath;
        }

        public void Drop(Item item)
        {
            _itemLauncher.DropAndGetEndingDropPosition(item);
        }

        private void OnDeath(IDamageable damageable)
        {
            var itemsPreparedToDrop = _pickUpper.Inventory.GetAllItemsAndClearInventory();
            foreach (var item in itemsPreparedToDrop)
            {
                _itemLauncher.DropAndGetEndingDropPosition(item);
            }
        }

        public void Dispose()
        {
            _damageable.Death -= OnDeath;
            _damageable = null;
            _pickUpper = null;
            _itemLauncher = null;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}