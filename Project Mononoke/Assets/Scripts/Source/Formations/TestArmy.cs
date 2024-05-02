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
               var pos = _formationPositions[i];
                _spawnedUnits[i].StartFollowing(pos, 0.5f);
            }
        }

        private void Spawn(IEnumerable<Vector3> positions)
        {
            foreach (var position in positions)
            {
                var unit = MinionsFactory.Create(position);
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