namespace Base.Grid.CellContent
{
    public class PathNode
    {
        public int GCost { get; }
        public int HCost { get; }
        public int FCost { get; }
        
        public PathNode PrecedingNode { get; }

        public PathNode()
        {
            
        }

        public override string ToString()
        {
            return $"GCost = {GCost}\nHCost = {HCost}\nFCost = {FCost}";
        }
    }
}