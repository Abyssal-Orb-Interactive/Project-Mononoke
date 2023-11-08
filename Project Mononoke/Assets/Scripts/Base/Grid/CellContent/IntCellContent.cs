namespace Base.Grid.CellContent
{
    public class IntCellContent : IFillableCellContent
    {
        public int Value { get; private set; }

        public IntCellContent(int value)
        {
            Value = value;
        }

        public IntCellContent()
        {
            Value = 0;
        }
        public int GetWeight()
        {
            return Value;
        }

        public bool TryAdd(IFillableCellContent content)
        {
            if (content is not IntCellContent cellContent) return false;
            AddValue(cellContent);
            return true;
        }

        private void AddValue(IntCellContent content)
        {
            Value += content.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}