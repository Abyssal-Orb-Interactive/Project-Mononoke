using System.Collections.Generic;
using System.Linq;
using Base.Math;
using Source.Character.AI;
using Source.Character.Minions_Manager;
using UnityEngine;

namespace Source.Formations
{
    public class TestArmy : MonoBehaviour
    {
        [SerializeField] private FormationPositionsHolder _formation;
        [SerializeField] private MinionsTargetPositionCoordinator _targetPositionCoordinator = null;

        private readonly List<InFormationPathfinder> _spawnedUnits = new();
        private List<Transform> _formationMarkersPositions = null;
        private List<InFormationPathfinder> _dispatchQueue = new();
        private int _dispatchUnitIndex = 0;

        private void Update()
        {
            SetFormation();
        }

        private void SetFormation()
        {
            _formationMarkersPositions = _formation.Markers.ToList();

            if (_spawnedUnits.Count == 0)
            {
                if (_formationMarkersPositions.Count > _spawnedUnits.Count + _dispatchQueue?.Count)
                {
                    var remainingTransforms = _formationMarkersPositions.Skip(_spawnedUnits.Count);
                    Spawn(remainingTransforms);
                }
                else if (_formationMarkersPositions.Count < _spawnedUnits.Count + _dispatchQueue?.Count)
                {
                    Kill(_spawnedUnits.Count - _formationMarkersPositions.Count);
                }   
            }
            
            for (var i = 0; i < _spawnedUnits.Count; i++)
            {
                if (_spawnedUnits[i].Equals(null)) continue;
                _spawnedUnits[i].AI.StartFollowing(_formationMarkersPositions[i].position, 0.5f);
            }
        }

        private void Spawn(IEnumerable<Transform> transforms)
        {
            var transformsList = transforms.ToList();
            foreach (var formationPositionTransform in transformsList)
            {
                var worldPos = formationPositionTransform.position;
                var worldCartesianPos = new Vector3Iso(worldPos.x, worldPos.y, worldPos.z).ToCartesian();
                var unit = MinionsFactory.Create(worldCartesianPos);
                var ai = unit.GetComponent<PathfinderAI>();
                ai.StopAnalyzingInformationSources();
                var inFormationUnit = new InFormationPathfinder(ai, formationPositionTransform, _targetPositionCoordinator);
                _spawnedUnits.Add(inFormationUnit);
                inFormationUnit.ReturningToFormation += ReturnDispatchedUnitToFormation;
                
            }

            _dispatchUnitIndex = transformsList.Count() - 1;
        }

        private void ReturnDispatchedUnitToFormation(InFormationPathfinder unit)
        {
            _dispatchQueue.Remove(unit);
            _spawnedUnits.Add(unit);
            _dispatchUnitIndex++;
        }

        private void Kill(int num)
        {
            for (var i = 0; i < num; i++)
            {
                var unit = _spawnedUnits.Last();
                _spawnedUnits.Remove(unit);
                Destroy(unit.AI.gameObject);
            }
        }

        public void ReturnAllDispatchedUnitsToFormation()
        {
            foreach (var unit in _dispatchQueue)
            {
                unit.ReturnToFormation();
            }
        }

        public void DispatchUnit()
        {
            if(_dispatchUnitIndex < 0) return;
            _dispatchQueue.Add(_spawnedUnits[_dispatchUnitIndex]);
            _spawnedUnits.RemoveAt(_dispatchUnitIndex);
            _dispatchQueue.Last().Dispatch();
            _dispatchUnitIndex--;
        }
    }
}