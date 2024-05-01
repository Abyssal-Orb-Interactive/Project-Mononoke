using System.Collections.Generic;
using System.Linq;
using Base.Input;
using Base.Math;
using Source.Character.AI;
using Source.Character.Minions_Manager;
using Source.Character.Movement;
using UnityEngine;

namespace Source.Formations
{
    public class TestArmy : MonoBehaviour
    {
        [SerializeField] private PathfinderAI _unitPrefab = null;
        [SerializeField] private float _unitSpeed = 2f;
        [SerializeField]private Formation _formation;

        private readonly List<PathfinderAI> _spawnedUnits = new();
        private List<Vector3> _formationPositions = null;
        private Transform _parent = null;

        private void Awake()
        {
            _parent = new GameObject("Army").transform;
        }

        private void Update()
        {
            SetFormation();
        }

        private void SetFormation()
        {
            _formationPositions = _formation.GetFormationPositions().ToList();

            if (_formationPositions.Count > _spawnedUnits.Count)
            {
                var remainingPositions = _formationPositions.Skip(_spawnedUnits.Count);
                Spawn(remainingPositions);
            }
            else if (_formationPositions.Count < _spawnedUnits.Count)
            {
                Kill(_spawnedUnits.Count - _formationPositions.Count);
            }
            
            for (var i = 0; i < _spawnedUnits.Count; i++)
            {
                var isometricFormationPosition = new Vector3Iso(_formationPositions[i]);
                var pos = new Vector3(isometricFormationPosition.X, isometricFormationPosition.Y,
                    isometricFormationPosition.Z);
                _spawnedUnits[i].StartFollowing(pos);
            }
        }

        private void NewMethod(int i)
        {
            var position = _spawnedUnits[i].transform.position;
            var directionVector = _formationPositions[i] - position;
            var isometricDirection = new Vector3Iso(directionVector.x, directionVector.y, directionVector.z);
            var cartesianDirection = isometricDirection.ToCartesian();
            var cartesianMovementDirection = cartesianDirection.normalized;
            var direction = InputVectorToDirectionConverter.GetMovementDirectionFor(cartesianMovementDirection);
            Debug.Log(direction);
            
        }

        private void Spawn(IEnumerable<Vector3> positions)
        {
            foreach (var position in positions)
            {
                var unit = MinionsFactory.Create();
                _spawnedUnits.Add(unit.GetComponent<PathfinderAI>());
            }
        }

        private void Kill(int num)
        {
            for (var i = 0; i < num; i++)
            {
                var unit = _spawnedUnits.Last();
                _spawnedUnits.Remove(unit);
                Destroy(unit.gameObject);
            }
        }
    }
}