using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Create
    {
        public static Vector2 Vector2With(float x, float y)
        {
            return new Vector2(x, y);
        }
    }
}