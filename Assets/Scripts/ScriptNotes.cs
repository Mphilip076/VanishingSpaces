using UnityEngine;
using TMPro;

public class ScrollNote : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Content")]
    [TextArea(3, 10)]
    public string noteText = "The code is 1234";

    [Header("UI")]
    public GameObject notePanel;
    public TextMeshProUGUI noteTextUI;

    private bool isOpen = false;

    void Start()
    {
        if (notePanel != null)
            notePanel.SetActive(false);
    }

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange);
        bool playerNearby = false;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerNearby = true;
                break;
            }
        }

        if (playerNearby && Input.GetKeyDown(interactKey))
        {
            if (!isOpen)
                OpenNote();
            else
                CloseNote();
        }
    }

    void OpenNote()
    {
        if (notePanel == null) return;

        isOpen = true;
        notePanel.SetActive(true);

        if (noteTextUI != null)
            noteTextUI.text = noteText;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = false;
    }

    void CloseNote()
    {
        if (notePanel == null) return;

        isOpen = false;
        notePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = true;
    }
}