using FluentAssertions;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Check
    {
        public static void IsExpectedVector2MatchesWith(Vector2 isometricVector2)
        {
            new { Vector = isometricVector2 }.Should().Be(new { Vector = TestParameter.ExpectedVector2 });
        }
    }
}