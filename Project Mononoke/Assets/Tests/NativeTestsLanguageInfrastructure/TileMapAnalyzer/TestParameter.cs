using Base.Grid;
using Base.TileMap;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure.TileMapAnalyzer
{
    public static partial class TestParameter
    {
        public static Vector3 CellPosition = Vector3.zero;
        public static CellType ExpectedCellType = CellType.Air;
        public static ITileCollection TileMapSubstitute = null;
        public static ITile TileSubstitute = null;
        public static Vector3Int TilePosition = Vector3Int.zero;
        public static TileCollectionAnalyzer TileCollectionAnalyzer = null;
        public static string TileName = null;
    }
}