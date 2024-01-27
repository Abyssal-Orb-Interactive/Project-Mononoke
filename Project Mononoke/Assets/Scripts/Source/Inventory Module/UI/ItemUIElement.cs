using System;
using Source.ItemsModule;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.InventoryModule.UI
{
    public class ItemUIElement : MonoBehaviour
    {
        [SerializeField] private GameObject _icon = null;
        [SerializeField] private GameObject _countBackground = null;
        [SerializeField] private GameObject _border = null;
        [SerializeField] private TMP_Text _countOFItems = null;

        private bool _isEmpty = true;

        public event Action<ItemUIElement> OnItemLeftClicked, OnRightClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag; 

        public void ResetData()
        {
            _icon.SetActive(false);
            _countBackground.SetActive(false);
            _border.SetActive(false);
            _isEmpty = true;
        }

        private void Select()
        {
            _border.SetActive(true);
        }

        private void Deselect()
        {
            _border.SetActive(false);
        } 

        public void InitializeWith(Sprite sprite) //Add quantity
        {
            _icon.GetComponent<Image>().sprite = sprite;
            _icon.SetActive(true);
            _countBackground.SetActive(true); 
            _isEmpty = false;
        }

        private void OnBeginDrag()
        {
            if(_isEmpty) return;
            OnItemBeginDrag?.Invoke(this);
        }

        private void OnEndDrag()
        {
            OnItemEndDrag?.Invoke(this);
        }

        private void OnDrop()
        {
            OnItemDroppedOn?.Invoke(this);
        }
    }
}
