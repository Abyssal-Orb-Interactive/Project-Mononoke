using NSubstitute;
using Source.ItemsModule;

namespace Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.Manipulator
{
    public static class SetUp
    {
        public static void CapacityWith(float capacity)
        {
            TestParameter.Capacity = capacity;
        }

        public static void StrengthWith(float strength)
        {
            TestParameter.Strength = strength;
        }

        public static void ManipulatorWith(Source.PickUpModule.Manipulator manipulator)
        {
            TestParameter.Manipulator = manipulator;
        }
        
        public static void ManipulatorWithTestParameters()
        {
            var manipulator = Pick_Up_Module.Manipulator.Create.ManipulatorWith(TestParameter.Strength, TestParameter.Capacity);
            ManipulatorWith(manipulator);
        }

        public static void ItemWith(Item item)
        {
            TestParameter.Item = item;
        }
        
        public static void ItemWithNullData()
        {
            ItemWith(Pick_Up_Module.Manipulator.Create.ItemWithNullData());
        }

        public static void ItemWithTestParameterItemDataSubstitute()
        {
            ItemWith(Pick_Up_Module.Manipulator.Create.ItemWithTestParametersSubstitute());
        }

        public static void StrengthAndCapacityWith(float strength, float capacity)
        {
            StrengthWith(strength);
            CapacityWith(capacity);
        }

        public static void ManipulatorWith10StrengthAnd10Capacity()
        {
            StrengthAndCapacityWith(10f, 10f);
            ManipulatorWithTestParameters();
        }

        public static void ItemDataSubstituteWith(float weight, float volume)
        {
            TestParameter.ItemDataSubstitute = Substitute.For<IItemData>();
            TestParameter.ItemDataSubstitute.Weight.Returns(weight);
            TestParameter.ItemDataSubstitute.Volume.Returns(volume);
        }

        public static void ItemAndItemDataSubstituteWith(float weight, float volume)
        {
            SetUp.ItemDataSubstituteWith(weight, volume);
            SetUp.ItemWithTestParameterItemDataSubstitute();
        }

        public static void ManipulatorWithItem()
        {
            SetUp.ManipulatorWith10StrengthAnd10Capacity();
            SetUp.ItemAndItemDataSubstituteWith(3f, 4f);
            TestParameter.Manipulator.TryTake(TestParameter.Item);
        }
    }
}