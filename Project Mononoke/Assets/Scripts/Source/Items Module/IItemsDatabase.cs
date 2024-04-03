using System.Collections.Generic;
using UnityEngine;

namespace Source.ItemsModule
{
    public interface IItemsDatabase
    {
        void ReplaceDatabaseWith(IEnumerable<IItemData> data);
        void InitializeDatabase();
        void AddOrOverwriteItemsData(IEnumerable<IItemData> itemsData);
        bool TryAddOrOverwriteItemData (IItemData data);
        void AddOrOverwriteItemData(IItemData data);
        bool TryGetItemDataBy (string ID, out IItemData value);
        bool IsDatabaseEmpty();
        string ToString();
        bool Equals(object other);
        int GetHashCode();
        int GetInstanceID();
        string name { get; set; }
        HideFlags hideFlags { get; set; }
        void SetDirty();
    }
}