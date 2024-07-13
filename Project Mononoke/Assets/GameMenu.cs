using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUI = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inventoryUI.SetActive(!_inventoryUI.active);
        }
    }
}
