using UnityEngine;

public class FallenVase : InteractableItem
{
    public static bool isUpright;
    private static FallenVase instance = null;

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

        interactMessage = "Press E to set upright";
        isUpright = false;
    }

    public override void OnInteract()
    {
        gameObject.transform.position = new Vector3(-10.850459098815918f, 2.5157351046800616e-7f, -19.1200008392334f);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        isUpright = true;
        canInteract = false;
    }
}
