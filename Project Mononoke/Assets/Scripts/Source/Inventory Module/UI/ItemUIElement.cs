using System;
using Source.ItemsModule;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Source.InventoryModule.InventoryPresenter;
using static Source.ItemsModule.TrashItemsDatabaseSO;

namespace Source.InventoryModule.UI
{
    public class ItemUIElement : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private GameObject _icon = null;
        [SerializeField] private GameObject _countBackground = null;
        [SerializeField] private GameObject _border = null;
        [SerializeField] private TMP_Text _countOFItems = null;

        public StackDataForUI StackData {get; private set;} = default;
        public bool IsEmpty {get; private set;}
        public Vector2 Sizes => new Vector2(gameObject.GetComponent<RectTransform>().rect.width, gameObject.GetComponent<RectTransform>().rect.height);

        public event Action<ItemUIElement> OnItemLeftClicked, OnItemRightClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag; 

        public void ResetData()
        {
            _icon.SetActive(false);
            _countBackground.SetActive(false);
            _border.SetActive(false);
            StackData = default;
            IsEmpty = true;
        }

        public void Select()
        {
            _border.SetActive(true);
        }

        public void Deselect()
        {
            _border.SetActive(false);
        } 

        public void InitializeWith(StackDataForUI stackData) //Add quantity
        {
           
            if(stackData == null || stackData.StackCount == 0) 
            {
                ResetData();
                return;
            }
            StackData = stackData;
            StackData.ItemDatabase.TryGetItemDataBy(StackData.ItemID, out var data);
            _icon.GetComponent<Image>().sprite = data.UIData.Icon;
            _icon.SetActive(true);
            _countOFItems.text = stackData.StackCount.ToString();
            _countBackground.SetActive(true); 
            IsEmpty = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(IsEmpty) return;
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
            if(IsEmpty) return;
            if(eventData.button == PointerEventData.InputButton.Left) OnItemLeftClicked?.Invoke(this);
            else OnItemRightClicked?.Invoke(this);
        }
    }
}
