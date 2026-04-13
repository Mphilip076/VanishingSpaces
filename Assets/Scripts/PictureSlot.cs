using UnityEngine;
using TMPro;

public class PictureSlot : MonoBehaviour
{
    public bool inUse;
    public AudioSource placeSound;
    GameObject pickupPromptUI;
    TextMeshProUGUI promptText;


    void Start()
    {
        pickupPromptUI = GameObject.Find("InteractPromptUI");
        GameObject promptObj = GameObject.Find("InteractText");
        if (promptObj != null)
            promptText = promptObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 3f);
        bool playerNearby = false;

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
            ShowPrompt("Press R to place a picture");

            if (Input.GetKeyDown(KeyCode.R))
            {
                CancelInvoke("HidePrompt");
            }
        }
        else
        {
            HidePrompt();
        }
    }

    void ShowPrompt(string message)
    {
        if (pickupPromptUI != null) pickupPromptUI.SetActive(true);
        if (promptText != null) promptText.text = message;
    }

    void HidePrompt()
    {
        if (pickupPromptUI != null) pickupPromptUI.SetActive(false);
    }
}
