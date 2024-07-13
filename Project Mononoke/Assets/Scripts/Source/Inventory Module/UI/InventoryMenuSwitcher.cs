using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.InventoryModule.UI
{
    public class InventoryMenuSwitcher : MonoBehaviour
    {
        [SerializeField] private List<InventoryMenu> _inventoryMenus = new();
        private bool _oneOfMenusIsActive = false;
        private int _activeMenuIndex = 0;

        public event Action MenuSwitched = null; 

        public void Initialize()
        {
            foreach (var menu in _inventoryMenus)
            {
                menu.StatusChanged += OnMenuActivationStatusChanged;
            }
        }

        private void OnMenuActivationStatusChanged(InventoryMenu inventoryMenu, bool active)
        {
            if (active)
            {
                if (_oneOfMenusIsActive)
                {
                    _inventoryMenus[_activeMenuIndex].gameObject.SetActive(false);
                }
                _oneOfMenusIsActive = true;
                for (var i = 0; i < _inventoryMenus.Count; i++)
                {
                    if (_inventoryMenus[i].Equals(inventoryMenu))
                    {
                        _activeMenuIndex = i;
                    }
                }
            }
            else
            {
                _oneOfMenusIsActive = false;
                _activeMenuIndex = 0;
            }
            
            MenuSwitched?.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (_oneOfMenusIsActive)
                {
                    _inventoryMenus[_activeMenuIndex].gameObject.SetActive(false);
                }
                else
                {
                    _inventoryMenus[0].gameObject.SetActive(true);
                }
            }
        }
    }
}