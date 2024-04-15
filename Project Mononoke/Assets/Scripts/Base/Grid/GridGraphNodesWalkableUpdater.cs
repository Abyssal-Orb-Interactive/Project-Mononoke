using Pathfinding;
using UnityEngine;

namespace Base.Grid
{
    public class GridGraphNodesWalkableUpdater
    {
        private readonly GridGraph _gridGraph = null;

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
                if (!grid.IsCellPassableAt(inGridPose))
                {
                    AstarPath.active.AddWorkItem(new AstarWorkItem(() =>
                    {
                        node.Walkable = false;
                    }));
                }
            }
        }
    }
}