using System;
using UnityEngine;

namespace Source.InventoryModule.UI
{
    public class InventoryMenu : MonoBehaviour
    {
        public Action<InventoryMenu, bool> StatusChanged = null;

        public void Toggle(bool signal)
        {
            gameObject.SetActive(signal);
        }
        private void OnEnable()
        {
            StatusChanged?.Invoke(this, true);
        }

        private void OnDisable()
        {
            StatusChanged?.Invoke(this, false);
        }
    }
}