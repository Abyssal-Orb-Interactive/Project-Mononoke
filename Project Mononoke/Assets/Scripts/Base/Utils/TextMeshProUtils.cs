using TMPro;
using UnityEngine;

namespace Base.Utils
{
    public struct TextStyle
    {
        public float FontSize { get; }
        public Color Color { get; }
        public TextAlignmentOptions TextAlignment { get; }

        public TextStyle(float fontSize, Color? color = null, TextAlignmentOptions textAlignment = TextAlignmentOptions.Left)
        {
            FontSize = fontSize;
            Color = color ?? Color.white;
            TextAlignment = textAlignment;
        }

        public static TextStyle Default()
        {
            return new TextStyle(40f);
        }
    }

    public struct TextProperties
    {
        private const int DEFAULT_SORTING_ORDER = 5000;
        
        public TextStyle Style { get; }
        public Transform Parent { get; }
        public Vector3 LocalPosition { get; }
        public int SortingOrder { get; }
        
        public TextProperties(TextStyle textStyle, Transform parent = null, Vector3 localPosition = default, int sortingOrder = DEFAULT_SORTING_ORDER)
        {
            Style = textStyle;
            Parent = parent;
            LocalPosition = localPosition;
            SortingOrder = sortingOrder;
        }

        public static TextProperties Default()
        {
            return new TextProperties(TextStyle.Default());
        }
    }
    
    public static class TextMeshProFabric
    {
        public static TextMeshPro CreateTextInWorld(string text, TextProperties properties)
        {
            var gameObject = new GameObject("World_Text", typeof(TextMeshPro));
            ConfigureTransform(properties, gameObject);
            var textMesh = ConfigureTextMeshPro(text, properties, gameObject);
            return textMesh;
        }

        private static TextMeshPro ConfigureTextMeshPro(string text, TextProperties properties, GameObject gameObject)
        {
            var textMesh = gameObject.GetComponent<TextMeshPro>();
            textMesh.text = text;
            ConfigureTextStyle(text, properties, textMesh);
            textMesh.sortingOrder = properties.SortingOrder;
            return textMesh;
        }

        private static void ConfigureTextStyle(string text, TextProperties properties, TextMeshPro textMesh)
        {
            textMesh.alignment = properties.Style.TextAlignment;
            textMesh.fontSize = properties.Style.FontSize;
            textMesh.color = properties.Style.Color;
        }

        private static void ConfigureTransform(TextProperties properties, GameObject gameObject)
        {
            var transform = gameObject.transform;
            transform.SetParent(properties.Parent, false);
            transform.localPosition = properties.LocalPosition;
        }
    }
}