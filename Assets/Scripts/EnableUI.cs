using UnityEngine;
using TMPro;

public class RestorePersistentUI : MonoBehaviour
{
    void Start()
    {
        if (InventoryManager.Instance != null)
        {
            GameObject uiRoot = InventoryManager.Instance.transform.root.gameObject;
            uiRoot.SetActive(true);
            InventoryManager.Instance.RefreshUI();
        }

        GameObject pickupPrompt = GameObject.Find("PickupPromptUI");
        if (pickupPrompt != null)
        {
            pickupPrompt.SetActive(false);

            TextMeshProUGUI text = pickupPrompt.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = "";
        }
    }
}