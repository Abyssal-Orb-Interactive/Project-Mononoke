using System;

namespace Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.Manipulator
{
    public static class Try
    {
        public static bool CreateManipulatorWith(float strength, float capacity, out Source.PickUpModule.Manipulator manipulator)
        {
            try
            { 
                manipulator = Pick_Up_Module.Manipulator.Create.ManipulatorWith(strength, capacity);
            }
            catch (Exception e)
            {
                manipulator = null;
                return false;
            }

            return true;
        }
    }
}