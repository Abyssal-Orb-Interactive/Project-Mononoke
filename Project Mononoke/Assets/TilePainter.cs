using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
   [SerializeField] private Tilemap _tilemap;
   [SerializeField] private Vector3Int _pos;
   [SerializeField] private Tile _brush;

   [ContextMenu("Paint")]
   private void Paint(){
    _tilemap.SetTile(_pos, _brush);
   }
}
