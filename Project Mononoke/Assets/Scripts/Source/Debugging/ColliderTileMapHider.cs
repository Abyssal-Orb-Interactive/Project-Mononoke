using UnityEngine;
using UnityEngine.Tilemaps;

namespace Source.Debugging
{
    [RequireComponent(typeof(TilemapRenderer))]
    public class ColliderTileMapHider : MonoBehaviour
    {
        [SerializeField] private bool _show = false;
        private TilemapRenderer _renderer = null;

        private void OnValidate()
        {
            _renderer ??= GetComponent<TilemapRenderer>();
            SwitchVisualizationMode();
        }

        private void SwitchVisualizationMode()
        {
            _renderer.enabled = _show;
        }
    }
}
