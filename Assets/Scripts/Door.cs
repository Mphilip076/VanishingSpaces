using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Exit (1, 2, or 3)")]
    public int exitNumber;

    [Header("Sounds")]
    public AudioSource doorSound;

    private GameObject pickupPromptUI;
    private TextMeshProUGUI promptText;
    private bool playerNearby = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorSound = GetComponent<AudioSource>();

        pickupPromptUI = GameObject.Find("PickupPromptUI");
        GameObject promptObj = GameObject.Find("PromptText");
        if (promptObj != null)
            promptText = promptObj.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
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
            ShowPrompt("Press E to use door");

            if (Input.GetKeyDown(interactKey))
            {
                CancelInvoke("HidePrompt");
                doorSound.Play();
                Invoke("NextScene", 1.5f);
            }
        }
        else
        {
            HidePrompt();
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

    void NextScene()
    {
        if(exitNumber < 0 || exitNumber > 3) return;
        Room nextRoom;
        if(exitNumber == 1) nextRoom = Room.currentRoom.GetExit1();
        if(exitNumber == 2) nextRoom = Room.currentRoom.GetExit2();
        if(exitNumber == 3) nextRoom = Room.currentRoom.GetExit3();

        Room.currentRoom.UseExit(exitNumber);
    }

}
