namespace Source.Items
{
    public abstract class Item : IPickUpable, IDamageable, ISellable
    {
        protected float _weight = 0f;
        protected float _volume = 0f;
        protected float _price = 0f;
        protected float _durability = 0f;
        protected float _currentDurability = 0f;


        public abstract float GetWeight();
        public abstract float GetVolume();
        public abstract void TakeDamage();
        public abstract float GetPrice();
    }
}
