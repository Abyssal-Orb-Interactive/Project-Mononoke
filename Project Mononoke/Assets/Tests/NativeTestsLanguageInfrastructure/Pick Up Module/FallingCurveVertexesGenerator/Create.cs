using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure.Pick_Up_Module.FallingCurveVertexesGenerator
{
    public static partial class Create
    {
        public static Source.PickUpModule.FallingCurveVertexesGenerator FallingCurveVertexesGeneratorWith(AnimationCurve curve, float duration)
        {
            return new Source.PickUpModule.FallingCurveVertexesGenerator(curve, duration);
        }
    }
}