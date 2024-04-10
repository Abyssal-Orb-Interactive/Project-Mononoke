using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUI = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(_inventoryUI.active == false) _inventoryUI.SetActive(true);
            else _inventoryUI.SetActive(false);
        }
    }
}
