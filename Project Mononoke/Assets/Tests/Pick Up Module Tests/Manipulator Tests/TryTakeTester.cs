using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Source.ItemsModule;
using Source.PickUpModule;
using Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.Manipulator;

namespace Tests.Pick_Up_Module_Tests.Manipulator_Tests
{
    public class TryTakeTester
    {
        [Test]
        public void WhenItemIsNull_AndManipulatorTryTakeItem_ThenManipulatorShouldNotTakeItem()
        {
            // Arrange.
            SetUp.ManipulatorWith10StrengthAnd10Capacity();
            SetUp.ItemWith(null);
            // Act.
            var result = !TestParameter.Manipulator.TryTake(TestParameter.Item);
            // Assert.
            Check.ItemWasNotTaken(result);
        }

        [Test]
        public void WhenItemDataIsNull_AndManipulatorTryTakeItem_ThenManipulatorShouldNotTakeItem()
        {
            // Arrange.
            SetUp.ManipulatorWith10StrengthAnd10Capacity();
            SetUp.ItemWithNullData();
            // Act.
            var result = !TestParameter.Manipulator.TryTake(TestParameter.Item);
            // Assert.
            Check.ItemWasNotTaken(result); 
        }
        
        [Test]
        public void WhenItemDataIs12WeightAnd13Volume_AndManipulatorWith10StrengthAnd10CapacityTryTakeItem_ThenManipulatorShouldNotTakeItem()
        {
            // Arrange.
            SetUp.ManipulatorWith10StrengthAnd10Capacity();
            SetUp.ItemAndItemDataSubstituteWith(12f, 13f);
            // Act.
            var result = !TestParameter.Manipulator.TryTake(TestParameter.Item);
            // Assert.
            Check.ItemWasNotTaken(result);
        }
        
        [Test]
        public void WhenItemDataIs9WeightAnd13Volume_AndManipulatorWith10StrengthAnd10CapacityTryTakeItem_ThenManipulatorShouldNotTakeItem()
        {
            // Arrange.
            SetUp.ManipulatorWith10StrengthAnd10Capacity();
            SetUp.ItemAndItemDataSubstituteWith(9f, 13f);
            // Act.
            var result = !TestParameter.Manipulator.TryTake(TestParameter.Item);
            // Assert.
            Check.ItemWasNotTaken(result);
        }
        
        [Test]
        public void WhenItemDataIs15WeightAnd4Volume_AndManipulatorWith10StrengthAnd10CapacityTryTakeItem_ThenManipulatorShouldNotTakeItem()
        {
            // Arrange.
            SetUp.ManipulatorWith10StrengthAnd10Capacity();
            SetUp.ItemAndItemDataSubstituteWith(15f, 4f);
            // Act.
            var result = !TestParameter.Manipulator.TryTake(TestParameter.Item);
            // Assert.
            Check.ItemWasNotTaken(result);
        }
        
        [Test]
        public void WhenManipulatorAlreadyHasItem_AndManipulatorTryTakeItem_ThenManipulatorShouldNotTakeItem()
        {
            // Arrange.
            SetUp.ManipulatorWithItem();
            SetUp.ItemAndItemDataSubstituteWith(2f,3f);
            // Act.
            var result = !TestParameter.Manipulator.TryTake(TestParameter.Item);
            // Assert.
            Check.ItemWasNotTaken(result);
        }
    }
}