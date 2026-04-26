using System.Collections.Generic;
using UnityEngine;

public class Altar : InteractableItem
{
    private bool isFilled = false;
    private static Dictionary<string, int> altars 
                    = new Dictionary<string, int>();

    [Header("Visuals")]
    public string altarID;
    public GameObject placedLionVisual;
    public Renderer altarRenderer;
    public Color activeColor = Color.green;

    void Start()
    {
        // Ensure there is a key
        if (!altars.ContainsKey(altarID))
        {
            altars.Add(altarID, 0);
        }

        interactMessage = "Press E to place lion";
        interactRange = 3f;

        // Was there a statue?
        if(altars[altarID] == 0)
        {
            // The altar was empty
            if (placedLionVisual != null)
                placedLionVisual.SetActive(false);

            canInteract = true;
            isFilled = false;
        }
        else
        {
            // The altar had a statue
            if (placedLionVisual != null)
                placedLionVisual.SetActive(true);

            canInteract = false;
            isFilled = true;
        }

        
    }

    public override void OnInteract()
    {
        if (isFilled)
        {
            ShowShortMessage("This altar has a lion", 1);
            return;
        }

        if (InventoryManager.Instance == null)
            return;

        if (!InventoryManager.Instance.HasItem("Lion"))
        {
            ShowShortMessage("You need a lion", 2);
            return;
        }

        isFilled = true;
        canInteract = false;
        InventoryManager.Instance.RemoveItemByName("Lion");
        ActivateAltar();
    }

    void ActivateAltar()
    {
        if (placedLionVisual != null){
            placedLionVisual.SetActive(true);

        }

        if (altarRenderer != null)
        {
            altarRenderer.material.EnableKeyword("_EMISSION");
            altarRenderer.material.SetColor("_EmissionColor", activeColor);
        }

        altars[altarID] = 1;

        if (AltarManager.Instance != null)
            AltarManager.Instance.RegisterAltar();
    }
}