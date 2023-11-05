using Base.Utils;
using NUnit.Framework;
using Tests.NativeTestsLanguageInfrastructure;
using TMPro;
using UnityEngine;

namespace Tests.TextMeshProFabricTests
{
    public class TMPtFabricCreatingTests
    {
        [Test]
        public void WhenCreatingDefaultTMPt_AndDefaultValues_ThenFontSizeOfTMPtShouldBe40f()
        {
            // Arrange.
            SetUp.FontSizePatternWith(40f);
            // Act.
            Create.TextMeshProWith(TextProperties.Default());
            // Assert.
            Check.TextFrontSizeShouldMatchWith(TestParameter.FontSizePattern);
        }
        
        [Test]
        public void WhenCreatingDefaultTMPt_AndDefaultValues_ThenColorOfTMPtShouldBeWhite()
        {
            // Arrange.
            SetUp.ColorPatternWith(Color.white);
            // Act.
            Create.TextMeshProWith(TextProperties.Default());
            // Assert.
            Check.TextColorShouldMatchWith(TestParameter.ColorPattern);
        }
        
        [Test]
        public void WhenCreatingDefaultTMPt_AndDefaultValues_ThenTextAlignmentOfTMPtShouldBeWhite()
        {
            // Arrange.
            SetUp.TextAlignmentPatternWith(TextAlignmentOptions.Left);
            // Act.
            Create.TextMeshProWith(TextProperties.Default());
            // Assert.
            Check.TextAlignmentShouldMatchWith(TestParameter.TextAlignmentPattern);
        }
        
        [Test]
        public void WhenCreatingDefaultTMPt_AndDefaultValues_ThenTextParentOfTMPtShouldBeNull()
        {
            // Arrange.
            SetUp.TextParentPatternWith(Create.GameObjectWithEmptyParent().parent);
            // Act.
            Create.TextMeshProWith(TextProperties.Default());
            // Assert.
            Check.TextParentShouldMatchWith(TestParameter.TextParentPattern);
        }

        [Test]
        public void WhenCreatingDefaultTMPt_AndDefaultValues_ThenTextLocalPositionOfTMPtShouldBeVector3Default()
        {
            // Arrange.
            SetUp.TextLocalPositionPatternWith(default);
            // Act.
            Create.TextMeshProWith(TextProperties.Default());
            // Assert.
            Check.TextLocalPositionShouldMatchWith(TestParameter.TextLocalPositionPattern);
        }
        
        [Test]
        public void WhenCreatingDefaultTMPt_AndDefaultValues_ThenTextSortingOrderOfTMPtShouldBe5000()
        {
            // Arrange.
            SetUp.TextSortingOrderPatternWith(5000);
            // Act.
            Create.TextMeshProWith(TextProperties.Default());
            // Assert.
            Check.TextSortingOrderShouldMatchWith(TestParameter.TextSortingOrderPattern);
        }
    }
}
