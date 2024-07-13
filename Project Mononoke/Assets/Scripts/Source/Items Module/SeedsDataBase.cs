using Base.Databases;
using UnityEngine;

namespace Source.ItemsModule
{
    [CreateAssetMenu(fileName = "SeedsDatabase", menuName = "Databases/Create seeds database")]
    public class SeedsDataBase : DatabaseSO<SeedData>
    {
    }
}