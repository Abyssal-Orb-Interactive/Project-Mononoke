using System;
using UnityEngine;

namespace Source.BattleSystem.Visual
{
    public class ShadowballAnimationDisposeTrigger : MonoBehaviour
    {
        public event Action DisposeEnds = null;

        private void DisposeEndsTrigger()
        {
            DisposeEnds?.Invoke();
        }
    }
}