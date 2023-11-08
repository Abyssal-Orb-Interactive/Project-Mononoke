namespace Base.Grid
{
    public class Cell<TCellContent> : ICell<TCellContent>
    {
        private TCellContent _content;

        public Cell()
        {
            _content = default;
        }
        
        public Cell(TCellContent content)
        {
            _content = content;
        }
        public TCellContent GetValue()
        {
            return _content;
        }

        public bool TrySetValue(TCellContent content)
        {
            if (content == null) return false;
            SetValue(content);
            return true;
        }

        private void SetValue(TCellContent content)
        {
            _content = content;
        }
    }
}