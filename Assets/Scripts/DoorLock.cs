using UnityEngine;
using TMPro;

public class DoorLock : InteractableItem
{
    [Header("Settings")]
    public string requiredKeyName = "Key";
    public int exitNum = 1;

    [Header("Sounds")]
    public AudioClip unlockSound;
    private AudioSource doorSound;

    private static bool isUnlocked = false;
    private static DoorLock instance;

    void Start()
    {
        if(instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        doorSound = GetComponent<AudioSource>();
        interactRange = 3f;
        interactKey = KeyCode.E;
        interactMessage = "Press E to use door";

        DontDestroyOnLoad(gameObject);
    }

    public override void OnInteract()
    {
        if(isUnlocked) UseDoor();
        else TryUnlock();
    }

    private void UseDoor()
    {
        if (doorSound != null && unlockSound != null)
        {
            doorSound.clip = unlockSound;
            doorSound.Play();
        }

        Invoke("LoadNextScene", 1.5f);
    }

    void TryUnlock()
    {        
        if (InventoryManager.Instance.HasItem(requiredKeyName)){
            isUnlocked = true;
            interactMessage = "Press E to use door";
            InventoryManager.Instance.RemoveItemByName(requiredKeyName);
        }
        else
        {
            ShowShortMessage("You need a key!", 2);
        }
    }


    void LoadNextScene()
    {
        Room.currentRoom.UseExit(exitNum);
    }
}