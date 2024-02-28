using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Source.ItemsModule
{
    public delegate void UseBehaviour(object context);
    
    [Serializable]
    public abstract class ItemsDatabase<T> : ScriptableObject where T : ItemData
    {
        [SerializeField] private List<T> _savedData = new();

        private Dictionary<string, T> _database = new();
        
        public void ReplaceDatabaseWith(IEnumerable<T> data)
        {
            _savedData.Clear();

            foreach(var itemData in data)
            {
                _savedData.Add(itemData);
            }

            AddOrOverwriteItemsData(_savedData);
        }
        
        private void InitializeDatabase()
        {
            AddOrOverwriteItemsData(_savedData);
        }

        private void AddOrOverwriteItemsData(IEnumerable<T> itemsData)
        {
            foreach(var data in itemsData)
            {
                TryAddOrOverwriteItemData(data);
            }
        }
        
        private bool TryAddOrOverwriteItemData (T data)
        {
            _database ??= new Dictionary<string, T>();

            if(!ItemDataValidator.CheckDataCorrectness(data, _database)) return false;

            AddOrOverwriteItemData(data);
            return true;
        }

        private void AddOrOverwriteItemData(T data)
        {
            if(!_database.TryAdd(data.ID, data)) _database[data.ID] = data;
        }
        
        public virtual bool TryGetItemDataBy (string ID, out T value)
        {
            if(IsDatabaseEmpty()) InitializeDatabase();
            if(_database.TryGetValue(ID, out value)) return true;
            
            #if DEBUG
            Debug.LogWarning($"{name} database doesn't contains item data with {ID} ID");
            #endif
            return false;
        }
        
        protected bool IsDatabaseEmpty()
        {
            return _database == null || _database.Count == 0;
        }
        
        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            strBuilder.Append($"{this.name} Database: ");

            foreach(var data in _database.Values)
            {
                strBuilder.AppendLine("\t----------------------------------");
                strBuilder.AppendLine(data.ToString());
                strBuilder.AppendLine("\t----------------------------------");
            }

            return strBuilder.ToString();
        }
    }
}
