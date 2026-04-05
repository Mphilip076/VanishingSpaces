using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DoorLock : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.E;
    public string requiredKeyName = "Key";
    public string nextScene = "DiningRoom";

    private Transform player;
    private bool isUnlocked = false;
    private GameObject pickupPromptUI;
    private TextMeshProUGUI promptText;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Auto find UI
        pickupPromptUI = GameObject.Find("PickupPromptUI");
        GameObject promptObj = GameObject.Find("PromptText");
        if (promptObj != null)
            promptText = promptObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isUnlocked) return;
        if (player == null)
        {
            Debug.Log("Player is null in DoorLock!");
            return;
        }

        float distance = Vector3.Distance(
            transform.position, player.position
        );
    }

    void TryUnlock()
    {
        Debug.Log("TryUnlock called!");
        bool hasKey = HasKey();
        Debug.Log("Has key: " + hasKey);

        if (hasKey)
        {
            isUnlocked = true;
            ShowPrompt("Door unlocked!");
            Invoke("LoadNextScene", 1.5f);
        }
        else
        {
            ShowPrompt("The door is locked!");
        }
    }

    bool HasKey()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject item = InventoryManager.Instance.GetItemAtSlot(i);
            if (item != null)
            {
                PickableItem pickable = item.GetComponent<PickableItem>();
                Debug.Log("Slot " + i + ": " + (pickable != null ? pickable.itemName : "no PickableItem"));
                if (pickable != null && pickable.itemName == requiredKeyName)
                    return true;
            }
        }
        return false;
    }

    void ShowPrompt(string message)
    {
        if (pickupPromptUI != null) pickupPromptUI.SetActive(true);
        if (promptText != null) promptText.text = message;
        Invoke("HidePrompt", 2f);
    }

    void HidePrompt()
    {
        if (pickupPromptUI != null) pickupPromptUI.SetActive(false);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}