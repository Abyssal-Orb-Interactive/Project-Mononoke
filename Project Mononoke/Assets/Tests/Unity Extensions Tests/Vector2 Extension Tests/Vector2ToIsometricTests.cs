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
            SetUp.ExpectedVector2With(0,0);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs11_AndConvertToIsometric_ThenVectorShouldBe10()
        {
            // Arrange
            SetUp.AssumedVector2With(1,1);
            SetUp.ExpectedVector2With(1,0);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs1Minus1_AndConvertToIsometric_ThenVectorShouldBe0MinusHalf()
        {
            // Arrange
            SetUp.AssumedVector2With(1,-1);
            SetUp.ExpectedVector2With(0,-0.5f);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIsMinus11_AndConvertToIsometric_ThenVectorShouldBe0Half()
        {
            // Arrange
            SetUp.AssumedVector2With(-1,1);
            SetUp.ExpectedVector2With(0,0.5f);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIsMinus1Minus1_AndConvertToIsometric_ThenVectorShouldBeMinus10()
        {
            // Arrange
            SetUp.AssumedVector2With(-1,-1);
            SetUp.ExpectedVector2With(-1,0);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIsMinus10_AndConvertToIsometric_ThenVectorShouldBeMinusHalfQuarter()
        {
            // Arrange
            SetUp.AssumedVector2With(-1,0);
            SetUp.ExpectedVector2With(-0.5f,0.25f);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs0Minus1_AndConvertToIsometric_ThenVectorShouldBeMinusHalfMinusQuarter()
        {
            // Arrange
            SetUp.AssumedVector2With(0,-1);
            SetUp.ExpectedVector2With(-0.5f,-0.25f);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs01_AndConvertToIsometric_ThenVectorShouldBeHalfQuarter()
        {
            // Arrange
            SetUp.AssumedVector2With(0,1);
            SetUp.ExpectedVector2With(0.5f, 0.25f);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs10_AndConvertToIsometric_ThenVectorShouldBeHalfMinusQuarter()
        {
            // Arrange
            SetUp.AssumedVector2With(1,0);
            SetUp.ExpectedVector2With(0.5f,-0.25f);
            // Act
            var result = TestParameter.AssumedVector2.ToIsometric();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
    }
}