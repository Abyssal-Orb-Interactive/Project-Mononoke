using System.Collections.Generic;
using Base.Input;
using Base.Math;
using Source.BuildingModule;
using Source.Character.Movement;
using UnityEngine;
using VContainer;

namespace Source.Formations
{
    public class FormationPositionsHolder : MonoBehaviour
    {
        [SerializeField] private GameObject _positionMarkerPrefab = null;
        private Formation _formation = null;
        private MovementDirection _facing = MovementDirection.North;
        private IsoCharacterMover _followingCharacter = null;
        private List<Transform> _spawnedPositionMarkers = null;
        private OnGridObjectPlacer _objectPlacer = null;

        [Inject]
        public void Initialize(Formation formation, OnGridObjectPlacer objectPlacer, IsoCharacterMover followingCharacter)
        {
            _formation = formation;
            _followingCharacter = followingCharacter;
            _followingCharacter.MovementChanged += RotateFormation;
            _spawnedPositionMarkers = new List<Transform>();
            var positions = _formation.GetFormationPositions();
            _objectPlacer = objectPlacer;
            SpawnMarkersAt(positions);
        }

        private void SpawnMarkersAt(IEnumerable<Vector3> positions)
        {
            foreach (var position in positions)
            {
                var holderWorldPosition = transform.position;
                var holderCartesianPosition =
                    new Vector3Iso(holderWorldPosition.x, holderWorldPosition.y, holderWorldPosition.z).ToCartesian();
                //var rotatedPosition = RotateFormationPosition(position);
                var formationPositionCoordinates = position + holderCartesianPosition;

                _spawnedPositionMarkers.Add(_objectPlacer.PlaceObject(new ObjectPlacementInformation<GameObject>(
                    _positionMarkerPrefab,
                    formationPositionCoordinates, Quaternion.identity, transform)).transform);
            }
        }

        private void RotateFormation(object sender, IsoCharacterMover.MovementActionEventArgs args)
        {
            RelocateFormationOneCellBehindCharacter();
            if(_facing == args.Facing) return;
            _facing = args.Facing;
            foreach (var marker in _spawnedPositionMarkers)
            {
               marker.transform.localPosition = RotateFormationPosition(marker.localPosition);
            }
        }

        private Vector3 RotateFormationPosition(Vector3 position)
        {
            position = DirectionToQuaternionConverter.GetQuaternionFor(_facing) * position;
            return position;
        }
        
        private void RelocateFormationOneCellBehindCharacter()
        {
            var oneIsoVector = DirectionToVector3IsoConverter.ToVector(DirectionReverser.Reverse(_facing));
            transform.localPosition = oneIsoVector;
        }
    }
}