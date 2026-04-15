using UnityEngine;
using TMPro;

public class ChestPuzzle : InteractableItem
{
    [Header("Passcode")]
    public string correctCode = "1234";

    [Header("Key Item to Give Player")]
    public GameObject keyPrefab;
    public Sprite keyIcon;
    public string keyItemName = "Key";

    [Header("UI")]
    public GameObject codeUI;
    public TMP_InputField codeInput;
    public TextMeshProUGUI feedbackText;

    // Private variables 
    private bool isOpen = false;
    private AudioSource chestSound;
    private Animation chestAnimation;

    void Start()
    {
        chestSound = GetComponent<AudioSource>();
        chestAnimation = GetComponent<Animation>();

        interactKey = KeyCode.E;
        interactRange = 2f;
        interactMessage = "Press E to open";

        if (codeUI == null)
            codeUI = GameObject.Find("CodeBoxUI");

        if (codeUI != null)
            codeUI.SetActive(false);
    }

    public override void OnInteract()
    {
        if (!isOpen)
            OpenCodeUI();
        else
            CloseCodeUI();
    }

    void OpenCodeUI()
    {
        if (codeUI == null) return;

        isOpen = true;
        codeUI.SetActive(true);

        if (feedbackText != null)
            feedbackText.text = "";

        if (codeInput != null)
        {
            codeInput.text = "";
            codeInput.ActivateInputField();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = false;
    }

    void CloseCodeUI()
    {
        if (codeUI == null) return;

        isOpen = false;
        codeUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = true;
    }

    void GiveKey()
    {
        if (keyPrefab == null) return;

        InventoryManager.Instance.AddItem(
            keyPrefab,
            keyIcon,
            keyItemName
        );

        canInteract = false;
    }

    public void SubmitCode()
    {
        if (codeInput == null) return;

        if (codeInput.text == correctCode)
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Correct!";
                feedbackText.color = Color.green;
            }

            if (chestAnimation != null)
                chestAnimation.Play("ChestAnim");

            if (chestSound != null)
                chestSound.Play();

            Invoke("CloseCodeUI", 1.5f);
            Invoke("GiveKey", 2f);
        }
        else
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Incorrect!";
                feedbackText.color = Color.red;
            }

            if (codeInput != null)
                codeInput.text = "";

            Invoke("CloseCodeUI", 1f);
        }
    }
}