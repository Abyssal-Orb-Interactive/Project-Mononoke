using System.Linq;
using Pathfinding;
using UnityEngine;

namespace Base.Grid
{
    public class GridGraphNodesWalkableUpdater
    {
        private readonly GridGraph _gridGraph = null;
        private readonly float _edgeThreshold = 0.1f;
        private readonly uint _penaltyAmount = 1000;
        private readonly float _tileSize = 1f;


        public GridGraphNodesWalkableUpdater()
        {
            _gridGraph = AstarPath.active.data.gridGraph;
        }

        public void UpdateGridGraphUsing(GroundGrid grid)
        {
            foreach (var node in _gridGraph.nodes)
            {
                var vectorNodePose = (Vector3)node.position;
                var inGridPose = grid.WorldToGrid(vectorNodePose);

                AstarPath.active.AddWorkItem(new AstarWorkItem(() =>
                {
                    if (!grid.IsCellPassableAt((inGridPose)))
                    {
                        node.Walkable = false;
                        return;
                    }

                    if (HasUnpassableNeighbor(node, grid))
                    {
                        node.Penalty += _penaltyAmount;
                    }
                })); 
            }
        }

        private bool HasUnpassableNeighbor(GraphNode node, GroundGrid grid)
        {
            var nodePosition = (Vector3)node.position;
            var inGridPose = grid.WorldToGrid(nodePosition);
            
            var directions = new[]
            {
                new Vector3Int(-1, 0), // Left
                new Vector3Int(1, 0),  // Right
                new Vector3Int(0, -1), // Down
                new Vector3Int(0, 1),  // Up
                new Vector3Int(-1, -1), // Bottom Left
                new Vector3Int(1, -1), // Bottom Right
                new Vector3Int(-1, 1), // Top Left
                new Vector3Int(1, 1), // Top Right
            };


            return directions.Select(direction => inGridPose + direction).Any(neighborPose => !grid.IsCellPassableAt(neighborPose));
        }
    }
}