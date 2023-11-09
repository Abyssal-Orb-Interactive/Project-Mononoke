using System;
using System.Collections.Generic;
using Base.Grid.CellContent;
using Base.Math;
using Base.Utils;
using TMPro;
using UnityEngine;

namespace Base.Grid.Pathfinder
{
    public class PathfindingDebugStepVisual : MonoBehaviour
    {
        public static PathfindingDebugStepVisual Instance { get; private set; }

        [SerializeField] private Transform _pathFiendingVisualNode;
        [SerializeField] private Transform _nodesHolder;
        private List<Transform> _nodesVisuals;
        private List<GridSnapshotAction> _gridSnapshotActions;
        private bool _autoShowSnapshotsDesired;
        private float _autoShowSnapshotsTimer;
        private Transform[,] _visualNodes;

        private void Initialize()
        {
            Instance = this;
            _nodesVisuals = new List<Transform>();
            _gridSnapshotActions = new List<GridSnapshotAction>();
        }

        public void SetUp(Grid<PathNode> grid)
        {
            Initialize();
            _visualNodes = new Transform[grid.Sizes.X, grid.Sizes.Y];

            for (var x = 0; x < grid.Sizes.X; x++)
            {
                for (var y = 0; y < grid.Sizes.Y; y++)
                {
                    var gridPosition = new Vector3(x, y) * grid.CellArea + Vector3.one * grid.CellArea * 0.5f;
                    var visualNode = CreateVisualNode(gridPosition);
                    _visualNodes[x, y] = visualNode;
                    _nodesVisuals.Add(visualNode);
                }
            }

            HideNodesVisuals();
        }

        public void UpdatedVisual()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                ShowNextSnapshot();
            }
            
            if (Input.GetKeyDown(KeyCode.Return)) {
                _autoShowSnapshotsDesired = true;
            }

            if (!_autoShowSnapshotsDesired) return;
            
            const float autoShowSnapshotsTimerMax = .05f;
            _autoShowSnapshotsTimer -= Time.deltaTime;
            if (!(_autoShowSnapshotsTimer <= 0f)) return;
            
            _autoShowSnapshotsTimer += autoShowSnapshotsTimerMax;
            ShowNextSnapshot();
            if (_gridSnapshotActions.Count == 0) {
                _autoShowSnapshotsDesired = false;
            }
        }

        private void ShowNextSnapshot()
        {
            if (_gridSnapshotActions.Count <= 0) return;
            var gridSnapshotAction = _gridSnapshotActions[0];
            _gridSnapshotActions.RemoveAt(0);
            gridSnapshotAction.TriggerAction();
        }

        public void ClearSnapshots()
        {
            _gridSnapshotActions.Clear();
        }

        public void TakeSnapshot(Grid<PathNode> grid, PathNode current, List<PathNode> openedList,
            List<PathNode> closedList)
        {
            var gridSnapshotAction = new GridSnapshotAction();
            gridSnapshotAction.AddAction(HideNodesVisuals);

            for (var x = 0; x < grid.Sizes.X; x++)
            {
                for (var y = 0; y < grid.Sizes.Y; y++)
                {
                    var pathNode = grid.GetCellValue(new InPlaneCoordinateInt(x, y));

                    var gCost = pathNode.GCost;
                    var hCost = pathNode.HCost;
                    var fCost = pathNode.FCost;
                    var isCurrent = pathNode == current;
                    var isInOpenedList = openedList.Contains(pathNode);
                    var isInClosedList = closedList.Contains(pathNode);
                    var tmpX = x;
                    var tmpY = y;

                    gridSnapshotAction.AddAction(() =>
                    {
                        var visualNode = _visualNodes[tmpX, tmpY];
                        SetupVisualNode(visualNode, gCost, hCost, fCost);

                        var backgroundColor = ColorUtil.GetColorFromString("636363");

                        if (isInClosedList)
                        {
                            backgroundColor = new Color(1, 0, 0);
                        }

                        if (isInOpenedList)
                        {
                            backgroundColor = ColorUtil.GetColorFromString("009AFF");
                        }

                        if (isCurrent)
                        {
                            backgroundColor = new Color(0, 1, 0);
                        }

                        visualNode.GetComponentInChildren<SpriteRenderer>().color = backgroundColor;
                    });
                }
            }
            _gridSnapshotActions.Add(gridSnapshotAction);
        }

        public void TakeSnapshotFinalPath(Grid<PathNode> grid, List<PathNode> path)
        {
            var gridSnapshotAction = new GridSnapshotAction();
            gridSnapshotAction.AddAction(HideNodesVisuals);
            for (var x = 0; x < grid.Sizes.X; x++)
            {
                for (var y = 0; y < grid.Sizes.Y; y++)
                {
                    var pathNode = grid.GetCellValue(new InPlaneCoordinateInt(x, y));
                    var gCost = pathNode.GCost;
                    var hCost = pathNode.HCost;
                    var fCost = pathNode.FCost;
                    var isInPath = path.Contains(pathNode);
                    var tmpX = x;
                    var tmpY = y;
                    
                    gridSnapshotAction.AddAction(() =>
                    {
                        var visualNode = _visualNodes[tmpX, tmpY];
                        SetupVisualNode(visualNode, gCost, hCost, fCost);

                        Color backgroundColor;
                        backgroundColor = isInPath ? new Color(0, 1, 0) : ColorUtil.GetColorFromString("636363");

                        visualNode.GetComponentInChildren<SpriteRenderer>().color = backgroundColor;
                    });
                }
            }
            
            _gridSnapshotActions.Add(gridSnapshotAction);
        }

        private void HideNodesVisuals()
        {
            foreach (var visualNode in _visualNodes)
            {
                SetupVisualNode(visualNode, int.MaxValue, int.MaxValue, int.MaxValue);
            }
        }

        private Transform CreateVisualNode(Vector3 position)
        {
            var visualNode = Instantiate(_pathFiendingVisualNode, position, Quaternion.identity, _nodesHolder);
            return visualNode;
        }
        
        private static void SetupVisualNode(Transform visualNodeTransform, int gCost, int hCost, int fCost) {
            if (fCost < 1000) {
                visualNodeTransform.Find("gCostText").GetComponent<TextMeshPro>().SetText(gCost.ToString());
                visualNodeTransform.Find("hCostText").GetComponent<TextMeshPro>().SetText(hCost.ToString());
                visualNodeTransform.Find("fCostText").GetComponent<TextMeshPro>().SetText(fCost.ToString());
            } else {
                visualNodeTransform.Find("gCostText").GetComponent<TextMeshPro>().SetText("");
                visualNodeTransform.Find("hCostText").GetComponent<TextMeshPro>().SetText("");
                visualNodeTransform.Find("fCostText").GetComponent<TextMeshPro>().SetText("");
            }
        }

        private class GridSnapshotAction
        {

            private Action _action = () => { };

            public void AddAction(Action action)
            {
                _action += action;
            }

            public void TriggerAction()
            {
                _action();
            }
        }
    }
}