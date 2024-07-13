using Base.Databases;
using UnityEngine;

namespace Source.ItemsModule
{
    [CreateAssetMenu(fileName = "ItemsDatabase", menuName = "Databases/Create items database")]
    public class TrashItemsDatabaseSO : DatabaseSO<ItemData>
    {
    }
}
