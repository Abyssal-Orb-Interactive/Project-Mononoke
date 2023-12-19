using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Create
    {
        public static Vector3 Vector3With(float x, float y, float z = 0)
        {
            return new Vector3(x, y, z);
        }
    }
}