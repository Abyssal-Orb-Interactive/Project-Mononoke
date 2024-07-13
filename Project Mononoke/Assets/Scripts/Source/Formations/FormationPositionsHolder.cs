using System.Collections.Generic;
using System.Linq;
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
        private List<Vector3> _positions = null;

        public IEnumerable<Transform> Markers => _spawnedPositionMarkers;


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
            _positions = positions.ToList();
            foreach (var position in _positions)
            {
                var holderWorldPosition = transform.position;
                var holderCartesianPosition = 
                    new Vector3Iso(holderWorldPosition.x, holderWorldPosition.y, holderWorldPosition.z).ToCartesian();
                var rotatedPosition = RotateFormationPosition(position);
                var formationPositionCoordinates = rotatedPosition + holderCartesianPosition;

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
            var rotationMatrix = Matrix4x4.Rotate(DirectionToQuaternionConverter.GetQuaternionFor(_facing));
            for (var i = 0; i < _positions.Count; i++)
            {
                var rotatedPosition = rotationMatrix.MultiplyPoint(_positions[i]);
                var isoRotatedPosition = new Vector3Iso(rotatedPosition);
                var localPosition = new Vector3(isoRotatedPosition.X, isoRotatedPosition.Y, isoRotatedPosition.Z);
                _spawnedPositionMarkers[i].transform.localPosition = localPosition;
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