using System.Collections.Generic;
using System.Linq;
using Base.Math;
using Source.BattleSystem;
using Source.Character.AI;
using Source.Character.AI.BattleAI;
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
        private readonly List<InFormationPathfinder> _dispatchQueue = new();
        private int _dispatchUnitIndex = 0;

        private void Update()
        {
            SetFormation();
        }

        private void SetFormation()
        {
            _formationMarkersPositions = _formation.Markers.ToList();
            
            

            for (var i = 0; i < _spawnedUnits.Count; i++)
            {
                if (_spawnedUnits[i].Equals(null)) continue;
                _spawnedUnits[i].AI.StartFollowing(_formationMarkersPositions[i].position, 0.5f);
            }
        }

        public bool TryGetFreePosition(out Transform position)
        {
            if (_spawnedUnits.Count + _dispatchQueue.Count >= _formationMarkersPositions.Count)
            {
                position = null;
                return false;
            }
            
            position = GetFreePosition();
            return true;
        }

        private Transform GetFreePosition()
        {
            return _formationMarkersPositions[_spawnedUnits.Count];
        }

        public bool TryAddToArmy(GameObject unit, Transform formationPositionTransform)
        {
            if (!unit.TryGetComponent<PathfinderAI>(out var ai)) return false;
            AddToFormation(ai, formationPositionTransform);
            return true;
        }

        private void AddToFormation(PathfinderAI AI, Transform formationPositionTransform)
        {
            AI.StopAnalyzingInformationSources();
            var battleAI = AI.GetComponent<BattleAI>();
            battleAI.StopListeningAreasSignals();
            var inFormationUnit = new InFormationPathfinder(AI, formationPositionTransform, _targetPositionCoordinator, battleAI);
            _spawnedUnits.Add(inFormationUnit);
            inFormationUnit.ReturningToFormation += ReturnDispatchedUnitToFormation;
            _dispatchUnitIndex = _spawnedUnits.Count - 1;
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