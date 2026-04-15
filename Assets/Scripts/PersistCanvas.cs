using UnityEngine;
using TMPro;

public class PersistCanvas : MonoBehaviour
{
    private static PersistCanvas Instance;
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
        if (pickupPromptUI == null)
            pickupPromptUI = GameObject.Find("PickupPromptUI");

        if (promptText == null)
        {
            GameObject promptObj = GameObject.Find("PromptText");
            if (promptObj != null)
                promptText = promptObj.GetComponent<TextMeshProUGUI>();
        }
    }

    public static void HidePrompt()
    {
        if (pickupPromptUI != null)
            pickupPromptUI.SetActive(false);
    }

    public static void ShowPrompt(string message)
    {
        pickupPromptUI.SetActive(true);
        promptText.text = message;
    }
}