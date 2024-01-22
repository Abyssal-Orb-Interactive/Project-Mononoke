using Source.BuildingModule;
using UnityEngine;
using VContainer;

namespace Source.Items
{
    public class Hoe : MonoBehaviour
    {
       [SerializeField] private int _seedBedID = 0;

       private OnGridBuilder _builder = null;
       
       [Inject] public void Initialize(OnGridBuilder builder)
       {            
            _builder = builder;
       }


        [ContextMenu ("Plow")]
        public void Plow()
        {
            _builder?.TryBuildBuildingWith(_seedBedID, Vector3.zero);
        }
    }
}
