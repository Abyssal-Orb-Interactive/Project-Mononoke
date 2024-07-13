using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Source.Character;
using Source.InventoryModule;
using Source.ItemsModule;
using Source.PickUpModule;
using Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.Manipulator;
using UnityEngine;

namespace Tests.Pick_Up_Module_Tests.PickUpper_Tests
{
    public class TryTakeItemFromInventoryWithManipulatorTester
    {
        [Test]
        public void WhenManipulatorAlreadyHasItem_AndPickUpperTryTakeAnotherItemFromInventory_ThenPickUpperShouldStashFirstItem()
        {
            // Arrange.
            SetUp.ManipulatorWithItem();
            TestParameter.Manipulator.Item.Data.ID.Returns("OldIDSubstitute");
            var inventory = new Inventory();
            var itemSubstitute = Create.ItemWithTestParametersSubstitute();
            itemSubstitute.Data.ID.Returns("IDSubstitute");
            inventory.TryAddItem(itemSubstitute);
            var go = new GameObject("PickUpper");
            var collider = go.AddComponent<CircleCollider2D>();
            var collidersHolder = go.AddComponent<CollidersHolder>();
            var pickUpper = go.AddComponent<PickUpper>();
            pickUpper.Initialize(inventory, TestParameter.Manipulator, collidersHolder);
            var heldItem = TestParameter.Manipulator.Item;
            // Act.
            var result = pickUpper.TryTakeItemFromInventoryWithManipulator(itemSubstitute.Data.ID);
            // Assert.
            new { HeldItem = pickUpper.Manipulator.Item }.Should().NotBeEquivalentTo(new { HeldItem = heldItem });
            //Tear Down
            Object.Destroy(pickUpper.gameObject);
        }
    }
}