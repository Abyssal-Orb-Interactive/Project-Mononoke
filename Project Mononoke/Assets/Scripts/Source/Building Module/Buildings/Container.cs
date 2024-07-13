using Source.InventoryModule;

namespace Source.BuildingModule.Buildings
{
    public abstract class Container : Building
    {
        protected Inventory _inventory = null;

        public virtual void Initialize(Inventory inventory)
        {
            _inventory = inventory;
        }
    }
}