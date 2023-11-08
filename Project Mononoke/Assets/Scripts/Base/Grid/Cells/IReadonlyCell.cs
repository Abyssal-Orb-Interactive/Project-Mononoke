namespace Base.Grid
{
    public interface IReadonlyCell<TCellContent>
    {
        public TCellContent GetValue();
        
    }
}