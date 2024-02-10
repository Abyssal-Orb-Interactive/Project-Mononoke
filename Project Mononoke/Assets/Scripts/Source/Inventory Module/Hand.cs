using Source.ItemsModule;

namespace Source.InventoryModule
{
    public class Hand
    {
     private IPickUpable _heldObject = null;
     private Inventory _inventory = null;
     private float _strength = 0f; 
     private float _heldVolume = 0f;

     public Hand(Inventory inventory, float strength, float heldVolume)
     {
          _strength = strength;
          _inventory = inventory;
          _heldVolume = heldVolume;
     }

     public void Drop()
     {
          if(_heldObject == null) return;

          _heldObject = null;
     }

     public void Take(IPickUpable item)
     {
          if(item.Weight > _strength || item.Volume > _heldVolume) return;

          Drop();
          _heldObject = item;
     }

     public void StashToInventory()
     {
          if(_heldObject == null) return;
          if(!_inventory.TryAddItem(_heldObject)) return;
          _heldObject = null;
     }
    }
}
