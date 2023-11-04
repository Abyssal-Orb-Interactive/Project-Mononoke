namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class SetUp
    {
        public static void CountPatternWith(int value)
        {
            TestParameter.CountPattern = value;
        }
    }
}