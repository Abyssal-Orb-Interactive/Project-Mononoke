using UnityEngine;

namespace Source.InventoryModule.UI
{
    public class InventoryTableViewWithDescription : InventoryTableView
    {
        [SerializeField] private UIItemDescriptionWindow _descriptionWindow = null;

        protected override void HandleItemSelection(ItemUIElement element)
        {
            base.HandleItemSelection(element);
            _descriptionWindow.InitializeWith(element.StackData.ItemData);
        }

        protected override void ResetSelection()
        {
            base.ResetSelection();
            _descriptionWindow.Reset();
        }
        
        private void OnEnable()
        {
            _descriptionWindow.Reset();
        }
    }
}