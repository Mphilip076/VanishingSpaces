using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : InteractableItem
{
    [Header("Exit (1, 2, or 3)")]
    public int exitNumber = 1;

    [Header("Sounds")]
    public AudioSource doorSound;

    [Header("End Scene Name")]
    public string endSceneName = "EndScene";

    void Start()
    {
        doorSound = GetComponent<AudioSource>();
        interactMessage = "Press E to use door";
        interactRange = 3f;
    }

    public override void OnInteract()
    {
        if (doorSound != null) doorSound.Play();
        Invoke("LoadNextScene", 1.5f);
    }

    void LoadNextScene()
    {

        if (Fireplace.AllRoomsAnchored())
        {
            SceneManager.LoadScene("EndScene");
            return;
        }

        Room.currentRoom.UseExit(exitNumber);
    }
}