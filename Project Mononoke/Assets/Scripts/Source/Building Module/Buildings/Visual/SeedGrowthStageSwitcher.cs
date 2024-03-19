using System.Collections.Generic;
using UnityEngine;

namespace Source.BuildingModule.Buildings.Visual
{
    public class SeedGrowthStageSwitcher
    {
        private List<Sprite> _growthStages = null;
        private int _currentGrowthStageIndex = 0;
        private Seedbed _seedbed = null;

        public SeedGrowthStageSwitcher(Seedbed seedbed)
        {
            _seedbed = seedbed;
            _seedbed.GrowthStageChanged += SetNextGrowthStageSprite;
            
        }

        private void SetNextGrowthStageSprite(SpriteRenderer renderer)
        {
            if(!TryCalculateNextIndex()) return;
            
            renderer.sprite = _growthStages[_currentGrowthStageIndex];
        }

        private bool TryCalculateNextIndex()
        {
            if(_currentGrowthStageIndex == _growthStages.Count - 1) return false;
            
            _currentGrowthStageIndex++;
            return true;
        }
    }
}