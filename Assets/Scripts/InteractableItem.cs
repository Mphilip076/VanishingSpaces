using UnityEngine;
using TMPro;

public class InteractableItem : MonoBehaviour
{
    [Header("Interactable Item Settings")]
    public KeyCode interactKey = KeyCode.E;
    public string interactMessage = "Press E to interact";
    public float interactRange = 3f;
    public bool canInteract = true;
    public bool isPickable = false;

    public virtual void OnInteract()
    {
        // Override with the item's function
    }

    public void ShowPrompt()
    {
        PersistCanvas.ShowPrompt(interactMessage);
    }    
}