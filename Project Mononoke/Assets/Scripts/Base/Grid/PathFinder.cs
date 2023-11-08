using System.Collections.Generic;
using System.Linq;
using Base.Grid.CellContent;
using Base.Math;
using UnityEngine;

namespace Base.Grid
{
    public class PathFinder
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;
        
        private readonly Grid<PathNode> _grid;
        private List<PathNode> _openedList;
        private List<PathNode> _closedList;

        public PathFinder(InPlaneCoordinateInt Sizes)
        {
            _grid = new Grid<PathNode>(Sizes, () => new PathNode());
        }

        public Grid<PathNode> GetGrid()
        {
            return _grid;
        }

        public List<PathNode> FindPath(InPlaneCoordinateInt start, InPlaneCoordinateInt end)
        {
            var startNode = _grid.GetCellValue(start);
            var endNode = _grid.GetCellValue(end);

            _openedList = new List<PathNode> { startNode };
            _closedList = new List<PathNode>();

            InitializePathNodes();
            InitializeStartNode(start, end, startNode);

            while (_openedList.Count > 0)
            {
                var currentNode = GetLowestFCostNode(_openedList);
                if (currentNode == endNode) return CalculatePath(endNode);

                _openedList.Remove(currentNode);
                _closedList.Add(currentNode);

                var currentNodeCoordinate = _grid.GetCoordinateOfCell(currentNode);
                foreach (var neighbourNode in GetNeighboursNodes(currentNodeCoordinate))
                {
                    if(_closedList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.IsWalkable)
                    {
                        _closedList.Add(neighbourNode);
                        continue;
                    }

                    var neighbourNodeCoordinate = _grid.GetCoordinateOfCell(neighbourNode); 
                    var tentativeGCost = currentNode.GCost + CalculateDistance(currentNodeCoordinate, neighbourNodeCoordinate);
                    if (tentativeGCost >= neighbourNode.GCost) continue;
                    
                    UpdateShorterPath(end, neighbourNode, currentNode, tentativeGCost, neighbourNodeCoordinate);
                }
            }
            return null;
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
            var currentNode = endNode.PrecedingNode;

            while (currentNode.PrecedingNode != null)
            {
                path.Add(currentNode.PrecedingNode);
                currentNode = currentNode.PrecedingNode;
            }

            path.Reverse();
            return path;
        }

        private List<PathNode> GetNeighboursNodes(InPlaneCoordinateInt nodeCoordinate)
        {
            var neighboursList = new List<PathNode>();
            
            if (HasLefterNeighbours(nodeCoordinate))
            {
                var leftNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X - 1, nodeCoordinate.Y);
                neighboursList.Add(GetNode((leftNodeCoordinate)));
                if (HasLowerNeighbours(nodeCoordinate))
                {
                    var lowerLeftNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X - 1, nodeCoordinate.Y - 1);
                    neighboursList.Add(_grid.GetCellValue(lowerLeftNodeCoordinate));
                }
                if(HasUpperNeighbours(nodeCoordinate))
                {
                    var upperLeftNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X - 1, nodeCoordinate.Y + 1);
                    neighboursList.Add(_grid.GetCellValue(upperLeftNodeCoordinate));
                }
            }

            if (HasRighterNeighbours(nodeCoordinate))
            {
                var righterNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X + 1, nodeCoordinate.Y);
                neighboursList.Add(_grid.GetCellValue(righterNodeCoordinate));
                if (HasLowerNeighbours(nodeCoordinate))
                {
                    var lowerRightNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X + 1, nodeCoordinate.Y - 1);
                    neighboursList.Add(_grid.GetCellValue(lowerRightNodeCoordinate));
                }
                if(HasUpperNeighbours(nodeCoordinate))
                {
                    var upperRightNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X + 1, nodeCoordinate.Y + 1);
                    neighboursList.Add(_grid.GetCellValue(upperRightNodeCoordinate));
                }
            }

            if (HasLowerNeighbours(nodeCoordinate))
            {
                var lowerNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X, nodeCoordinate.Y - 1);
                neighboursList.Add(_grid.GetCellValue(lowerNodeCoordinate));
            }
            if(HasUpperNeighbours(nodeCoordinate))
            {
                var upperNodeCoordinate = new InPlaneCoordinateInt(nodeCoordinate.X, nodeCoordinate.Y + 1);
                neighboursList.Add(_grid.GetCellValue(upperNodeCoordinate));
            }

            return neighboursList;
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

        private PathNode GetNode(InPlaneCoordinateInt coordinate)
        {
            return _grid.GetCellValue(coordinate);
        }
    }
}
