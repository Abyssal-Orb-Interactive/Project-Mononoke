using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.FallingCurveVertexesGenerator
{
    public static partial class SetUp
    {
        public static void DurationWith(float duration)
        {
            TestParameter.Duration = duration;
        }  
        public static void CurveWithSubstitute()
        {
            TestParameter.Curve = NSubstitute.Substitute.For<AnimationCurve>();
        }
    }
}