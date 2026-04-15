using UnityEngine;

public class Door : InteractableItem
{
    [Header("Exit (1, 2, or 3)")]
    public int exitNumber = 1;

    [Header("Sounds")]
    public AudioSource doorSound;

    public override void Start()
    {
        base.Start();
        doorSound = GetComponent<AudioSource>();
        interactMessage = "Press E to use door";
    }

    public override void OnInteract()
    {
        Debug.Log("[Door] Player interacted with door");
        if(doorSound != null) doorSound.Play();
        Invoke("LoadNextScene", 1.5f);
    }

    void LoadNextScene()
    {
        Debug.Log("[Door] its heeeerrrreeeeeeee");
        Debug.Log("[door] room.currentRoom " + Room.currentRoom.SceneName());
        Room.currentRoom.UseExit(exitNumber);
    }

}
