using Source.BuildingModule;
using UnityEngine;
using VContainer;

namespace Source.ItemsModule
{
    public static class ItemViewFabric
    {
        private static ItemView _itemViewPrefab = null;
        private static Transform _itemViewsContainer = null;
        private static OnGridObjectPlacer _placer = null;

        [Inject]
        public static void Initialize(ItemView itemViewPrefab, Transform itemViewsContainer, OnGridObjectPlacer placer)
        {
            _itemViewPrefab = itemViewPrefab;
            _itemViewsContainer = itemViewsContainer;
            _placer = placer;
        }
        
        public static void Create(Item item, Vector3 position)
        {
            if(!CheckFabricCreationStatus(item)) return;
            
            var itemView = _placer.PlaceObject(new ObjectPlacementInformation<ItemView>(_itemViewPrefab, position, Quaternion.identity , _itemViewsContainer));
            itemView.Initialize(item);
        }

        private static bool CheckFabricCreationStatus(Item item)
        {
            return _itemViewPrefab != null && _itemViewsContainer != null && _placer != null && item != null;
        }
    }
}