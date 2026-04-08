using UnityEngine;
using TMPro;

public class ChestPuzzle : MonoBehaviour
{
    [Header("Settings")]
    public string correctCode = "1234";
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.O;

    [Header("Key Item to Give Player")]
    public Sprite keyIcon;
    public string keyItemName = "Key";

    [Header("UI")]
    public GameObject codeUI;
    public TMP_InputField codeInput;
    public TextMeshProUGUI feedbackText;

    // [Header("Chest Animation")]
    // public Animation chestAnimation;

    private bool isOpen = false;
    private bool isSolved = false;
    private Transform player;
    private Animation chestAnimation;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        chestAnimation = GetComponent<Animation>();

        if (codeUI != null)
            codeUI.SetActive(false);
    }

    void Update()
    {
        if (isSolved) return;
        if (player == null)
        {
            Debug.Log("Player is NULL!");
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance to chest: " + distance);

        if (distance < interactRange && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Key pressed near chest!");
            if (!isOpen)
                OpenCodeUI();
            else
                CloseCodeUI();
        }
    }

    void OpenCodeUI()
    {
        isOpen = true;
        codeUI.SetActive(true);
        feedbackText.text = "";
        codeInput.text = "";
        codeInput.ActivateInputField();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = false;
    }

    void GiveKey()
    {
        InventoryManager.Instance.AddItem(
            gameObject,
            keyIcon,
            keyItemName
        );
    }

    void CloseCodeUI()
    {
        isOpen = false;
        codeUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = true;
    }

    public void SubmitCode()
    {
        if (codeInput.text == correctCode)
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;

            isSolved = true;

            // Play chest open animation
            if (chestAnimation != null)
                chestAnimation.Play("ChestAnim");

            // Close UI after a short delay
            Invoke("CloseCodeUI", 1.5f);

            // Give key after animation finishes
            Invoke("GiveKey", 2f);
        }
        else
        {
            feedbackText.text = "Incorrect!";
            feedbackText.color = Color.red;
            codeInput.text = "";

            // Close UI after showing incorrect message
            Invoke("CloseCodeUI", 1f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}