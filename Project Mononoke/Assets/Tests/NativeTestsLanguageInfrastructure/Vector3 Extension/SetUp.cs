namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class SetUp
    {
        public static void AssumedVector3With(float x, float y, float z = 0)
        {
            TestParameter.AssumedVector3 = Create.Vector3With(x, y, z);
        }
        
        public static void ExpectedVector3With(float x, float y, float z = 0)
        {
            TestParameter.ExpectedVector3 = Create.Vector3With(x, y, z);
        }
    }
}