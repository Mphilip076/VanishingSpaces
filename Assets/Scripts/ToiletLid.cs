using UnityEngine;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.UIElements;

public class ToiletLid : InteractableItem
{
    static bool isOpen = false;
    private static ToiletLid instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        interactMessage = "Press E to move lid";
        interactRange = 4f;
    }

    public override void OnInteract()
    {
        if (isOpen)
        {
            // close
            transform.rotation = Quaternion.Euler(0, 90, 270);
        }
        else
        {
            // open
            transform.rotation = Quaternion.Euler(0, 90, 180);
        }

        isOpen = !isOpen;
    }

    public static bool IsOpen()
    {
        return isOpen;
    }
}
