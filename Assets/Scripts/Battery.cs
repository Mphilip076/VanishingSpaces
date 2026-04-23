using UnityEngine;

public class Battery : InteractableItem
{
    [Header("Battery Info")]
    public int minCharge = 20;
    public int maxCharge = 50;
    private int currentCharge;

    [Header("Identity")]
    public string batteryID; // Give each battery a unique ID in Inspector!

    void Start()
    {
        // If this battery was already picked up, destroy it immediately
        if (PlayerPrefs.GetInt(batteryID, 0) == 1)
        {
            Destroy(gameObject);
            return;
        }

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
                if (!FlashlightManager.hasFlashlight)
                    interactMessage = "You need a flashlight first!";
                else
                    interactMessage = "Press E to pick up battery";

                ShowPrompt();

                if (Input.GetKeyDown(KeyCode.E))
                    OnInteract();

                return;
            }
        }

        PersistCanvas.HidePrompt();
    }

    public override void OnInteract()
    {
        if (!FlashlightManager.hasFlashlight)
            return;

        FlashlightManager.batteryLevel += currentCharge;
        if (FlashlightManager.batteryLevel > 100)
            FlashlightManager.batteryLevel = 100;

        // Save that this battery has been picked up
        PlayerPrefs.SetInt(batteryID, 1);
        PlayerPrefs.Save();

        Debug.Log("Battery picked up! +" + currentCharge + "%");
        PersistCanvas.HidePrompt();
        Destroy(gameObject);
    }
}