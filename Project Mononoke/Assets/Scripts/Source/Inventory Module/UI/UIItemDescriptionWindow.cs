using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void InitializeWith(Sprite icon, string title, string description)
    {
        _icon.sprite = icon;
        _title.text = title;
        _description.text = description;
        _icon.gameObject.SetActive(true);
    }
}
