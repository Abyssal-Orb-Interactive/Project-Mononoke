using System;
using Source.BattleSystem;
using UnityEngine;

namespace Source.Character.AI
{
    public class AISearchAreaTrigger : MonoBehaviour
    {

        public event Action<object> SomethingEnteredInSearchTrigger = null;
        private PathfinderAI _ai = null;

        public void Initialize(PathfinderAI pathfinderAI)
        {
            _ai = pathfinderAI;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<StatsHolder>(out var statsHolder))
            {
                SomethingEnteredInSearchTrigger?.Invoke(statsHolder);
                _ai.StartFollowingPath(statsHolder.transform.position);
            }
        }
    }
}
