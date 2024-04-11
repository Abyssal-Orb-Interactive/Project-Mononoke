using System;
using System.Collections.Generic;
using Math = System.Math;
using UnityEngine;
#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace Pathfinding {
    using Pathfinding.Serialization;
    using Pathfinding.Util;
    
    [JsonOptIn]
    [Pathfinding.Util.Preserve]
    public class GroundGridGraph : NavGraph, IUpdatableGraph, ITransformedGraph
    {
        protected override void OnDestroy () {
            base.OnDestroy();

            // Clean up a reference in a static variable which otherwise should point to this graph forever and stop the GC from collecting it
            RemoveGridGraphFromStatic();
        }
        protected override void DestroyAllNodes () {
            GetNodes(node => {
                // If the grid data happens to be invalid (e.g we had to abort a graph update while it was running) using 'false' as
                // the parameter will prevent the Destroy method from potentially throwing IndexOutOfRange exceptions due to trying
                // to access nodes outside the graph. It is safe to do this because we are destroying all nodes in the graph anyway.
                // We do however need to clear custom connections in both directions
                (node as GridNodeBase)?.ClearCustomConnections(true);
                node.ClearConnections(false);
                node.Destroy();
            });
        }
        
        void RemoveGridGraphFromStatic () {
            var graphIndex = active.data.GetGraphIndex(this);

            GridNode.ClearGridGraph(graphIndex, this);
        }

        [JsonMember] public InspectorGridMode inspectorGridMode = InspectorGridMode.Grid;
        [JsonMember] public InspectorGridHexagonNodeSize inspectorHexagonSizeMode = InspectorGridHexagonNodeSize.Width;
        public int width;
        public int depth;
        [JsonMember] public float aspectRatio = 1F;
        [JsonMember] public float isometricAngle;
        public virtual bool uniformWidthDepthGrid => true;

        public virtual int LayerCount => 1;
        public static readonly float StandardIsometricAngle = 90-Mathf.Atan(1/Mathf.Sqrt(2))*Mathf.Rad2Deg;
        public override void GetNodes(Action<GraphNode> action)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Progress> ScanInternal()
        {
            throw new NotImplementedException();
        }

        public void UpdateArea(GraphUpdateObject o)
        {
            throw new NotImplementedException();
        }

        public void UpdateAreaInit(GraphUpdateObject o)
        {
            throw new NotImplementedException();
        }

        public void UpdateAreaPost(GraphUpdateObject o)
        {
            throw new NotImplementedException();
        }

        public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
        {
            throw new NotImplementedException();
        }

        public GraphTransform transform { get; }
    }
}