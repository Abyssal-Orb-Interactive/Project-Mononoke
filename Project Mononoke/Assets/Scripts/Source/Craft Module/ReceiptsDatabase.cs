using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Source.Craft_Module
{
    [CreateAssetMenu(fileName = "ReceiptsDatabase", menuName = "Databases/Create receipts database")]
    public class ReceiptsDatabase : ScriptableObject
    {
        [SerializeField] private List<Receipt> _savedData = new();

        private Dictionary<string, Receipt> _database = new();
    }
}