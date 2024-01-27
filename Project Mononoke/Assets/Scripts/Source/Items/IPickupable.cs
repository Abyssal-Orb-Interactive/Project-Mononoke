namespace Source.ItemsModule
{
    public interface IPickUpable : IInventoryUIPresentable
    {
        public float Weight{get;}
        public float Volume{get;}
    }
}
