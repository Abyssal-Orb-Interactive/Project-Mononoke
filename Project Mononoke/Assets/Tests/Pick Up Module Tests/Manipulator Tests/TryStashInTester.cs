using FluentAssertions;
using NUnit.Framework;
using Source.InventoryModule;
using Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.Manipulator;

namespace Tests.Pick_Up_Module_Tests.Manipulator_Tests
{
    public class TryStashInTester
    {
        [Test]
        public void WhenInventoryIsNull_AndManipulatorTryStashInInventory_ThenManipulatorShouldNotStashItem()
        {
            // Arrange.
            SetUp.ManipulatorWithItem();
            // Act.
            var result = !TestParameter.Manipulator.TryStashIn(null);
            // Assert.
            Check.ItemWasNotStashed(result);
        }
        
        [Test]
        public void WhenManipulatorHasNotItem_AndManipulatorTryStashInInventory_ThenManipulatorShouldNotStashItem()
        {
            // Arrange.
            SetUp.ManipulatorWith10StrengthAnd10Capacity();
            // Act.
            var result = !TestParameter.Manipulator.TryStashIn(new Inventory());
            // Assert.
            Check.ItemWasNotStashed(result);
        }

        [Test] public void WhenInventoryCanNotTakeItem_AndManipulatorTryStashInInventory_ThenManipulatorShouldNotStashItem()
        {
            // Arrange.
            SetUp.ManipulatorWithItem();
            // Act.
            var result = !TestParameter.Manipulator.TryStashIn(new Inventory(0,0));
            // Assert.
            Check.ItemWasNotStashed(result);
        }
    }
}