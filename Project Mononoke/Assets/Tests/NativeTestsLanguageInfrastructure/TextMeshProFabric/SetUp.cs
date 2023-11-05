using TMPro;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class SetUp
    {
        public static void FontSizePatternWith(float fontSizePattern)
        {
            TestParameter.FontSizePattern = fontSizePattern;
        }

        public static void ColorPatternWith(Color color)
        {
            TestParameter.ColorPattern = color;
        }
        
        public static void TextAlignmentPatternWith(TextAlignmentOptions textAlignment)
        {
            TestParameter.TextAlignmentPattern = textAlignment;
        }
        
        public static void TextParentPatternWith(Transform textParent)
        {
            TestParameter.TextParentPattern = textParent;
        }
        
        public static void TextLocalPositionPatternWith(Vector3 textLocalPosition)
        {
            TestParameter.TextLocalPositionPattern = textLocalPosition;
        }
        
        public static void TextSortingOrderPatternWith(int textSortingOrder)
        {
            TestParameter.TextSortingOrderPattern = textSortingOrder;
        }
    }
}