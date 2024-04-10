using System.Collections.Generic;
using Source.PickUpModule;

namespace Source.BuildingModule.Buildings
{
    public class BuildingsInteractionsRequestsHandler
    {
        private List<BuildingInteractionActionRequester> _requesters = null;

        public BuildingsInteractionsRequestsHandler()
        {
            _requesters = new List<BuildingInteractionActionRequester>();
        }

        public void AddRequester(BuildingInteractionActionRequester requester)
        {
            _requesters.Add(requester);
            requester.InteractionActionRequest += OnRequest;
        }

        private void OnRequest(Building building, PickUpper pickUpper)
        {
            building.StartInteractiveAction(pickUpper);
        }
    }
}