using UnityEngine;

namespace Source.ItemsModule
{
    public interface IInventoryUIPresentable
    {
        public Sprite Icon { get; }
        public string Description { get; }
    }
}
