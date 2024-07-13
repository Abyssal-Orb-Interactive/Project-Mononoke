using Source.ItemsModule;

namespace Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.Manipulator
{
    public static class Create
    {
        public static Source.PickUpModule.Manipulator ManipulatorWith(float strength, float capacity)
        {
            return new Source.PickUpModule.Manipulator(strength, capacity);
        }
        public static Item ItemWithNullData()
        {
            return ItemWith(null);
        }

        public static Item ItemWith(IItemData data)
        {
            return new Item(data);
        }

        public static Item ItemWithTestParametersSubstitute()
        {
            return ItemWith(TestParameter.ItemDataSubstitute);
        }
    }
}