using Base.Databases;
using UnityEngine;

namespace Scripts.Source.Craft_Module
{
    [CreateAssetMenu(fileName = "ReceiptsDatabase", menuName = "Databases/Create receipts database")]
    public class ReceiptsDatabase : DatabaseSO<Receipt>
    {
        
    }
}