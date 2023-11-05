using TMPro;
using UnityEngine;

namespace Base.Utils
{
    /// <summary>
    /// Represents the style properties for text elements.
    /// </summary>
    public record TextStyle(float FontSize = 40f, Color? Color = null, TextAlignmentOptions TextAlignment = TextAlignmentOptions.Left)
    {
        public float FontSize { get; } = FontSize;
        public Color? Color { get; } = Color ?? UnityEngine.Color.white;
        public TextAlignmentOptions TextAlignment { get; } = TextAlignment;
        
        /// <summary>
        /// Returns a custom string representation of the text style.
        /// </summary>
        /// <returns>
        /// A string format "Style of text:\nFont size - {FontSize}.\nColor - {Color}.\nText Alignment - {TextAlignment}.".
        /// </returns>
        public override string ToString()
        {
            return $"Style of text:\nFont size - {FontSize}.\nColor - {Color}.\nText Alignment - {TextAlignment}.";
        }
    }

    /// <summary>
    /// Represents the properties for configuring and creating TextMeshPro objects.
    /// </summary>
    public record TextProperties (TextStyle Style, Transform Parent = null, Vector3 LocalPosition = default, int SortingOrder = 5000)
    {
        public TextStyle Style { get; } = Style;
        public Transform Parent { get; } = Parent;
        public Vector3 LocalPosition { get; } = LocalPosition;
        public int SortingOrder { get; } = SortingOrder;
        
        /// <summary>
        /// Returns a custom string representation of the text properties.
        /// </summary>
        /// <returns>
        /// A string format "Properties of text:\nStyle - {Style}.\nParent - {Parent}.\nText local position - {LocalPosition}\nSorting order - {SortingOrder}."
        /// </returns>
        public override string ToString()
        {
            return
                $"Properties of text:\nStyle - {Style}.\nParent - {Parent}.\nText local position - {LocalPosition}\nSorting order - {SortingOrder}.";
        }
    }
    
    /// <summary>
    /// A utility class for creating TextMeshPro objects in a Unity scene.
    /// </summary>
    public static class TextMeshProFabric
    {
        /// <summary>
        /// Creates a TextMeshPro object in the Unity scene with the specified text and properties.
        /// </summary>
        /// <param name="text">The text content of the TextMeshPro object.</param>
        /// <param name="properties">The properties for configuring the TextMeshPro object.</param>
        /// <returns>The created TextMeshPro object.</returns>
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
            ConfigureTextStyle(properties, textMesh);
            textMesh.sortingOrder = properties.SortingOrder;
            return textMesh;
        }

        private static void ConfigureTextStyle(TextProperties properties, TextMeshPro textMesh)
        {
            textMesh.alignment = properties.Style.TextAlignment;
            textMesh.fontSize = properties.Style.FontSize;
            textMesh.color = (Color) properties.Style!.Color;
        }

        private static void ConfigureTransform(TextProperties properties, GameObject gameObject)
        {
            var transform = gameObject.transform;
            transform.SetParent(properties.Parent, false);
            transform.localPosition = properties.LocalPosition;
        }
    }
}