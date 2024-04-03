using Source.PickUpModule;
using UnityEngine;

namespace Source.BuildingModule
{
    public abstract class Building : MonoBehaviour
    {
        public abstract void StartInteractiveAction(PickUpper pickUpper);
    }
}