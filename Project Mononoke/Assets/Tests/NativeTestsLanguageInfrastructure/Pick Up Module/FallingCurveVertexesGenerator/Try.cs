using System;
using Source.PickUpModule;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Try
    {
        public static bool CreateFallingCurveVertexGeneratorWith(AnimationCurve curve, float duration, out FallingCurveVertexesGenerator generator)
        {
            try
            {
                generator = Pick_Up_Module.FallingCurveVertexesGenerator.Create.FallingCurveVertexesGeneratorWith(curve, duration);
            }
            catch (Exception e)
            {
                generator = null;
                return false;
            }

            return true;
        }
    }
}