using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEngine;

namespace Plugins.GoogleSheetsIntegration
{
    public class GoogleSheetsImporter
    {
        private readonly SheetsService _service = null;
        private readonly string _spreadsheetID = null;
        private readonly List<string> _headers = new();

        public GoogleSheetsImporter(string credentialsPath, string spreadsheetID)
        {
            _spreadsheetID = spreadsheetID;
            GoogleCredential credential = null;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);
            }
            
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }

        public async Task DownloadAndParseSheet(string sheetName, IGoogleSheetParser parser)
        {
            var response = await DownloadTable(sheetName);
            TryProcessResponse(sheetName, parser, response);
        }

        private async UniTask<ValueRange> DownloadTable(string sheetName)
        {
            var range = PrepareRequestInputData(sheetName);
            var request = MakeRequestWith(range);
            var response = await GetResponse(request);
            return response;
        }

        private string PrepareRequestInputData(string sheetName)
        {
            Debug.Log($"Starting downloading sheet (${sheetName})...");
            var range = $"{sheetName}!A1:Z";
            return range;
        }

        private SpreadsheetsResource.ValuesResource.GetRequest MakeRequestWith(string range)
        {
            return _service.Spreadsheets.Values.Get(_spreadsheetID, range);
        }

        private static async UniTask<ValueRange> GetResponse(SpreadsheetsResource.ValuesResource.GetRequest request)
        {
            return await TryWaitRequest(request);
        }

        private static async UniTask<ValueRange> TryWaitRequest(SpreadsheetsResource.ValuesResource.GetRequest request)
        {
            ValueRange response = null;
            try
            {
                response = await request.ExecuteAsync();
            }
            catch (Exception e)
            {
                LogRequestError(e);
            }

            return response;
        }

        private static void LogRequestError(Exception e)
        {
            Debug.LogError($"Error retrieving Google Sheets data: {e.Message}");
        }

        private bool TryProcessResponse(string sheetName, IGoogleSheetParser parser, ValueRange response)
        {
            if (CheckCorrectnessOf(response))
            {
                ProcessResponse(sheetName, parser, response);
                return true;
            }
            
            LogResponseWarning();
            return false;
        }

        private bool CheckCorrectnessOf(ValueRange response)
        {
            return response != null && response.Values != null;
        }

        private void ProcessResponse(string sheetName, IGoogleSheetParser parser, ValueRange response)
        {
            var table = response.Values;
            Debug.Log($"Sheet downloaded successfully: {sheetName}. Parsing started.");
            ParseTable(parser, table);
            Debug.Log($"Sheet parsed successfully.");
        }

        private void ParseTable(IGoogleSheetParser parser, IList<IList<object>> table)
        {
            ParseHeaders(table);
            ParseCells(parser, table);
        }

        private void ParseHeaders(IList<IList<object>> table)
        {
            var firstRow = table[0];
            foreach (var cell in firstRow)
            {
                _headers.Add(cell.ToString());
            }
        }

        private void ParseCells(IGoogleSheetParser parser, IList<IList<object>> table)
        {
            ParseAllRows(parser, table);
        }

        private void ParseAllRows(IGoogleSheetParser parser, IList<IList<object>> table)
        {
            var rowsCount = table.Count;
            for (var i = 1; i < rowsCount; i++)
            {
                ParseRow(parser, table, i);
            }
        }

        private void ParseRow(IGoogleSheetParser parser, IList<IList<object>> table, int i)
        {
            var row = table[i];
            var rowLenght = row.Count;
            ParseAllCellsInRow(parser, rowLenght, row);
        }

        private void ParseAllCellsInRow(IGoogleSheetParser parser, int rowLenght, IList<object> row)
        {
            for (var j = 0; j < rowLenght; j++)
            {
                ParseCell(parser, row, j);
            }
        }

        private void ParseCell(IGoogleSheetParser parser, IList<object> row, int j)
        {
            var cell = row[j];
            var header = _headers[j];
            parser.Parse(header, cell.ToString());
        }
        
        private void LogResponseWarning()
        {
            Debug.LogWarning("No data found in Google Sheets.");
        }
    }
}