using Base.Grid.CellContent;
using Base.Math;

namespace Base.Grid
{
    public class PathFinder
    {
        private Grid<PathNode> _grid;
        public PathFinder(InPlaneCoordinateInt Sizes)
        {
            _grid = new Grid<PathNode>(Sizes, () => new PathNode());
        }

        public Grid<PathNode> GetGrid()
        {
            return _grid;
        }
    }
}
