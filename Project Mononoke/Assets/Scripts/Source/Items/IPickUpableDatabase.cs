using System;
using static Source.ItemsModule.ItemsDatabaseSO;

namespace Source.ItemsModule
{
    public interface IPickUpableDatabase
    {
        public bool TryGetItemDataBy (int ID, out ItemData value);
    }
}
