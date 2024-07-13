using NUnit.Framework;
using Tests.NativeTestsLanguageInfrastructure;
using Check = Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.FallingCurveVertexesGenerator.Check;
using SetUp = Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.FallingCurveVertexesGenerator.SetUp;
using TestParameter = Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.FallingCurveVertexesGenerator.TestParameter;

namespace Tests.Pick_Up_Module_Tests.FalligCurveVertexGenerator_Tests
{
    public class ConstructorTester
    {
        [Test]
        public void WhenGenerationDurationIsNegative_AndFallingCurveVertexGeneratorConstructing_ThenConstructorShouldThrowError()
        {
            // Arrange.
            SetUp.DurationWith(-1f);
            SetUp.CurveWithSubstitute();
            // Act.
            var result = Try.CreateFallingCurveVertexGeneratorWith(TestParameter.Curve, TestParameter.Duration, out var generator);
            // Assert.
            Check.HasFallingCurveVertexGeneratorConstructorErrors(result);
        }
    }
}