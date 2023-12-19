using FluentAssertions;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Check
    {
        public static void IsExpectedVector3MatchesWith(Vector3 isometricVector3)
        {
            new { Vector = isometricVector3 }.Should().Be(new { Vector = TestParameter.ExpectedVector3 });
        }
    }
}