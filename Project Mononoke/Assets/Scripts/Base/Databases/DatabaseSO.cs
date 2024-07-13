using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Base.Databases
{
    public abstract class DatabaseSO<T> : DatabaseSOBase, IDatabase<T> where T : IDatabaseItem
    {
        [SerializeField] private List<T> _savedData = new();
        
        private Dictionary<string, T> _database = new();
        private IDatabaseItemValidator<T> _databaseItemValidator = null;

        public void ReplaceDatabaseWith(IEnumerable<T> data)
        {
            _savedData.Clear();

            foreach(var databaseItem in data)
            {
                _savedData.Add(databaseItem);
            }

            AddOrOverwriteDatabaseItems(_savedData);
        }
        
        public void Initialize(IDatabaseItemValidator<T> databaseItemValidator)
        {
            _databaseItemValidator = databaseItemValidator;
            AddOrOverwriteDatabaseItems(_savedData);
        }

        public void AddOrOverwriteDatabaseItems(IEnumerable<T> data)
        {
            foreach(var databaseItem in data)
            {
                TryAddOrOverwriteDatabaseItem(databaseItem);
            }
        }

        public bool TryAddOrOverwriteDatabaseItem (T data)
        {
            _database ??= new Dictionary<string, T>();

            if(!_databaseItemValidator.CheckDataCorrectness(data, _database)) return false;

            AddOrOverwriteDatabaseItem(data);
            return true;
        }

        public void AddOrOverwriteDatabaseItem(T data)
        {
            if(!_database.TryAdd(data.ID, data)) _database[data.ID] = data;
        }

        public bool TryGetItemDataBy (string ID, out T value)
        {
            if (IsDatabaseEmpty())
            {
                Debug.Log($"Initialize {name} database before using");
                value = default;
                return false;
            }
            
            if(_database.TryGetValue(ID, out value)) return true;
            
            #if DEBUG
            Debug.LogWarning($"{name} database doesn't contains database item with {ID} ID");
            #endif
            return false;
        }

        public bool IsDatabaseEmpty()
        {
            return _database == null || _database.Count == 0;
        }
        
        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            strBuilder.Append($"{name} Database: ");

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