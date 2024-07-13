using System;
using UnityEngine;

namespace Source.BattleSystem.Visual
{
    public class AttackTrigger : MonoBehaviour
    {
        public event Action AttackStart, AttackEnds = null;

        public void TriggerAttackStart()
        {
            AttackStart?.Invoke();
        }
        
        public void TriggerAttackEnd()
        {
            AttackEnds?.Invoke();
        }
    }
}