using System.Collections.Generic;
using System.Linq;
using Base.DataStructuresModule;
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
        private Formation _formation;

        private readonly List<PriorityPathfinder> _spawnedUnits = new();
        private List<Vector3> _formationPositions = null;
        private List<PriorityPathfinder> _dispatchQueue = null;
        private int _dispatchUnitIndex = 0;

        private void Update()
        {
            SetFormation();
        }

        private void SetFormation()
        {
            _formationPositions = _formation.GetFormationPositions().ToList();

            if (_spawnedUnits.Count == 0)
            {
                if (_formationPositions.Count > _spawnedUnits.Count + _dispatchQueue?.Count)
                {
                    var remainingPositions = _formationPositions.Skip(_spawnedUnits.Count);
                    Spawn(remainingPositions);
                }
                else if (_formationPositions.Count < _spawnedUnits.Count + _dispatchQueue?.Count)
                {
                    Kill(_spawnedUnits.Count - _formationPositions.Count);
                }   
            }

            _dispatchQueue = new List<PriorityPathfinder>();
            _dispatchUnitIndex = _spawnedUnits.Count - 1;
            for (var i = 0; i < _spawnedUnits.Count; i++)
            {
                if (_spawnedUnits[i].Equals(null)) continue; 
                var pos = _formationPositions[i];
                var worldPosition = new Vector3Iso(pos);
                var worldPositionVector3 = new Vector3(worldPosition.X, worldPosition.Y, worldPosition.Z);
                _spawnedUnits[i].AI.StartFollowing(worldPositionVector3, 0.5f);
            }
        }

        private void Spawn(IEnumerable<Vector3> positions)
        {
            var index = 0;
            foreach (var position in positions)
            {
                var unit = MinionsFactory.Create(position);
                var ai = unit.GetComponent<PathfinderAI>();
                ai.AddToFormation();
                _spawnedUnits.Add(new PriorityPathfinder(ai, index));
                index++;
            }
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

        public void DispatchUnit()
        {
            _dispatchQueue.Add(_spawnedUnits[_dispatchUnitIndex]);
            _spawnedUnits.RemoveAt(_dispatchUnitIndex);
            _dispatchQueue.Last().AI.StartListeningTargetChanging();
            _dispatchQueue.Last().AI.StartListeningColliders();
            _dispatchUnitIndex--;
        }
    }
}