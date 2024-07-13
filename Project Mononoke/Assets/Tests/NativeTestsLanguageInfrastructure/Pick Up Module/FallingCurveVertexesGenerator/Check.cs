using FluentAssertions;

namespace Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.FallingCurveVertexesGenerator
{
    public static partial class Check
    {
        public static void HasFallingCurveVertexGeneratorConstructorErrors(bool result)
        {
            new { HasException = result }.Should().BeEquivalentTo(new { HasException = false });
        }
    }
}