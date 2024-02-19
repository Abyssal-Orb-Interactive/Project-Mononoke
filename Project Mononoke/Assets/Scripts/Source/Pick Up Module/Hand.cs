using System;
using Source.ItemsModule;

namespace Source.PickUpModule
{
    public class Hand
    {
        private float _handStrength = 5f;
        private Item _item = null;

        public void Take(Item item)
        {
            //if(item.Weight > _handStrength) return;

            _item = item;
        }

        public void UseTackedItem()
        {
            //_item?.Use();
        }
    }
}
