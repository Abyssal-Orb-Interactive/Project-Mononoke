using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.InventoryModule.UI
{
    public class ItemActionsMenu : MonoBehaviour
    {
        [SerializeField] private Button _equipButton = null;
        [SerializeField] private Button _dropButton = null;

        public void AddEquipAction(Action equipAction)
        {
            _equipButton.onClick.AddListener(() => equipAction());
        }

        public void AddDropAction(Action dropAction)
        {
            _dropButton.onClick.AddListener(() => dropAction());
        }
        
        public void Toggle(bool signal)
        {
            if (signal == false)
            {
                _equipButton.onClick.RemoveAllListeners();
                _dropButton.onClick.RemoveAllListeners();
            }
            gameObject.SetActive(signal);
        }
    }
}