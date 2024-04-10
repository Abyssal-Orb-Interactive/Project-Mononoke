using UnityEngine;

namespace Source.ItemsModule
{
    public interface IItemData
    {
        string Name { get; }
        string ID { get; }
        GameObject Prefab { get; }
        float Weight { get; }
        float Volume { get; }
        float Price { get; }
        float Durability { get; }
        int MaxStackCapacity { get; }
        UseBehaviour UseBehaviour { get; }
        ItemData.UIItemData UIData { get; }
        string ToString();
    }
}