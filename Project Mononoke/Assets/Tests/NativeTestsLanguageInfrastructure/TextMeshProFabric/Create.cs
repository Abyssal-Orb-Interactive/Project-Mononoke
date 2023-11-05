using Base.Utils;
using Tests.TextMeshProFabricTests;
using TMPro;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Create
    {
        public static void TextMeshProWith(TextProperties textProperties, string text = "Test")
        {
            TestParameter.Text = TextMeshProFabric.CreateTextInWorld(text, textProperties);
        }
        
        public static Transform GameObjectWithEmptyParent()
        {
            var gameObject = new GameObject("TestPattern", typeof(TextMeshPro));
            gameObject.transform.SetParent(null);
            return gameObject.transform;
        }
    }
}