using System;
using System.Linq;
using Source.ItemsModule;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Source.InventoryModule.UI
{
    public class ItemUIElement : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private GameObject _icon = null;
        [SerializeField] private GameObject _countBackground = null;
        [SerializeField] private GameObject _border = null;
        [SerializeField] private TMP_Text _countOFItems = null;

        public IPickUpable _item = null;

        public event Action<ItemUIElement> OnItemLeftClicked, OnItemRightClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag; 

        public void ResetData()
        {
            _icon.SetActive(false);
            _countBackground.SetActive(false);
            _border.SetActive(false);
            _item = null;
        }

        public void Select()
        {
            _border.SetActive(true);
        }

        public void Deselect()
        {
            _border.SetActive(false);
        } 

        public void InitializeWith(IPickUpable item) //Add quantity
        {
            _item = item;
            //_icon.GetComponent<Image>().sprite = _item.Icon;
            _icon.SetActive(true);
            _countBackground.SetActive(true); 
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(_item == null) return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData){}

        public string Test()
        {
            return OnItemDroppedOn.GetInvocationList().Count().ToString();
        }

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
