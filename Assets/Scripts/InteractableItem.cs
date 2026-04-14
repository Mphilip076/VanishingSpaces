using UnityEngine;
using TMPro;

public class InteractableItem : MonoBehaviour
{
    [Header("Interactable Item Settings")]
    public KeyCode interactKey = KeyCode.E;
    public float interactRange = 3f;
    public bool canInteract = true;
    public bool isPickable = false;


    [Header("UI")]
    public GameObject pickupPromptUI;
    public TextMeshProUGUI promptText;
    public string interactMessage;
    public virtual void Start()
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

    public virtual void OnInteract()
    {
        // Override with the item's function
    }

    public void ShowPrompt()
    {
        ShowPrompt(interactMessage);
    }

    public void ShowPrompt(string message)
    {
        pickupPromptUI.SetActive(true);
        if(promptText != null)
            promptText.text = message;
    }

    public void HidePrompt()
    {
        pickupPromptUI.SetActive(false);
    }

}