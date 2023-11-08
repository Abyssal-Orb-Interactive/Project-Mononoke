namespace Base.Grid.CellContent
{
    public class PathNode
    {
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get; private set; }
        
        public bool IsWalkable { get; set; }
        public PathNode PrecedingNode { get; set; }

        public PathNode()
        {
            IsWalkable = true;
        }

        public void CalculateFCost()
        {
            FCost = GCost + HCost;
        }

        public override string ToString()
        {
            return $"GCost = {GCost}\nHCost = {HCost}\nFCost = {FCost}";
        }
    }
}