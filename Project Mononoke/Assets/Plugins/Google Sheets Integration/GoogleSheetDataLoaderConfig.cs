using System;
using UnityEngine;

namespace Plugins.GoogleSheetsIntegration
{
    [CreateAssetMenu(fileName = "Data Loader Config", menuName = "Plugins/Google Sheet")]
    public class GoogleSheetDataLoaderConfig : ScriptableObject
    {
        [SerializeField] private string _itemsTableID = "1y-Y10kbAhhNJPotL3F-C9Cnr3EpGxSy3SafOiHXN72k";
        public string ItemsTableID => _itemsTableID;
        [SerializeField] private string _trashItemsSheetGID = "0";
        public string TrashItemsSheetGID => _trashItemsSheetGID;
        [SerializeField] private string _trashItemsDatabaseAddressablesKey = "Trash Items Database";
        public string TrashItemsDatabaseAddressablesKey => _trashItemsDatabaseAddressablesKey;
    }
}
