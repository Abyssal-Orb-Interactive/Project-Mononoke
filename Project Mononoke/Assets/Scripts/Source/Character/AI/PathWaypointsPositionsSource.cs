using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Pathfinding;
using UnityEngine;

namespace Source.Character.AI
{
    public class PathWaypointsPositionsSource
    {
        private readonly Seeker _pathBuilder = null;
        
        public PathWaypointsPositionsSource(Seeker seeker)
        {
            _pathBuilder = seeker;
        }

        public async UniTask<IReadOnlyList<Vector3>> GetWaypointsFor(Vector3 startPosition, Vector3 endPosition)
        {
            var path = await BuildPath(startPosition, endPosition);
            return AreAnyErrorsIn(path) ? new List<Vector3>() : path.vectorPath;
        }

        private async UniTask<Path> BuildPath(Vector3 startPosition, Vector3 endPosition)
        {
            return await new UniTask<Path>(_pathBuilder.StartPath(startPosition, endPosition));
        }

        private bool AreAnyErrorsIn(Path path)
        {
            return path.error;
        }
    }
}