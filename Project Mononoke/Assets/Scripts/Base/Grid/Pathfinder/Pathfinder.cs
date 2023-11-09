using System.Collections.Generic;
using System.Linq;
using Base.Grid.CellContent;
using Base.Math;
using UnityEngine;

namespace Base.Grid.Pathfinder
{
    public class Pathfinder
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;
        
        public static Pathfinder Instance { get; private set; }
        
        private readonly Grid<PathNode> _grid;
        private List<PathNode> _openedList;
        private List<PathNode> _closedList;

        public Pathfinder(InPlaneCoordinateInt Sizes)
        {
            Instance = this;
            _grid = new Grid<PathNode>(Sizes, () => new PathNode());
        }

        public Grid<PathNode> GetGrid()
        {
            return _grid;
        }

        public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            var start= GridPositionConverter.GetCoordinateInGrid(startWorldPosition, _grid.CellArea, _grid.OriginPosition);
            var end = GridPositionConverter.GetCoordinateInGrid(endWorldPosition, _grid.CellArea, _grid.OriginPosition);

            var path = FindPath(start, end);
            return path?.Select(pathNode => _grid.GetCoordinateOfCell(pathNode)).Select(pathNodeCoordinate => new Vector3(pathNodeCoordinate.X, pathNodeCoordinate.Y) * _grid.CellArea + Vector3.one * _grid.CellArea * .5f).ToList();
        }
        
        public List<PathNode> FindPath(InPlaneCoordinateInt start, InPlaneCoordinateInt end)
        {
            var startNode = _grid.GetCellValue(start);
            var endNode = _grid.GetCellValue(end);

            if (startNode == null || endNode == null) return null;
            
            _openedList = new List<PathNode> { startNode };
            _closedList = new List<PathNode>();

            InitializePathNodes();
            InitializeStartNode(start, end, startNode);
            SetUpPathfindingDebugStepVisual(startNode);

            while (_openedList.Count > 0)
            {
                var currentNode = GetLowestFCostNode(_openedList);
                if (currentNode == endNode)
                {
                    PathfindingDebugStepVisual.Instance.TakeSnapshot(_grid, currentNode, _openedList, _closedList);
                    PathfindingDebugStepVisual.Instance.TakeSnapshotFinalPath(_grid, CalculatePath(endNode));
                    return CalculatePath(endNode);
                }

                _openedList.Remove(currentNode);
                _closedList.Add(currentNode);

                var currentNodeCoordinate = _grid.GetCoordinateOfCell(currentNode);
                foreach (var neighbourCoordinate in GetNeighboursNodes(currentNodeCoordinate))
                {
                    var neighbour = _grid.GetCellValue(neighbourCoordinate);
                    if(_closedList.Contains(neighbour)) continue;
                    if (!neighbour.IsWalkable)
                    {
                        _closedList.Add(neighbour);
                        continue;
                    }
                    
                    var tentativeGCost = currentNode.GCost +
                                         CalculateDistance(currentNodeCoordinate, neighbourCoordinate);
                    if (tentativeGCost >= neighbour.GCost) continue;
                    
                    UpdateShorterPath(end, neighbour, currentNode, tentativeGCost, neighbourCoordinate);
                    PathfindingDebugStepVisual.Instance.TakeSnapshot(_grid, currentNode, _openedList, _closedList);
                }
            }
            return null;
        }

        private void SetUpPathfindingDebugStepVisual(PathNode startNode)
        {
            PathfindingDebugStepVisual.Instance.ClearSnapshots();
            PathfindingDebugStepVisual.Instance.TakeSnapshot(_grid, startNode, _openedList, _closedList);
        }

        private void UpdateShorterPath(InPlaneCoordinateInt end, PathNode neighbourNode, PathNode currentNode,
            int tentativeGCost, InPlaneCoordinateInt neighbourNodeCoordinate)
        {
            neighbourNode.PrecedingNode = currentNode;
            neighbourNode.GCost = tentativeGCost;
            neighbourNode.HCost = CalculateDistance(neighbourNodeCoordinate, end);
            neighbourNode.CalculateFCost();
                        
            if(!_openedList.Contains(neighbourNode)) _openedList.Add(neighbourNode);
        }

        private void InitializePathNodes()
        {
            for (var x = 0; x < _grid.Sizes.X; x++)
            {
                for (var y = 0; y < _grid.Sizes.Y; y++)
                {
                    var pathNode = _grid.GetCellValue(new InPlaneCoordinateInt(x, y));
                    pathNode.GCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.PrecedingNode = null;
                }
            }
        }

        private void InitializeStartNode(InPlaneCoordinateInt start, InPlaneCoordinateInt end, PathNode startNode)
        {
            startNode.GCost = 0;
            startNode.HCost = CalculateDistance(start, end);
            startNode.CalculateFCost();
        }
        private int CalculateDistance(InPlaneCoordinateInt aNode, InPlaneCoordinateInt bNode)
        {
            var xDistance = Mathf.Abs(aNode.X - bNode.X);
            var yDistance = Mathf.Abs(aNode.Y - bNode.Y);
            var remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFCostNode(IReadOnlyList<PathNode> pathNodes)
        {
            var lowestFCostNode = pathNodes[0];
            foreach (var pathNode in pathNodes.Where(pathNode => pathNode.FCost < lowestFCostNode.FCost))
            {
                lowestFCostNode = pathNode;
            }

            return lowestFCostNode;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            var path = new List<PathNode> { endNode };
            var currentNode = endNode;

            while (currentNode.PrecedingNode != null)
            {
                path.Add(currentNode.PrecedingNode);
                currentNode = currentNode.PrecedingNode;
            }

            path.Reverse();
            return path;
        }

        private List<InPlaneCoordinateInt> GetNeighboursNodes(InPlaneCoordinateInt nodeCoordinate)
        {
            var neighboursCoordinatesList = new List<InPlaneCoordinateInt>();
            
            if (HasLefterNeighbours(nodeCoordinate))
            {
                var leftNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X - 1, nodeCoordinate.Y);
                neighboursCoordinatesList.Add(leftNodeCoordinate);
                if (HasLowerNeighbours(nodeCoordinate))
                {
                    var lowerLeftNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X - 1, nodeCoordinate.Y - 1);
                    neighboursCoordinatesList.Add(lowerLeftNodeCoordinate);
                }
                if(HasUpperNeighbours(nodeCoordinate))
                {
                    var upperLeftNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X - 1, nodeCoordinate.Y + 1);
                    neighboursCoordinatesList.Add(upperLeftNodeCoordinate);
                }
            }

            if (HasRighterNeighbours(nodeCoordinate))
            {
                var righterNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X + 1, nodeCoordinate.Y);
                neighboursCoordinatesList.Add(righterNodeCoordinate);
                if (HasLowerNeighbours(nodeCoordinate))
                {
                    var lowerRightNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X + 1, nodeCoordinate.Y - 1);
                    neighboursCoordinatesList.Add(lowerRightNodeCoordinate);
                }
                if(HasUpperNeighbours(nodeCoordinate))
                {
                    var upperRightNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X + 1, nodeCoordinate.Y + 1);
                    neighboursCoordinatesList.Add(upperRightNodeCoordinate);
                }
            }

            if (HasLowerNeighbours(nodeCoordinate))
            {
                var lowerNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X, nodeCoordinate.Y - 1);
                neighboursCoordinatesList.Add(lowerNodeCoordinate);
            }
            if(HasUpperNeighbours(nodeCoordinate))
            {
                var upperNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X, nodeCoordinate.Y + 1);
                neighboursCoordinatesList.Add(upperNodeCoordinate);
            }

            return neighboursCoordinatesList;
        }

        private bool HasRighterNeighbours(InPlaneCoordinateInt nodeCoordinate)
        {
            return nodeCoordinate.X + 1 < _grid.Sizes.X;
        }

        private bool HasUpperNeighbours(InPlaneCoordinateInt nodeCoordinate)
        {
            return nodeCoordinate.Y + 1 < _grid.Sizes.Y;
        }

        private bool HasLowerNeighbours(InPlaneCoordinateInt nodeCoordinate)
        {
            return nodeCoordinate.Y - 1 >= 0;
        }

        private bool HasLefterNeighbours(InPlaneCoordinateInt nodeCoordinate)
        {
            return nodeCoordinate.X - 1 >= 0;
        }

        public PathNode GetNode(InPlaneCoordinateInt coordinate)
        {
            return _grid.GetCellValue(coordinate);
        }
    }
}
