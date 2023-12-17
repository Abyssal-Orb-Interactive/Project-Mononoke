using Base.UnityExtensions;
using NUnit.Framework;
using Tests.NativeTestsLanguageInfrastructure;

namespace Tests.Unity_Extensions_Tests.Vector2_Extension_Tests
{
    public class Vector2ToIsometricTests
    {
        [Test]
        public void WhenVectorIs00_AndConvertToIsometric_ThenVectorShouldBe00()
        {
            // Arrange
            SetUp.AssumedVector2With(0,0);
            SetUp.ExpectedVector2(0,0);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs11_AndConvertToIsometric_ThenVectorShouldBe20()
        {
            // Arrange
            SetUp.AssumedVector2With(1,1);
            SetUp.ExpectedVector2(2,0);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs1Minus1_AndConvertToIsometric_ThenVectorShouldBe0Minus1()
        {
            // Arrange
            SetUp.AssumedVector2With(1,-1);
            SetUp.ExpectedVector2(0,-1);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
        [Test]
        public void WhenVectorIsMinus11_AndConvertToIsometric_ThenVectorShouldBe01()
        {
            // Arrange
            SetUp.AssumedVector2With(-1,1);
            SetUp.ExpectedVector2(0,1);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
        [Test]
        public void WhenVectorIsMinus1Minus1_AndConvertToIsometric_ThenVectorShouldBeMinus20()
        {
            // Arrange
            SetUp.AssumedVector2With(-1,-1);
            SetUp.ExpectedVector2(-2,0);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
        [Test]
        public void WhenVectorIsMinus10_AndConvertToIsometric_ThenVectorShouldBeMinus1AndHalf()
        {
            // Arrange
            SetUp.AssumedVector2With(-1,0);
            SetUp.ExpectedVector2(-1,0.5f);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
        [Test]
        public void WhenVectorIs0Minus1_AndConvertToIsometric_ThenVectorShouldBeMinus1AndMinusHalf()
        {
            // Arrange
            SetUp.AssumedVector2With(0,-1);
            SetUp.ExpectedVector2(-1,-0.5f);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
        [Test]
        public void WhenVectorIs01_AndConvertToIsometric_ThenVectorShouldBe1AndHalf()
        {
            // Arrange
            SetUp.AssumedVector2With(0,1);
            SetUp.ExpectedVector2(1,0.5f);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
        [Test]
        public void WhenVectorIs10_AndConvertToIsometric_ThenVectorShouldBeMinus1AndMinusHalf()
        {
            // Arrange
            SetUp.AssumedVector2With(1,0);
            SetUp.ExpectedVector2(1,-0.5f);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVectorMatchesWith(result);
        }
    }
}