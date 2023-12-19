using System;
using Base.Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Source
{
    [RequireComponent(typeof(Tilemap))]
    public class Ground : MonoBehaviour
    {
        [SerializeField] private Tilemap _groundTileMap = null;

        private GroundGrid _grid = null;

        private void OnValidate()
        {
            _groundTileMap ??= GetComponent<Tilemap>();
            _grid = new GroundGrid(new TileMapAnalyzer(_groundTileMap));
        }
    }
}