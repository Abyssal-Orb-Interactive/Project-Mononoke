using Cysharp.Threading.Tasks;
using Source.ItemsModule;
using UnityEditor;
using UnityEngine.AddressableAssets;

namespace Plugins.GoogleSheetsIntegration
{
    public class ConfigImportsMenu
    {
        private const string SPREADSHEET_ID = "1y-Y10kbAhhNJPotL3F-C9Cnr3EpGxSy3SafOiHXN72k";
        private const string CREDENTIALS_PATH = "project-mononoke-426109-9bca86778b0d.json";
        private const string SHEET_NAME = "Items";
        private const string DATABASE_PATH = "Trash Items Database";

        [MenuItem("GoogleSheets/Import item data")]
        private static async void LoadItemsData()
        {
            var sheetImporter = new GoogleSheetsImporter(CREDENTIALS_PATH, SPREADSHEET_ID);
            var database = await LoadAssetAsync<ItemsDatabase>(DATABASE_PATH);
            await sheetImporter.DownloadAndParseSheet(SHEET_NAME, new ItemDataParser(database));
        }
        
        private static async UniTask<T> LoadAssetAsync<T>(string assetKey)
        {
            return await Addressables.LoadAssetAsync<T>(assetKey);
        }
    }
}