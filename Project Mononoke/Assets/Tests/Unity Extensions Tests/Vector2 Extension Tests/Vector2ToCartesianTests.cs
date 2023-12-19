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
            SetUp.ExpectedVector2With(0,0);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs10_AndConvertToCartesian_ThenVectorShouldBe11()
        {
            // Arrange
            SetUp.AssumedVector2With(1,0);
            SetUp.ExpectedVector2With(1,1);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs0MinusHalf_AndConvertToCartesian_ThenVectorShouldBe1Minus1()
        {
            // Arrange
            SetUp.AssumedVector2With(0,-0.5f);
            SetUp.ExpectedVector2With(1,-1);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIs0Half_AndConvertToCartesian_ThenVectorShouldBeMinus11()
        {
            // Arrange
            SetUp.AssumedVector2With(0,0.5f);
            SetUp.ExpectedVector2With(-1,1);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIsMinus10_AndConvertToCartesian_ThenVectorShouldBeMinus1Minus1()
        {
            // Arrange
            SetUp.AssumedVector2With(-1,0);
            SetUp.ExpectedVector2With(-1,-1);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIsMinusHalfQuarter_AndConvertToCartesian_ThenVectorShouldBeMinus10()
        {
            // Arrange
            SetUp.AssumedVector2With(-0.5f,0.25f);
            SetUp.ExpectedVector2With(-1,0);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIsMinusHalfMinusQuarter_AndConvertToCartesian_ThenVectorShouldBe0Minus1()
        {
            // Arrange
            SetUp.AssumedVector2With(-0.5f,-0.25f);
            SetUp.ExpectedVector2With(0,-1);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIsHalfQuarter_AndConvertToCartesian_ThenVectorShouldBe01()
        {
            // Arrange
            SetUp.AssumedVector2With(0.5f, 0.25f);
            SetUp.ExpectedVector2With(0,1);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
        
        [Test]
        public void WhenVectorIsHalfMinusQuarter_AndConvertToCartesian_ThenVectorShouldBe10()
        {
            // Arrange
            SetUp.AssumedVector2With(0.5f,-0.25f);
            SetUp.ExpectedVector2With(1,0);
            // Act
            var result = TestParameter.AssumedVector2.ToCartesian();
            // Assert
            Check.IsExpectedVector2MatchesWith(result);
        }
    }
}