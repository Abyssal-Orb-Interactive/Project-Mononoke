using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Source.ItemsModule;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

namespace Plugins.GoogleSheetsIntegration
{
    public class ItemDataParser : IGoogleSheetParser
    {
        private const string CULTURE_INFO = "en-US";
        private ItemDataUnsafe _currentData = null;
        private readonly ItemsDatabase _itemsDatabase = null;

        public ItemDataParser(ItemsDatabase itemsDatabase)
        {
            _itemsDatabase = itemsDatabase;
        }


        public async Task Parse(string header, string token)
        {
            switch (header)
            {
                case "ID":
                    if(_currentData != null) break;
                    _currentData = new ItemDataUnsafe(token);
                    break;
                case "Name":
                    if(_currentData == null) break;
                    _currentData.Name = token;
                    break;
                case "Weight":
                    if(_currentData == null) break;
                    if (!TryParseFloatFrom(token, out var weight)) break;
                    _currentData.Weight = weight;
                    break;
                case "Volume":
                    if(_currentData == null) break;
                    if (!TryParseFloatFrom(token, out var volume)) break;
                    _currentData.Volume = volume;
                    break;
                case "Price":
                    if(_currentData == null) break;
                    if (!TryParseFloatFrom(token, out var price)) break;
                    _currentData.Price = price;
                    break;
                case "Durability":
                    if(_currentData == null) break;
                    if (!TryParseFloatFrom(token, out var durability)) break;
                    _currentData.Durability = durability;
                    break;
                case "Stack Capacity":
                    if(_currentData == null) break;
                    if (!TryParseIntFrom(token, out var stackCapacity)) break;
                    _currentData.StackCapacity = stackCapacity;
                    break;
                case "Icon Atlas Addressables Key":
                    if(_currentData == null) break;
                    _currentData.Atlas = await LoadAssetAsync<SpriteAtlas>(token);
                    break;
                case "Icon In Atlas Name":
                    if(_currentData == null) break;
                    _currentData.IconInAtlasName = token;
                    break;
                case "Description":
                    if(_currentData == null) break;
                    _currentData.Description = token;
                    _itemsDatabase.AddOrOverwriteDatabaseItem(_currentData.ToSafe());
                    _currentData = null;
                    break;
                default:
                    throw new Exception($"Invalid header {header}");
            }
        }

        private static bool TryParseIntFrom(string token, out int result)
        {
            if (int.TryParse(token, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.GetCultureInfo(CULTURE_INFO), out result)) return true;
            Debug.LogWarning($"Can't parse float in {token}, wrong cell data");
            return false;
        }

        private static bool TryParseFloatFrom(string token, out float result)
        {
            if (float.TryParse(token, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.GetCultureInfo(CULTURE_INFO), out result)) return true;
            Debug.LogWarning($"Can't parse float in {token}, wrong cell data");
            return false;

        }
        
        private static async UniTask<T> LoadAssetAsync<T>(string assetKey)
        {
            return await Addressables.LoadAssetAsync<T>(assetKey);
        }

        private class ItemDataUnsafe
        {
            public string ID = null;
            public string Name = null;
            public float Weight = 0;
            public float Volume = 0;
            public float Price = 0;
            public float Durability = 0;
            public int StackCapacity = 0;
            public SpriteAtlas Atlas = null;
            public string IconInAtlasName = null;
            public string Description = null;

            public ItemDataUnsafe(string id)
            {
                ID = id;
            }


            public ItemData ToSafe()
            {
                return new ItemData(ID, Name, Weight, Volume, Price, Durability, StackCapacity, IconInAtlasName, Atlas, Description);
            }
        } 
    }
}