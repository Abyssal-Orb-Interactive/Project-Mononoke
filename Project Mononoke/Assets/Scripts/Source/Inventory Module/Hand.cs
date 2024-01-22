using Source.ItemsModule;

namespace Source.InventoryModule
{
    public class Hand
    {
       private IPickUpable _retainedObject = null;

       public void Drop()
       {
            if(_retainedObject == null) return;

            _retainedObject = null;
       }
    }
}
