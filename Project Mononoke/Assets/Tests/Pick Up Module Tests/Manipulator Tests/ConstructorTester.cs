using FluentAssertions;
using NUnit.Framework;
using Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.Manipulator;

namespace Tests.Pick_Up_Module_Tests.Manipulator_Tests
{
    public class ConstructorTester
    {
        [Test]
        public void WhenStrengthAndCapacityIsNegative_AndManipulatorConstructing_ThenConstructorShouldThrowError()
        {
            // Arrange.
            SetUp.StrengthWith(-2f);
            SetUp.CapacityWith(-1f);
            // Act.
            var result = !Try.CreateManipulatorWith(TestParameter.Strength, TestParameter.Capacity, out var manipulator);
            // Assert.
            Check.HasManipulatorConstructorErrors(result);
        }
        [Test]
        public void WhenStrengthIsNegativeAndCapacityIsPositive_AndManipulatorConstructing_ThenConstructorShouldThrowError()
        {
            // Arrange.
            SetUp.StrengthWith(-2f);
            SetUp.CapacityWith(10f);
            // Act.
            var result = !Try.CreateManipulatorWith(TestParameter.Strength, TestParameter.Capacity, out var manipulator);
            // Assert.
            Check.HasManipulatorConstructorErrors(result);
        }
        [Test]
        public void WhenStrengthIsPositiveAndCapacityIsNegative_AndManipulatorConstructing_ThenConstructorShouldThrowError()
        {
            // Arrange.
            SetUp.StrengthWith(12f);
            SetUp.CapacityWith(-5f);
            // Act.
            var result = !Try.CreateManipulatorWith(TestParameter.Strength, TestParameter.Capacity, out var manipulator);
            // Assert.
            Check.HasManipulatorConstructorErrors(result);
        }
    }
}