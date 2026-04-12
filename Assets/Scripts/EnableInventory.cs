using UnityEngine;

public class EnableInventoryUI : MonoBehaviour
{
    void Start()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.gameObject.SetActive(true);
            InventoryManager.Instance.RefreshUI();
        }
    }
}