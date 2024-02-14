using System;
using UnityEngine;
using static Source.ItemsModule.TrashItemsDatabaseSO;

namespace Source.ItemsModule
{
    [Serializable]
    public abstract class PickUpableDatabase : ScriptableObject
    {
        public abstract bool TryGetItemDataBy (int ID, out ItemData value);
    }
}
