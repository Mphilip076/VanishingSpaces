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
    public volatile bool playerNearby = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorSound = GetComponent<AudioSource>();
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
            if (Input.GetKeyDown(interactKey))
            {
                doorSound.Play();
                Invoke(nameof(NextScene), 1.5f);
            }
        }
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
