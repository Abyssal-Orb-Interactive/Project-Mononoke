using System.Collections.Generic;
using System.Linq;
using Base.Databases;

namespace Source.ItemsModule
{
    public class SeedDataValidator : ItemDataValidator, IDatabaseItemValidator<SeedData>
    {
        public bool CheckDataCorrectness(SeedData databaseItem, IReadOnlyDictionary<string, SeedData> database)
        {
            var itemsDatabase = database.ToDictionary<KeyValuePair<string, SeedData>, string, ItemData>(kvp => kvp.Key, kvp => kvp.Value);
            return base.CheckDataCorrectness(databaseItem, itemsDatabase);
        }
    }
}