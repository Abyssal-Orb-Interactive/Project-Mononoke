using System.Collections.Generic;

namespace Base.Databases
{
    public interface IDatabaseItemValidator<T> where T : IDatabaseItem
    {
        public bool CheckDataCorrectness(T databaseItem, IReadOnlyDictionary<string, T> database);
    }
}