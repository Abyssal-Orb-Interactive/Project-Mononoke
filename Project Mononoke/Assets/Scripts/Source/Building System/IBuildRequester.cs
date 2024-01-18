using System;

namespace Source.BuildingSystem
{
    public partial interface IBuildRequester
    {
        protected event EventHandler<BuildRequestEventArgs> OnBuildRequested;
        public void AddBuildRequestHandler(EventHandler<BuildRequestEventArgs> handler)
        {
            OnBuildRequested += handler;
        }
         public void RemoveBuildRequestHandler(EventHandler<BuildRequestEventArgs> handler)
         {
            OnBuildRequested -= handler;
         }
    }
}
