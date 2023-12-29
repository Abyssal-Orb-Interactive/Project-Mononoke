using Base.Grid;
using Base.TileMap;
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
            _grid = new GroundGrid(new TileCollectionAnalyzer(new UnityTileMapWrapper(_groundTileMap)));
        }

        private void Start()
        {
            Debug.Log(_grid.GetCellAt(new Vector3Int(0,0)).Type);
            Debug.Log(_grid.GetCellAt(new Vector3Int(-2,0, -1)).Type);
        }
    }
}