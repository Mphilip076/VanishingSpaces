using UnityEngine;
using TMPro;

public class DoorLock : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.E;
    public string requiredKeyName = "Key";
    public int exitNum = 1;

    [Header("Sounds")]
    public AudioClip unlockSound;

    private AudioSource doorSound;

    private bool isUnlocked = false;
    private bool playerNearby = false;
    private GameObject pickupPromptUI;
    private TextMeshProUGUI promptText;

    void Start()
    {
        doorSound = GetComponent<AudioSource>();

        pickupPromptUI = GameObject.Find("PickupPromptUI");
        GameObject promptObj = GameObject.Find("PromptText");
        if (promptObj != null)
            promptText = promptObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isUnlocked) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange);
        playerNearby = false;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerNearby = true;
                break;
            }
        }

        if (playerNearby)
        {
            if(isUnlocked) ShowPrompt("Press E to use door");
            else ShowPrompt("You need a key! (E to use)");

            if (Input.GetKeyDown(interactKey))
            {
                CancelInvoke("HidePrompt");
                TryUnlock();
            }
        }
        else
        {
            HidePrompt();
        }
    }

    void TryUnlock()
    {
        if (InventoryManager.Instance.HasItem(requiredKeyName))
        {
            isUnlocked = true;

            if (doorSound != null && unlockSound != null)
            {
                doorSound.clip = unlockSound;
                doorSound.Play();
            }

            ShowPrompt("Door unlocked!");
            Invoke("LoadNextScene", 1.5f);
        }
        else
        {
            ShowPrompt("You need a key!");
        }
    }

    void ShowPrompt(string message)
    {
        if (pickupPromptUI != null) pickupPromptUI.SetActive(true);
        if (promptText != null) promptText.text = message;
    }

    void HidePrompt()
    {
        if (pickupPromptUI != null) pickupPromptUI.SetActive(false);
    }

    void LoadNextScene()
    {
        Room.currentRoom.UseExit(exitNum);
    }
}