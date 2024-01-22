namespace Source.BuildingModule
{
    public partial interface IBuildRequester
    {
        public void MakeRequest(BuildRequestEventArgs args);
    }
}
