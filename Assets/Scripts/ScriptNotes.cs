using UnityEngine;
using TMPro;

public class ScrollNote : InteractableItem
{
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
        interactRange = 3f;
        interactKey = KeyCode.E;
        interactMessage = "Press E to read";
    }

    public override void OnInteract()
    {
        if (!isOpen) OpenNote();
        else CloseNote();
    }

    void OpenNote()
    {
        if (notePanel == null) return;

        isOpen = true;
        notePanel.SetActive(true);

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