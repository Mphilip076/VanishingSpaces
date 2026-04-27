using UnityEngine;
using UnityEngine.InputSystem;

public class Lighter : PickableItem
{
    private static bool exists = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        if (exists)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        exists = true;
    }

    void OnDestroy()
    {
        exists = false;
    }
}
