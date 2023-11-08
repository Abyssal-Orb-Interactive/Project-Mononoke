namespace Base.Grid
{
    public interface IFillableCellContent
    {
        public int GetWeight();
        public bool TryAdd (IFillableCellContent value);
    }
}