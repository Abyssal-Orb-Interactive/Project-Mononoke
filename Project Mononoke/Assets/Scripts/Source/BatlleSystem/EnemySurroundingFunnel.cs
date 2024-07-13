using System;
using System.Collections.Generic;
using System.Linq;
using Source.Character.AI.BattleAI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.BattleSystem
{
    public class EnemySurroundingFunnel
    {
        public IDamageable Surrounded { get; private set; } = null;
        private List<IDamager> _surrounders = null;
        public event Action<int> SurroundersNumberChanged = null;

        public int SurroundersCount => _surrounders.Count;
        public float SurroundedHealthPointsInPercents => Surrounded.CurrentHealthPointsInPercents;

        public EnemySurroundingFunnel(IDamageable surrounded, IDamager surrounder)
        {
            Surrounded = surrounded;
            Surrounded.Death += OnSurroundedDeath;
            _surrounders = new List<IDamager>();
            Add(surrounder);

            var component = Surrounded as MonoBehaviour;
            if(component == null) return;
            if (component.TryGetComponent<BattleAI>(out var battleAI))
            {
                battleAI.StopListeningSearchTriggerSignals();
            }
        }

        private void OnSurroundedDeath(IDamageable sourrounded)
        {
            foreach (var component in _surrounders.OfType<MonoBehaviour>())
            {
                if (!component.TryGetComponent<BattleAI>(out var battleAI)) continue;
                battleAI.StopListeningAreasSignals();
                battleAI.StartListeningAreasSignals();
            }
        }

        public void Add(IDamager surrounder)
        {
            _surrounders.Add(surrounder);
            
            var component = surrounder as MonoBehaviour;
            if(component == null) return;
            if (component.TryGetComponent<BattleAI>(out var battleAI))
            {
                battleAI.StopListeningAreasSignals();
            }
            
            SurroundersNumberChanged?.Invoke(_surrounders.Count);
        }
        
        
    }
}