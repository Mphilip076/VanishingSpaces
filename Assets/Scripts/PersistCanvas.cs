using UnityEngine;
using TMPro;

public class PersistCanvas : MonoBehaviour
{
    private static PersistCanvas Instance;
    public GameObject pickupPromptUIObject;
    public TextMeshProUGUI promptTextObject;

    public static GameObject pickupPromptUI;
    public static TextMeshProUGUI promptText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (pickupPromptUIObject != null)
            pickupPromptUI = pickupPromptUIObject;
        else
            pickupPromptUI = GameObject.Find("PickupPromptUI");

        if (promptTextObject != null)
            promptText = promptTextObject;
        else
        {
            GameObject promptObj = GameObject.Find("PromptText");
            if (promptObj != null)
                promptText = promptObj.GetComponent<TextMeshProUGUI>();
        }

        if (pickupPromptUI != null)
            pickupPromptUI.SetActive(false);
    }

    public static void HidePrompt()
    {
        if (pickupPromptUI != null)
            pickupPromptUI.SetActive(false);
    }

    public static void ShowPrompt(string message)
    {
        if (pickupPromptUI == null || promptText == null)
        {
            Debug.LogError("PersistCanvas: PickupPromptUI or PromptText is not assigned!");
            return;
        }

        pickupPromptUI.SetActive(true);
        promptText.text = message;
    }
}