using System;
using Base.Grid;
using Base.Input;
using Base.Math;
using Cysharp.Threading.Tasks;
using Source.Character.Audio;
using Source.Character.Movement;
using Source.PickUpModule;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.ItemsModule
{
    public class ItemLauncher : MonoBehaviour
    {
        private GroundGrid _grid = null;
        private ItemView _droppingItem = null;

        public void Initialize(GroundGrid grid)
        {
            _grid = grid;
        }

        public Vector3 DropAndGetEndingDropPosition(Item item)
        {
            
            var worldPos = gameObject.GetComponentInChildren<SpriteRenderer>().transform.position;
            var cartesianWorldPosition = new Vector3Iso(worldPos.x, worldPos.y, worldPos.z).ToCartesian();
            var endingDropPosition = CalculateInGridEndingDropPosition(worldPos, out var inGridEndingPosition);
            while (!_grid.IsCellPassableAt(inGridEndingPosition))
            {
                endingDropPosition = CalculateInGridEndingDropPosition(transform.position, out inGridEndingPosition);
            }
            var itemView =  ItemViewFabric.Create(item, cartesianWorldPosition);
            var itemLauncher = itemView.GetComponent<ParabolicMotionAnimationPlayer>();
            itemLauncher.PlayAnimationBetween(worldPos, endingDropPosition);
            return endingDropPosition;
        }
        

        private Vector3 CalculateInGridEndingDropPosition(Vector3 worldPos, out Vector3Int inGridEndingPosition)
        {
            var endingDropPosition = CalculateEndingDropPosition(worldPos);
            inGridEndingPosition = _grid.WorldToGrid(endingDropPosition);
            return endingDropPosition;
        }

        private static Vector3 CalculateEndingDropPosition(Vector3 worldPos)
        {
            var randomDirection = Random.Range(0, 6);
            var isoOneDirectionVector = DirectionToVector3IsoConverter.ToVector((MovementDirection)randomDirection);
            var endingDropPosition = worldPos + isoOneDirectionVector;
            return endingDropPosition;
        }

        public void Launch(Item item, Vector3 targetPosition)
        {
            var worldPos = transform.position;
            var cartesianWorldPosition = new Vector3Iso(worldPos.x, worldPos.y, worldPos.z).ToCartesian();
            var itemView =  ItemViewFabric.Create(item, cartesianWorldPosition);
            itemView.GetComponent<ParabolicMotionAnimationPlayer>().PlayAnimationBetween(worldPos, targetPosition);
        }

        public void Launch(ItemView item, Vector3 targetPosition)
        {
            item.GetComponent<ParabolicMotionAnimationPlayer>().PlayAnimationBetween(transform.position, targetPosition);
        }
    }
}