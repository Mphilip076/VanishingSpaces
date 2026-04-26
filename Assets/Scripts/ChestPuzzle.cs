using UnityEngine;
using TMPro;

public class ChestPuzzle : InteractableItem
{
    [Header("Passcode")]
    public string correctCode = "4920";

    [Header("Key Item")]
    public GameObject keyInsideChest;

    [Header("Gem Reward")]
    public GameObject gemItemPrefab;
    public Sprite gemIcon;
    public string gemItemName = "White Gem";

    [Header("UI")]
    public GameObject codeUI;
    public TMP_InputField codeInput;
    public TextMeshProUGUI feedbackText;

    private bool isOpen = false;
    private bool gemGiven = false;
    private AudioSource chestSound;
    private Animation chestAnimation;
    private static ChestPuzzle instance;

    void Start()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        chestSound = GetComponent<AudioSource>();
        chestAnimation = GetComponent<Animation>();

        interactKey = KeyCode.E;
        interactRange = 3f;
        interactMessage = "Press E to open";

        if (codeUI == null)
            codeUI = GameObject.Find("CodeBoxUI");

        if (codeUI != null)
            codeUI.SetActive(false);

        if (keyInsideChest != null)
            keyInsideChest.SetActive(false);

        if (PlayerPrefs.GetInt("chest_solved", 0) == 1)
        {
            ShowKey();

            if (chestAnimation != null)
                chestAnimation.Play("ChestAnim");

            gemGiven = true;
        }

        DontDestroyOnLoad(gameObject);
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

    void ShowKey()
    {
        if (keyInsideChest != null)
            keyInsideChest.SetActive(true);

        canInteract = false;
    }

    void GiveGemReward()
    {
        if (gemGiven) return;

        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager.Instance was not found. Gem was not added.");
            return;
        }

        InventoryManager.Instance.AddItem(
            gemItemPrefab,
            gemIcon,
            gemItemName
        );

        gemGiven = true;
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

            PlayerPrefs.SetInt("chest_solved", 1);
            PlayerPrefs.Save();

            GiveGemReward();

            Invoke("CloseCodeUI", 1.5f);
            Invoke("ShowKey", 2f);
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