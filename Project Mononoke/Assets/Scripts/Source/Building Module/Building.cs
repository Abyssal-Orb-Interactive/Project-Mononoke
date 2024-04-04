using Source.PickUpModule;
using UnityEngine;

namespace Source.BuildingModule
{
    public abstract class Building : MonoBehaviour
    {
        public bool ReadyToInteract { get; protected set; } = true;
        public abstract void StartInteractiveAction(PickUpper pickUpper);
    }
}