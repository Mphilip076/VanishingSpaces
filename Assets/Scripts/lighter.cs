using UnityEngine;
using UnityEngine.InputSystem;

public class Lighter : PickableItem
{
    private Lighter instance = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        if(instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        interactKey = KeyCode.E;
        interactMessage = "Press E to pick up";
        interactRange = 3f;
        canInteract = true;
        canPickUp = true;
    }
}
