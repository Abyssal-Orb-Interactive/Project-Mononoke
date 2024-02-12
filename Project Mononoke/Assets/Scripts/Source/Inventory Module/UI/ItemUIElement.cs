using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Source.InventoryModule.Inventory;
using static Source.ItemsModule.TrashItemsDatabaseSO;

namespace Source.InventoryModule.UI
{
    public class ItemUIElement : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private GameObject _icon = null;
        [SerializeField] private GameObject _countBackground = null;
        [SerializeField] private GameObject _border = null;
        [SerializeField] private TMP_Text _countOFItems = null;

        public InventoryItem ItemData { get; private set; } = default;

        public event Action<ItemUIElement> OnItemLeftClicked, OnItemRightClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag; 

        public void ResetData()
        {
            _icon.SetActive(false);
            _countBackground.SetActive(false);
            _border.SetActive(false);
            ItemData = default;
        }

        public void Select()
        {
            _border.SetActive(true);
        }

        public void Deselect()
        {
            _border.SetActive(false);
        } 

        public void InitializeWith(InventoryItem item) //Add quantity
        {
            ItemData = item;
            if(EqualityComparer<InventoryItem>.Default.Equals(item, default)) 
            {
                _icon.SetActive(false);
                _countBackground.SetActive(false);
                return;
            }
            ItemData.Database.TryGetItemDataBy(ItemData.ID, out ItemData data);
            _icon.GetComponent<Image>().sprite = data.UIData.Icon;
            _icon.SetActive(true);
            _countBackground.SetActive(true); 
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(ItemData.Equals(default)) return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData){}

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left) OnItemLeftClicked?.Invoke(this);
            else OnItemRightClicked?.Invoke(this);
        }
    }
}
