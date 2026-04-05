using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CodeBox : MonoBehaviour
{
    [Header("Settings")]
    public string correctCode = "123";
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Key Item to Give Player")]
    public Sprite keyIcon;
    public string keyItemName = "Key";

    [Header("UI")]
    public GameObject codeUI;
    public TMP_InputField codeInput;
    public TextMeshProUGUI feedbackText;

    private bool isOpen = false;
    private bool isSolved = false;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (codeUI != null)
            codeUI.SetActive(false);
    }

    void Update()
    {
        if (isSolved) return;

        if (player == null)
        {
            Debug.Log("Player is NULL in CodeBox!");
            return;
        }

        float distance = Vector3.Distance(
            transform.position, player.position
        );

        Debug.Log("Distance to box: " + distance + " | Is open: " + isOpen);

        if (distance < interactRange && Input.GetKeyDown(interactKey))
        {
            Debug.Log("E pressed near box!");
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

        // Disable player movement
        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = false;
    }

    void CloseCodeUI()
    {
        isOpen = false;
        codeUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Re-enable player movement
        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = true;
    }

    public void SubmitCode()
    {
        if (codeInput.text == correctCode)
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;

            // Give player the key in inventory
            InventoryManager.Instance.AddItem(
                gameObject,
                keyIcon,
                keyItemName
            );

            // Hide the box
            Invoke("HideBox", 1f);
        }
        else
        {
            feedbackText.text = "Wrong code!";
            feedbackText.color = Color.red;
            codeInput.text = "";
        }
    }

    void HideBox()
    {
        CloseCodeUI();
        gameObject.SetActive(false); // box disappears!
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}