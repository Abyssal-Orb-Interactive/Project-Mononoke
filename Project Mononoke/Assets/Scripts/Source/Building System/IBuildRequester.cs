namespace Source.BuildingSystem
{
    public partial interface IBuildRequester
    {
        public void MakeRequest(BuildRequestEventArgs args)
        {
            BuildingRequestsBus.MakeRequest(this ,args);
        }
    }
}
