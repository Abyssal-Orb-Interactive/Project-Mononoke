using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.IO;

namespace Plugins.GoogleSheetsIntegration
{
    public static class GoogleSheetsCSVLoader
    {
        private const string URLFormat = "https://docs.google.com/spreadsheets/d/{TableID}/export?format=csv&gid={SheetGID}";

        public static async UniTask<string> DownloadTableAsync(string tableID, string sheetGID = "")
        {
            var url = URLFormat.Replace("{TableID}", tableID);
            if(string.IsNullOrEmpty(sheetGID)) url = url.Replace("&gid={SheetGID}", "");
            else url = url.Replace("{SheetGID}", sheetGID);

            return await DownloadRawCSVTableAsync(url);
        }

        private static async UniTask<string> DownloadRawCSVTableAsync(string url)
        {
            using var unityWebRequest = UnityWebRequest.Get(url);

            var asyncOperation = await unityWebRequest.SendWebRequest();

            await UniTask.WaitUntil(() => asyncOperation.isDone);

            if (unityWebRequest.result == UnityWebRequest.Result.Success) return unityWebRequest.downloadHandler.text;
            else throw new IOException($"Error loading CSV from URL: {url}\nError: {unityWebRequest.error}");
        }
    }
}

