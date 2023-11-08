namespace Base.Grid.CellContent
{
    public class BoolCellContent : IFillableCellContent
    {
        public bool Value { get; set; }

        public BoolCellContent(bool value)
        {
            Value = value;
        }

        public BoolCellContent()
        {
            Value = false;
        }

        public int GetWeight()
        {
            return Value ? 100 : 0;
        }

        public bool TryAdd(IFillableCellContent content)
        {
            if (content is not BoolCellContent cellContent) return false;
            AddValue(cellContent);
            return true;
        }

        private void AddValue(BoolCellContent content)
        {
            Value = true;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}