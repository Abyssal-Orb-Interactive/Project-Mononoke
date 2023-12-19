using Base.UnityExtensions;
using NUnit.Framework;
using Tests.NativeTestsLanguageInfrastructure;

namespace Tests.Unity_Extensions_Tests.Vector3_Extension_Tests
{
    public class Vector3ToCartesianTests
    {
        [Test]
        public void WhenVectorIs000_AndConvertToCartesian_ThenVectorShouldBe000()
        {
            // Arrange
            SetUp.AssumedVector3With(0,0, 0);
            SetUp.ExpectedVector3With(0,0, 0);
            // Act
            var result = TestParameter.AssumedVector3.ToCartesian();
            // Assert
            Check.IsExpectedVector3MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs101_AndConvertToCartesian_ThenVectorShouldBe111()
        {
            // Arrange
            SetUp.AssumedVector3With(1,0, 1);
            SetUp.ExpectedVector3With(1,1, 1);
            // Act
            var result = TestParameter.AssumedVector3.ToCartesian();
            // Assert
            Check.IsExpectedVector3MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs0MinusHalfMinus1_AndConvertToCartesian_ThenVectorShouldBe1Minus1Minus1()
        {
            // Arrange
            SetUp.AssumedVector3With(0,-0.5f, -1);
            SetUp.ExpectedVector3With(1,-1, -1);
            // Act
            var result = TestParameter.AssumedVector3.ToCartesian();
            // Assert
            Check.IsExpectedVector3MatchesWith(result);
        }
    }
}