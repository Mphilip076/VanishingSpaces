using UnityEngine;

public class Altar : InteractableItem
{
    private bool isFilled = false;

    [Header("Visuals")]
    public GameObject placedLionVisual;
    public Renderer altarRenderer;
    public Color activeColor = Color.green;

    void Start()
    {
        interactMessage = "Press E to place lion";
        isPickable = false;
        canInteract = true;

        if (placedLionVisual != null)
            placedLionVisual.SetActive(false);
    }

    public override void OnInteract()
    {
        if (isFilled)
        {
            ShowShortMessage("Already filled", 1);
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
        if (placedLionVisual != null)
            placedLionVisual.SetActive(true);

        if (altarRenderer != null)
        {
            altarRenderer.material.EnableKeyword("_EMISSION");
            altarRenderer.material.SetColor("_EmissionColor", activeColor);
        }

        if (AltarManager.Instance != null)
            AltarManager.Instance.RegisterAltar();
    }
}