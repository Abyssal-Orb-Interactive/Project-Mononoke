namespace Source.ItemsModule
{
    public interface IItem : IPickUpable, IDamageable, ISellable
    {
        public float Durability {get;}
    }
}
