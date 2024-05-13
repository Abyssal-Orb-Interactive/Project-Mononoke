using System;
using System.Collections.Generic;
using System.Linq;
using Base.DataStructuresModule;
using Base.Input;
using Base.Input.Actions;
using Base.Math;
using Cysharp.Threading.Tasks;
using Source.Character.AI;
using Source.Character.Minions_Manager;
using Source.Character.Movement;
using UnityEngine;

namespace Source.Formations
{
    public class TestArmy : MonoBehaviour
    {
        [SerializeField] private FormationPositionsHolder _formation;

        private readonly List<PriorityPathfinder> _spawnedUnits = new();
        private List<Transform> _formationMarkersPositions = null;
        private List<PriorityPathfinder> _dispatchQueue = new();
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
            var index = 0;
            foreach (var transform in transforms)
            {
                var worldPos = transform.position;
                var worldCartesianPos = new Vector3Iso(worldPos.x, worldPos.y, worldPos.z).ToCartesian();
                var unit = MinionsFactory.Create(worldCartesianPos);
                var ai = unit.GetComponent<PathfinderAI>();
                ai.AddToFormation();
                _spawnedUnits.Add(new PriorityPathfinder(ai, index));
                index++;
            }

            _dispatchUnitIndex = index - 1;
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
            if(_dispatchUnitIndex < 0) return;
            _dispatchQueue.Add(_spawnedUnits[_dispatchUnitIndex]);
            _spawnedUnits.RemoveAt(_dispatchUnitIndex);
            _dispatchQueue.Last().AI.RemoveFromFormation();
            _dispatchQueue.Last().AI.MovementCancelled += AIOnMovementCancelled;
            _dispatchUnitIndex--;
        }

        private void AIOnMovementCancelled(MovementInputEventArgs args)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.005f), cancellationToken: _cancellationTokenSource.Token);
        }
    }
}