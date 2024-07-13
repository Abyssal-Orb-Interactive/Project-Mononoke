using FluentAssertions;

namespace Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.Manipulator
{
    public static class Check
    {
        public static void HasManipulatorConstructorErrors(bool result)
        {
            new { HasException = result }.Should().BeEquivalentTo(new { HasException = true });
        }

        public static void ItemWasNotTaken(bool result)
        {
            new { ItemWasNotTaken = result }.Should().BeEquivalentTo(new { ItemWasNotTaken = true });
        }

        public static void ItemWasNotStashed(bool result)
        {
            new { ItemNotStashed = result }.Should().BeEquivalentTo(new { ItemNotStashed = true });
        }
    }
}