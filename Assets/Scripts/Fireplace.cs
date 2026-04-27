using System.Collections.Generic;
using UnityEngine;

public class Fireplace : InteractableItem
{
    [Header("Settings")]
    public string lightItemName = "Lighter";

    [Header("Sounds")]
    public AudioClip lightSound;
    public AudioClip fireSound;
    public AudioClip lockSound;

    [Header("Fire")]
    public GameObject fire;


    // Component that plays sound
    private AudioSource audioSource;

    // Keep track of all fireplaces to prevent dupes
    private static List<Fireplace> fireplaces = new List<Fireplace>();
    private static int numAnchored = 0;
    
    // The room the fireplace is in
    private string roomName;

    // Fireplace state
    private bool isLit; // the player used matches / lighter
    private bool isAnchored; // the player placed the anchor item

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomName = Room.currentRoom.SceneName();
        CheckForDupe();

        audioSource = GetComponent<AudioSource>();
        if(fire != null) fire.SetActive(false);

        interactMessage = "Press E to light the fire";
        interactRange = 4f;
        isLit = false;
        isAnchored = false;
    }

    public static void ResetFireplaces()
    {
        foreach(var f in fireplaces)
        {
            if(f != null && f.gameObject != null)
            {
                Destroy(f.gameObject);
            }
        }

        fireplaces.Clear();
    }

    void Update(){
        if(Room.currentRoom.SceneName() != roomName && audioSource.isPlaying)
        {
            audioSource.Pause();
        }

        if(Room.currentRoom.SceneName() == roomName)
        {
            if(isLit && audioSource.isPlaying == false) 
                audioSource.Play();
        }

        if(isLit && isAnchored)
        {
            interactMessage = $"The fireplace is anchored ({numAnchored}/5)";
        }
    }

    // Check if the object is a duplicate
    // If it is, destroy it
    // If it isn't, register it
    private void CheckForDupe()
    {
        foreach(var f in fireplaces)
        {
            if(f.roomName == Room.currentRoom.SceneName() && f != this)
            {
                // This room, but a different object
                // Get rid of it
                Destroy(this.gameObject);
                return;
            }
        }

        // Not a duplicate.         
        DontDestroyOnLoad(gameObject);
        fireplaces.Add(this);
    }

    void TryLight()
    {
        if(isLit) return;

        if (InventoryManager.Instance.HasItem(lightItemName))
        {
            OnLight();
        }
        else
        {
            ShowShortMessage("You need a tool to light this!", 2);
        }
    }

    // The player lit the fire
    void OnLight()
    {
        isLit = true;

        // Play the lighting sound once
        if(audioSource != null && lightSound != null){
            audioSource.PlayOneShot(lightSound);
        }

        // Play fire sound
        if(audioSource != null && fireSound != null)
        {
            audioSource.clip = fireSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        if(fire != null)
        {
            fire.SetActive(true);
        }
        
        interactMessage = "Press E to place a gem";
    }

    void TryAnchor()
    {
        InventoryManager i = InventoryManager.Instance;
        string itemName = null;

        if(i.HasItem("Red Gem")){
            itemName = "Red Gem";
        }
        else if(i.HasItem("Blue Gem"))
        {
            itemName = "Blue Gem";
        }
        else if(i.HasItem("Green Gem"))
        {
            itemName = "Green Gem";
        }
        else if(i.HasItem("Yellow Gem"))
        {
            itemName = "Yellow Gem";
        }
        else if(i.HasItem("White Gem"))
        {
            itemName = "White Gem";
        }

        if(itemName != null)
        {
            // Take the item and do the magic
            i.RemoveItemByName(itemName);
            OnAnchor();
        }
    }

    // The player placed the anchor item
    void OnAnchor()
    {
        Room.currentRoom.LockInPlace();
        isAnchored = true;

        // Play the sound?
        if(audioSource != null && lockSound != null)
        {
            audioSource.PlayOneShot(lockSound);
        }

        numAnchored++;
    }

    public static bool IsCurrentRoomAnchored(){
        foreach (Fireplace f in fireplaces)
        {
            if (f != null && f.roomName == Room.currentRoom.SceneName())
            {
                return f.isAnchored;
            }
        }

        return false;
    }

    public static bool AllRoomsAnchored()
    {
        return numAnchored >= 5;
    }
    public override void OnInteract()
    {
        if(!isLit) TryLight();
        else if(isLit && !isAnchored) TryAnchor();
    }
}
