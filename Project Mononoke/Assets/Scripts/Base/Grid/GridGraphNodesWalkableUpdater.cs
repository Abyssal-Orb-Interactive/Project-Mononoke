using System.Collections.Generic;
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
        private GroundGrid _grid = null;
        private Dictionary<Vector3Int, List<GridNode>> _nodesDividedIntoChunks = null;


        public GridGraphNodesWalkableUpdater()
        {
            _gridGraph = AstarPath.active.data.gridGraph;
        }

        public void UpdateGridGraphUsing(GroundGrid grid)
        {
            if (_grid != null) _grid.GridConfigurationChanged -= OnGridConfigurationChanged;
            _grid = grid;
            _grid.GridConfigurationChanged += OnGridConfigurationChanged;
            
            if (_nodesDividedIntoChunks == null) _nodesDividedIntoChunks = new Dictionary<Vector3Int, List<GridNode>>();
            else _nodesDividedIntoChunks.Clear();
            
            foreach (var node in _gridGraph.nodes)
            {
                var vectorNodePose = (Vector3)node.position;
                var inGridPose = _grid.WorldToGrid(vectorNodePose);
                
                if(!_nodesDividedIntoChunks.ContainsKey(inGridPose)) _nodesDividedIntoChunks.Add(inGridPose, new List<GridNode>());
                _nodesDividedIntoChunks[inGridPose].Add(node);

                RecalculateNode(inGridPose, node);
            }
        }
        
        private void OnGridConfigurationChanged(Vector3Int inGridCoord)
        {
            if(!_nodesDividedIntoChunks.ContainsKey(inGridCoord)) return;
            foreach (var node in _nodesDividedIntoChunks[inGridCoord])
            {
                RecalculateNode(inGridCoord, node);
            }
            
            RecalculateNeighbors(inGridCoord);
        }

        private void RecalculateNode(Vector3Int inGridPose, GridNode node)
        {
            AstarPath.active.AddWorkItem(new AstarWorkItem(() =>
            {
                if (!_grid.IsCellPassableAt(inGridPose))
                {
                    node.Walkable = false;
                }
                else
                {
                    node.Walkable = true;
                    if (HasUnpassableNeighbor(node, _grid))
                    {
                        node.Penalty += _penaltyAmount;
                    }
                    else
                    {
                        node.Penalty = 0;
                    }
                }
            }));
        }
        
        private void RecalculateNeighbors(Vector3Int inGridPose)
        {
            var directions = new[]
            {
                new Vector3Int(-1, 0, 0), // Left
                new Vector3Int(1, 0, 0),  // Right
                new Vector3Int(0, -1, 0), // Down
                new Vector3Int(0, 1, 0),  // Up
                new Vector3Int(-1, -1, 0), // Bottom Left
                new Vector3Int(1, -1, 0), // Bottom Right
                new Vector3Int(-1, 1, 0), // Top Left
                new Vector3Int(1, 1, 0)  // Top Right
            };

            foreach (var direction in directions)
            {
                var neighborChunkPose = inGridPose + direction;
                var chunk = _nodesDividedIntoChunks[neighborChunkPose];
                foreach (var node in chunk.Where(node => node.Walkable))
                {
                    if (HasUnpassableNeighbor(node, _grid)) node.Penalty += _penaltyAmount;
                    else node.Penalty = 0;
                }
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