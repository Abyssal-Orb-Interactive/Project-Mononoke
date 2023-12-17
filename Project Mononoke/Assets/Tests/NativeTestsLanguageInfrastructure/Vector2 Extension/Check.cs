using FluentAssertions;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Check
    {
        public static void IsExpectedVectorMatchesWith(Vector2 isometricVector2)
        {
            new { Vector = isometricVector2 }.Should().Be(new { Vector = TestParameter.ExpectedVector2 });
        }
    }
}