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

    void Start()
    {
        doorSound = GetComponent<AudioSource>();
        interactRange = 1f;
        interactKey = KeyCode.E;
        if(isUnlocked) interactMessage = "Press E to use door";
        else interactMessage = "The door is locked (Press E to use key)";
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
        }
    }


    void LoadNextScene()
    {
        Room.currentRoom.UseExit(exitNum);
    }
}