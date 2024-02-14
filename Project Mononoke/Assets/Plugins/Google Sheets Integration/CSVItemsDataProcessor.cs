using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using static Source.ItemsModule.TrashItemsDatabaseSO;
using UnityEngine.AddressableAssets;

namespace Plugins.GoogleSheetsIntegration
{
    public static class CSVItemsDataProcessor
    {
        private const string CULTURE_INFO = "en-US";

        private const int ID_COLUMN_INDEX = 0;
        private const int NAME_COLUMN_INDEX = 1;
        private const int WEIGHT_COLUMN_INDEX = 3;
        private const int VOLUME_COLUMN_INDEX = 4;
        private const int PRICE_COLUMN_INDEX = 5;
        private const int DURABILITY_COLUMN_INDEX = 6;
        private const int STACK_CAPACITY_COLUMN_INDEX = 7;
        private const int ICON_ATLAS_ADDRESSABLES_KEY_COLUMN_INDEX = 8;
        private const int ICON_IN_ATLAS_NAMR_COLUMN_INDEX = 9;
        private const int DESCRIPTION_COLUMN_INDEX = 10;


        private const char CELL_SEPARATOR = ',';
        public static async UniTask<List<ItemData>> ProcessData(string cvsRawData)
        {
            var lineEnding = GetPlatformSpecificLineEnd();
            string[] rows = cvsRawData.Split(lineEnding);
            var rawDataStartIndex = 1;
            var itemsData = new List<ItemData>();
            ItemData data;
            for (var i = rawDataStartIndex; i < rows.Length; i++)
            {
                string[] cells = rows[i].Split(CELL_SEPARATOR);
            
                if(!TryParseInt(cells[ID_COLUMN_INDEX],out int id)) break;

                var name = cells[NAME_COLUMN_INDEX];

                if(!TryParseFloat(cells[WEIGHT_COLUMN_INDEX], out float weight)) break; 
                if(!TryParseFloat(cells[VOLUME_COLUMN_INDEX], out float volume)) break;
                if(!TryParseFloat(cells[PRICE_COLUMN_INDEX], out float price)) break;
                if(!TryParseFloat(cells[DURABILITY_COLUMN_INDEX], out float durability)) break;
                if(!TryParseInt(cells[STACK_CAPACITY_COLUMN_INDEX], out int stackCapacity)) break;

                var atlasAddressablesKey = cells[ICON_ATLAS_ADDRESSABLES_KEY_COLUMN_INDEX];
                var atlas = await LoadAssetAsync<SpriteAtlas>(atlasAddressablesKey);

                if(EqualityComparer<SpriteAtlas>.Default.Equals(atlas, default)) break;

                
                var iconInAtlasName = cells[ICON_IN_ATLAS_NAMR_COLUMN_INDEX];

                var description = cells[DESCRIPTION_COLUMN_INDEX];

                data = new ItemData(id, name, weight, volume, price, durability, stackCapacity, iconInAtlasName, atlas, description);
                itemsData.Add(data);
            }
            return itemsData;

        }

        private static async UniTask<T> LoadAssetAsync<T>(string assetKey)
        {
            return await Addressables.LoadAssetAsync<T>(assetKey);
        }

        private static bool TryParseInt(string intString, out int result)
        {   
            result = -1;
            if (!int.TryParse(intString, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.GetCultureInfo(CULTURE_INFO), out result))
            {
                Debug.LogWarning($"Can't parse int in {intString}, wrong cell data");
                return false;
            }

            return true;
        }
    
        private static bool TryParseFloat(string floatString, out float result)
        {
            result = -1;
            if (!float.TryParse(floatString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo(CULTURE_INFO), out result))
            {
                Debug.LogWarning($"Can't pars float in {floatString},wrong cell data");
                return false;
            }

            result = (float)Math.Round(result, 2);

            return true;
        }
    
        private static char GetPlatformSpecificLineEnd()
        {
            char lineEnding = '\n';
            #if UNITY_IOS
            lineEnding = '\r';
            #endif
            return lineEnding;
        }
    }
}
