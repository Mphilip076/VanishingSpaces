using UnityEngine;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.UIElements;

public class ToiletLid : InteractableItem
{
    static int position = 1;
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
        position++;
        if(position > 4) position = 1;

        if(position == 1)
        {
            transform.rotation = Quaternion.Euler(0, 90, 180);
        }else if(position == 2)
        {
            transform.rotation = Quaternion.Euler(0, 90, 210);
        }else if(position == 3)
        {
            transform.rotation = Quaternion.Euler(0, 90, 240);
        }else if(position == 4)
        {
            transform.rotation = Quaternion.Euler(0, 90, 270);
        }
    }

    public static int GetPosition()
    {
        return position;
    }
}
