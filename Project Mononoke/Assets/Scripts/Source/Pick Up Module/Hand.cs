using Source.ItemsModule;

namespace Source.PickUpModule
{
    public class Hand
    {
        private float _strength = 5f;
        private float _volume = 2f;
        private Item<ItemData> _item = null;

        public Hand(float strength, float volume)
        {
            if (strength < 0) _strength = 0;
            else if (volume < 0) _volume = 0;
            else
            {
                _strength = strength;
                _volume = volume;
            }
        }

        public bool TryTake(Item<ItemData> item)
        {
            if (!item.Database.TryGetItemDataBy(item.ID, out var data)) return false;
            if(data.Weight > _strength || data.Volume > _volume) return false;

            Take(item);
            return true;
        }

        private void Take(Item<ItemData> item)
        {
            _item = item;
        }

        public void UseTackedItem()
        {
            //_item?.();
        }
    }
}
