using Base.UnityExtensions;
using NUnit.Framework;
using Tests.NativeTestsLanguageInfrastructure;

namespace Tests.Unity_Extensions_Tests.Vector2_Extension_Tests
{
    public class Vector2ToCartesianTests
    {
        [Test]
        public void WhenVectorIs00_AndConvertToCartesian_ThenVectorShouldBe00()
        {
            // Arrange
            SetUp.AssumedVector2With(0,0);
            SetUp.ExpectedVector2(0,0);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
        [Test]
        public void WhenVectorIs20_AndConvertToCartesian_ThenVectorShouldBe11()
        {
            // Arrange
            SetUp.AssumedVector2With(2,0);
            SetUp.ExpectedVector2(1,1);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
        [Test]
        public void WhenVectorIs0Minus1_AndConvertToCartesian_ThenVectorShouldBe1Minus1()
        {
            // Arrange
            SetUp.AssumedVector2With(0,-1);
            SetUp.ExpectedVector2(1,-1);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
    }
}