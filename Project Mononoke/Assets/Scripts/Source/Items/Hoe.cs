using Source.BuildingModule;
using Source.ItemsModule;
using UnityEngine;

namespace Source.Items
{
    public class Hoe : ITool, IBuildRequester
    {
        public float Durability { get; private set; }

        public float Weight => throw new System.NotImplementedException();

        public float Volume => throw new System.NotImplementedException();

        public float Price => throw new System.NotImplementedException();

        public int ID => throw new System.NotImplementedException();

        public IPickUpableDatabase Database => throw new System.NotImplementedException();

        public void MakeRequest(IBuildRequester.BuildRequestEventArgs args)
        {
            BuildingRequestsBus.MakeRequest(this,args);
        }

        public void TakeDamage()
        {
            Durability--;
        }

        public void Use()
        {
            MakeRequest(new IBuildRequester.BuildRequestEventArgs(0, new Vector3(0,0,0)));
        }
    }
}
