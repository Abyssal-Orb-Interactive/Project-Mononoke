using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Source.ItemsModule;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Plugins.GoogleSheetsIntegration
{
    [InitializeOnLoad]
    public class GoogleSheetsDataLoader
    {
        private const string DATA_LOADER_CONFIG_ADDRESSABLES_KEY = "Data Loader Config";

        private static GoogleSheetDataLoaderConfig _config = null;

        [MenuItem("Tools/Google Sheets/Update all data")]
        public static async void LoadAsyncAllDataFromGoogleSheet()
        {
            if(_config == null)
            {
                _config = await LoadAssetAsync<GoogleSheetDataLoaderConfig>(DATA_LOADER_CONFIG_ADDRESSABLES_KEY);
                if(EqualityComparer<GoogleSheetDataLoaderConfig>.Default.Equals(_config, null)) 
                {
                    Debug.LogWarning($"Failed to load GoogleSheetDataLoaderConfig on addresables key: {DATA_LOADER_CONFIG_ADDRESSABLES_KEY}");
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
                _config = await LoadAssetAsync<GoogleSheetDataLoaderConfig>(DATA_LOADER_CONFIG_ADDRESSABLES_KEY);
                if(EqualityComparer<GoogleSheetDataLoaderConfig>.Default.Equals(_config, null)) 
                {
                    Debug.LogWarning($"Failed to load GoogleSheetDataLoaderConfig on addresables key: {DATA_LOADER_CONFIG_ADDRESSABLES_KEY}");
                    return;
                }
            }

            TrashItemsDatabaseSO database = await LoadAssetAsync<TrashItemsDatabaseSO>(_config.TrashItemsDatabaseAddressablesKey);
            if(EqualityComparer<TrashItemsDatabaseSO>.Default.Equals(database, null)) 
            {
                Debug.LogWarning($"Failed to load TrashItemsDatabaseSO on addresables key: {_config.TrashItemsDatabaseAddressablesKey}");
                return;
            }

            var rawCVS = await GoogleSheetsCSVLoader.DownloadTableAsync(_config.ItemsTableID, _config.TrashItemsSheetGID);
            var itemsData = await CSVItemsDataProcessor.ProcessData(rawCVS);


            database.LoadDatabase(itemsData);
            Debug.Log(database.ToString());
        }

        private static async UniTask<T> LoadAssetAsync<T>(string assetKey)
        {
            return await Addressables.LoadAssetAsync<T>(assetKey);
        }
    }
}
