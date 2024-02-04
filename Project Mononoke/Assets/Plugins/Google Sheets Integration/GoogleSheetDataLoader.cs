using Source.ItemsModule;
using UnityEditor;
using UnityEngine;

namespace Plugins.GoogleSheetsIntegration
{
    [InitializeOnLoad]
    public class GoogleSheetsDataLoader
    {
        private const string DATA_LOADER_CONFIG_PATH = "Assets/Plugins/Google Sheets Integration/Data Loader Config.asset";

        private static GoogleSheetDataLoaderConfig _config = null;

        [MenuItem("Tools/Google Sheets/Update all data")]
        public static void LoadAsyncAllDataFromGoogleSheet()
        {
            if(_config == null)
            {
                if(!TryLoadConfigUsing(DATA_LOADER_CONFIG_PATH, out _config)) return;
            }
            
            LoadAsyncTrashItemsDataFromGoogleSheet();
        }


        [MenuItem("Tools/Google Sheets/Update Items Data/Trash items")]
        public static async void LoadAsyncTrashItemsDataFromGoogleSheet()
        {
            if(_config == null)
            {
                if(!TryLoadConfigUsing(DATA_LOADER_CONFIG_PATH, out _config)) return;
            }
            if(!TryLoadDatabaseUsing(_config.TrashItemsDatabasePath, out TrashItemsDatabaseSO database))  return;

            var rawCVS = await GoogleSheetsCSVLoader.DownloadTableAsync(_config.ItemsTableID, _config.TrashItemsSheetGID);
            var itemsData = CSVItemsDataProcessor.ProcessData(rawCVS);
            database.AddOrOverwriteItemsData(itemsData);
            Debug.Log(database.ToString());
        }

        private static bool TryLoadDatabaseUsing(string path, out TrashItemsDatabaseSO database)
        {
            database = AssetDatabase.LoadAssetAtPath<TrashItemsDatabaseSO>(path);

            if(database == null) 
            {
                Debug.LogWarning($"Failed to load ItemsDatabaseSO on path: {path}");
                return false;
            }

            return true;
        }

        private static bool TryLoadConfigUsing(string path, out GoogleSheetDataLoaderConfig database)
        {
            database = AssetDatabase.LoadAssetAtPath<GoogleSheetDataLoaderConfig>(path);

            if(database == null) 
            {
                Debug.LogWarning($"Failed to load GoogleSheetDataLoaderConfig on path: {path}");
                return false;
            }

            return true;
        }

        static GoogleSheetsDataLoader()
        {
            if(!TryLoadConfigUsing(DATA_LOADER_CONFIG_PATH, out _config)) return;

            LoadAsyncAllDataFromGoogleSheet();
        }
    }
}
