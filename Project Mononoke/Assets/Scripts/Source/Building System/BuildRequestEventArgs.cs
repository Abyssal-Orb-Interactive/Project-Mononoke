using UnityEngine;

namespace Source.BuildingSystem
{
    public partial interface IBuildRequester
    {
        public class BuildRequestEventArgs
        {
            public int BuildingID { get; }
            public Vector3 Position { get; }

            public BuildRequestEventArgs(int buildingID, Vector3 position)
            {
                BuildingID = buildingID;
                Position = position;
            } 
        }   
    } 
        
}
