using Base.UnityExtensions;
using NUnit.Framework;
using Tests.NativeTestsLanguageInfrastructure;

namespace Tests.Unity_Extensions_Tests.Vector3_Extension_Tests
{
    public class Vector3ToIsometricTests
    {
        [Test]
        public void WhenVectorIs000_AndConvertToIsometric_ThenVectorShouldBe000()
        {
            // Arrange
            SetUp.AssumedVector3With(0,0, 0);
            SetUp.ExpectedVector3With(0,0, 0);
            // Act
            var result = TestParameter.AssumedVector3.ToIsometric();
            // Assert
            Check.IsExpectedVector3MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs111_AndConvertToIsometric_ThenVectorShouldBe101()
        {
            // Arrange
            SetUp.AssumedVector3With(1,1, 1);
            SetUp.ExpectedVector3With(1,0, 1);
            // Act
            var result = TestParameter.AssumedVector3.ToIsometric();
            // Assert
            Check.IsExpectedVector3MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs1Minus1Minus1_AndConvertToIsometric_ThenVectorShouldBe0MinusHalfMinus1()
        {
            // Arrange
            SetUp.AssumedVector3With(1,-1, -1);
            SetUp.ExpectedVector3With(0,-0.5f, -1);
            // Act
            var result = TestParameter.AssumedVector3.ToIsometric();
            // Assert
            Check.IsExpectedVector3MatchesWith(result);
        }
    }
}