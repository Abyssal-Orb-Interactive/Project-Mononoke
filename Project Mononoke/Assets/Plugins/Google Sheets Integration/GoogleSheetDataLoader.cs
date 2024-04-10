using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Source.ItemsModule;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Plugins.GoogleSheetsIntegration
{
    public class GoogleSheetsDataLoader
    {
        private const string DATA_LOADER_CONFIG_ADDRESSABLE_KEY = "Data Loader Config";

        private static GoogleSheetDataLoaderConfig _config = null;

        [MenuItem("Tools/Google Sheets/Update all data")]
        public static async void LoadAsyncAllDataFromGoogleSheet()
        {
            if(_config == null)
            {
                _config = await LoadAssetAsync<GoogleSheetDataLoaderConfig>(DATA_LOADER_CONFIG_ADDRESSABLE_KEY);
                if(EqualityComparer<GoogleSheetDataLoaderConfig>.Default.Equals(_config, null)) 
                {
                    Debug.LogWarning($"Failed to load GoogleSheetDataLoaderConfig on addressable key: {DATA_LOADER_CONFIG_ADDRESSABLE_KEY}");
                    return;
                }
            }
            
            LoadAsyncTrashItemsDataFromGoogleSheet();
        }


        [MenuItem("Tools/Google Sheets/Update Items Data/Trash items")]
        public static async void LoadAsyncTrashItemsDataFromGoogleSheet()
        {
            if(_config == null)
            {
                _config = await LoadAssetAsync<GoogleSheetDataLoaderConfig>(DATA_LOADER_CONFIG_ADDRESSABLE_KEY);
                if(EqualityComparer<GoogleSheetDataLoaderConfig>.Default.Equals(_config, null)) 
                {
                    Debug.LogWarning($"Failed to load GoogleSheetDataLoaderConfig on addressable key: {DATA_LOADER_CONFIG_ADDRESSABLE_KEY}");
                    return;
                }
            }

            var database = await LoadAssetAsync<TrashItemsDatabaseSO>(_config.TrashItemsDatabaseAddressablesKey);
            if(EqualityComparer<TrashItemsDatabaseSO>.Default.Equals(database, null)) 
            {
                Debug.LogWarning($"Failed to load TrashItemsDatabaseSO on addressable key: {_config.TrashItemsDatabaseAddressablesKey}");
                return;
            }

            var rawCvs = await GoogleSheetsCSVLoader.DownloadTableAsync(_config.ItemsTableID, _config.TrashItemsSheetGID);
            var itemsData = await CSVItemsDataProcessor.ProcessData(rawCvs);


            database.ReplaceDatabaseWith(itemsData);
            Debug.Log(database.ToString());
        }

        private static async UniTask<T> LoadAssetAsync<T>(string assetKey)
        {
            return await Addressables.LoadAssetAsync<T>(assetKey);
        }
    }
}
