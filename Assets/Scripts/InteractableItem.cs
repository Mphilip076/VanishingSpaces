using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class InteractableItem : MonoBehaviour
{
    [Header("Interactable Item Settings")]
    public KeyCode interactKey = KeyCode.E;
    public string interactMessage = "Press E to interact";
    public float interactRange = 3f;
    public bool canInteract = true;
    public bool isPickable = false;

    private string tempMessage;

    public virtual void OnInteract()
    {
        // Override with the item's function
    }

    public void ShowPrompt()
    {
        PersistCanvas.ShowPrompt(interactMessage);
    }    

    public void ShowShortMessage(string message, int time)
    {
        tempMessage = interactMessage;
        interactMessage = message;
        // Show the prompt with the temp messge
        ShowPrompt();

        // Show the original message in time seconds
        Invoke("ResetMessage", time);
    }

    private void ResetMessage()
    {
        PersistCanvas.HidePrompt();
        interactMessage = tempMessage;
    }

}