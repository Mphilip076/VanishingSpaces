using UnityEngine;

public class Door : InteractableItem
{
    [Header("Exit (1, 2, or 3)")]
    public int exitNumber = 1;

    [Header("Sounds")]
    public AudioSource doorSound;

    void Start()
    {
        doorSound = GetComponent<AudioSource>();
        interactMessage = "Press E to use door";
    }

    public override void OnInteract()
    {
        if(doorSound != null) doorSound.Play();
        Invoke("LoadNextScene", 1.5f);
    }

    void LoadNextScene()
    {
        Room.currentRoom.UseExit(exitNumber);
    }

}
