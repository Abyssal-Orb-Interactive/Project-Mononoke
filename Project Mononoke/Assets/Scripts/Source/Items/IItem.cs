namespace Source.Items
{
    public interface IItem : IPickUpable, IDamageable, ISellable
    {
        protected float Durability {get;}

    }
}
