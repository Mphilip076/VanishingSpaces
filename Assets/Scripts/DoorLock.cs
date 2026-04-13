using UnityEngine;
using TMPro;

public class DoorLock : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 1f;
    public KeyCode interactKey = KeyCode.E;
    public string requiredKeyName = "Key";
    public int exitNum = 1;

    [Header("Sounds")]
    public AudioClip unlockSound;

    private AudioSource doorSound;

    private static bool isUnlocked = false;
    private bool playerNearby = false;

    void Start()
    {
        doorSound = GetComponent<AudioSource>();
    }

    void Update()
    {
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
            if (Input.GetKeyDown(interactKey))
            {
                TryUnlock();
            }
        }
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
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

            Invoke("LoadNextScene", 1.5f);
        }
    }


    void LoadNextScene()
    {
        Room.currentRoom.UseExit(exitNum);
    }
}