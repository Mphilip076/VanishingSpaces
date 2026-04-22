using UnityEngine;

public class Battery : InteractableItem
{
    [Header("Battery Info")]
    public int minCharge = 20;
    public int maxCharge = 50;
    private int currentCharge;

    void Start()
    {
        currentCharge = Random.Range(minCharge, maxCharge);
        interactMessage = "Press E to pick up battery";
        interactRange = 3f;
        canInteract = true;
    }

    void Update()
    {
        if (!canInteract) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // Show different message if player doesn't have flashlight
                if (!FlashlightManager.hasFlashlight)
                {
                    interactMessage = "You need a flashlight first!";
                }
                else
                {
                    interactMessage = "Press E to pick up battery";
                }

                ShowPrompt();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    OnInteract();
                }
                return;
            }
        }

        PersistCanvas.HidePrompt();
    }

    public override void OnInteract()
    {
        // Block pickup if no flashlight
        if (!FlashlightManager.hasFlashlight)
        {
            return;
        }

        FlashlightManager.batteryLevel += currentCharge;
        if (FlashlightManager.batteryLevel > 100)
            FlashlightManager.batteryLevel = 100;

        Debug.Log("Battery picked up! +" + currentCharge + "%");
        PersistCanvas.HidePrompt();
        Destroy(gameObject);
    }
}