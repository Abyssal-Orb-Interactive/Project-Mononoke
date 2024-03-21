using System.Collections.Generic;
using Source.ItemsModule;
using UnityEngine;

namespace Source.BuildingModule.Buildings.Visual
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlantSprite : MonoBehaviour
    {
        private SpriteRenderer _renderer = null;
        private PlantGrowthStageSwitcher _stageSwitcher = null;
        private List<Sprite> _stagesSprites = null;

        private void OnValidate()
        {
            _renderer ??= GetComponent<SpriteRenderer>();
        }

        public void Initialize(PlantGrowthStageSwitcher stageSwitcher, List<Sprite> stagesSprites)
        {
            _stageSwitcher = stageSwitcher;
            _stageSwitcher.GrowthStageChanged += ChangeSprite;
            _stagesSprites = stagesSprites;
        }

        private void ChangeSprite(int stageIndex)
        {
            _renderer.sprite = _stagesSprites[stageIndex - 1];
        }
    }
}