using System.Collections.Generic;

namespace Base.Databases
{
    public interface IDatabase<T> where T : IDatabaseItem
    {
        public void ReplaceDatabaseWith(IEnumerable<T> data);
        public void Initialize(IDatabaseItemValidator<T> databaseItemValidator);
        public void AddOrOverwriteDatabaseItems(IEnumerable<T> data);
        public bool TryAddOrOverwriteDatabaseItem(T data);
        public void AddOrOverwriteDatabaseItem(T data);
        public bool TryGetItemDataBy(string ID, out T value);
        public bool IsDatabaseEmpty();
    }
}