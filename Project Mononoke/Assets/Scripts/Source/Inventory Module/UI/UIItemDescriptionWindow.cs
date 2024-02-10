using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Source.InventoryModule.Inventory;
using static Source.ItemsModule.TrashItemsDatabaseSO;

public class UIItemDescriptionWindow : MonoBehaviour
{
    [SerializeField] private Image _icon = null;
    [SerializeField] private TMP_Text _title = null;
    [SerializeField] private TMP_Text _description = null;

    public void Reset()
    {
        _icon.gameObject.SetActive(false);
        _icon.sprite = null;
        _title.text = "";
        _description.text = "";
    }

    public void InitializeWith(InventoryItem item)
    {
        if(!item.Database.TryGetItemDataBy(item.ID, out ItemData data)) return;
        _icon.sprite = data.UIData.Icon;
        _title.text = data.Name;
        _description.text = data.UIData.Description;
        _icon.gameObject.SetActive(true);
    }
}
