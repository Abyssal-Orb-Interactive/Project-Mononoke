namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class SetUp
    {
        public static void AssumedVector2With(float x, float y)
        {
            TestParameter.AssumedVector2 = Create.Vector2With(x, y);
        }
        
        public static void ExpectedVector2With(float x, float y)
        {
            TestParameter.ExpectedVector2 = Create.Vector2With(x, y);
        }
    }
}