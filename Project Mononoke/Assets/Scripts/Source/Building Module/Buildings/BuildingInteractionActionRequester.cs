using System;
using Source.PickUpModule;

namespace Source.BuildingModule.Buildings
{
    public interface BuildingInteractionActionRequester
    {
        public event Action<Building, PickUpper> InteractionActionRequest;
        
    }
}