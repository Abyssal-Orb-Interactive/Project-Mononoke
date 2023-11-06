using FluentAssertions;
using TMPro;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Check
    {
        public static void TextFrontSizeShouldMatchWith(float fontSizePattern)
        {
            new {FontSize = TestParameter.Text.fontSize}.Should().Be(new{FontSize = fontSizePattern});
        }

        public static void TextColorShouldMatchWith(Color colorPattern)
        {
            new { Color = TestParameter.Text.color }.Should().Be(new { Color = colorPattern});
        }
        
        public static void TextAlignmentShouldMatchWith(TextAlignmentOptions textAlignmentPattern )
        {
            new { TextAlignment = TestParameter.Text.alignment }.Should().Be(new { TextAlignment = textAlignmentPattern});
        }
        
        public static void TextParentShouldMatchWith(Transform textParentPattern)
        {
            new { Parent = TestParameter.Text.transform.parent }.Should().Be(new { Parent = textParentPattern});
        }
        
        public static void TextLocalPositionShouldMatchWith(Vector3 textLocalPositionPattern )
        {
            new { TextLocalPositionPattern = TestParameter.Text.transform.localPosition }.Should().Be(new { TextLocalPositionPattern = textLocalPositionPattern});
        }
        
        public static void TextSortingOrderShouldMatchWith(int textSortingOrderPattern )
        {
            new { TextSortingOrderPattern = TestParameter.Text.sortingOrder }.Should().Be(new { TextSortingOrderPattern = textSortingOrderPattern});
        }
    }
}