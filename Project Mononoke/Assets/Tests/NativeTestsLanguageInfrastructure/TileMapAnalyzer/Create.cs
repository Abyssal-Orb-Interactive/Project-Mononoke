using Base.TileMap;
using NSubstitute;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Create
    {
        public static ITileMapSource TileMapSubstitute()
        {
            return Substitute.For<ITileMapSource>();
        }
        public static Vector3Int Vector3IntWith(int x, int y, int z)
        {
            return new Vector3Int(x,y,z);
        }

        public static TileMapAnalyzer TileMapAnalyzerWith(ITileMapSource tileMap)
        {
            return new TileMapAnalyzer(tileMap);
        }
    }
}