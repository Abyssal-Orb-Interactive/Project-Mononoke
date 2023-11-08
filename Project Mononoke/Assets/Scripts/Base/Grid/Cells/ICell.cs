namespace Base.Grid
{
    public interface ICell<TCellContent> : IReadonlyCell<TCellContent>
    {
        public bool TrySetValue(TCellContent content);
    }
}