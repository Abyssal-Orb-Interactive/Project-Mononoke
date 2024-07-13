using TMPro;
using UnityEngine;

namespace Source.BattleSystem.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SurroundersCounter : MonoBehaviour
    {
        private EnemySurroundingFunnel _surroundingFunnel = null;
        private TMP_Text _counter = null;

        public void Initialize(EnemySurroundingFunnel surroundingFunnel)
        {
            _surroundingFunnel = surroundingFunnel;
            _counter ??= GetComponent<TextMeshProUGUI>();
            _counter.text = _surroundingFunnel.SurroundersCount.ToString();
            _surroundingFunnel.SurroundersNumberChanged += OnSurroundingCounterNumberChanged;
        }

        private void OnSurroundingCounterNumberChanged(int number)
        {
            _counter.text = number.ToString();
        }
    }
}