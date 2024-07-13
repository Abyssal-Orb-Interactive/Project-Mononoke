using Base.TileMap;
using NSubstitute;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure.TileMapAnalyzer
{
    public static partial class Create
    {
        public static ITileCollection TileMapSubstitute()
        {
            return Substitute.For<ITileCollection>();
        }
        public static Vector3Int Vector3IntWith(int x, int y, int z)
        {
            return new Vector3Int(x,y,z);
        }

        public static TileCollectionAnalyzer TileMapAnalyzerWith(ITileCollection tileMap)
        {
            return new TileCollectionAnalyzer(tileMap);
        }
    }
}