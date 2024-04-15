using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Character.AI
{
    [RequireComponent(typeof(Seeker))]
    public class PathProcessor : MonoBehaviour
    {
        [SerializeField] private Seeker _pathBuilder = null;
        [SerializeField] private Transform _target = null;
        [SerializeField] private float _pathNodeProximityThreshold = 3f;
        
        private int _currentPathWaypointIndex = 0;
        private Path _path = null;
        private void OnValidate()
        {
            _pathBuilder ??= GetComponent<Seeker>();
        }

        private async UniTask<Path> BuildPath()
        {
            return await new UniTask<Path>(_pathBuilder.StartPath(transform.position, _target.position, OnPathBuilt));
        }

        private void OnPathBuilt(Path path)
        {
            if (path.error) return;
            _path = path;
            _currentPathWaypointIndex = 0;
        }

        public async void TraversePath()
        {
            if (_path == null)
            {
                await BuildPath();
            }

            var waypointsPositions = _path.vectorPath;

            while (CalculateDistanceTo(waypointsPositions[_currentPathWaypointIndex]) < _pathNodeProximityThreshold)
            {
                if (_currentPathWaypointIndex + 1 < waypointsPositions.Count)
                {
                    _currentPathWaypointIndex++;
                }
                else
                {
                    break;
                }
                
            }
        }

        private Vector3 CalculateDirectionToCurrentWaypoint()
        {
            return (_path.vectorPath[_currentPathWaypointIndex] - transform.position).normalized;
        }

        private float CalculateDistanceTo(Vector3 position)
        {
            return Vector3.Distance(transform.position, position);
        }
    }
}