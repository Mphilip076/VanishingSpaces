using UnityEngine;

public class AltarManager : MonoBehaviour
{
    public static AltarManager Instance;

    public int totalAltars = 5;
    private int filledAltars = 0;

    [Header("Gem Reward")]
    public GameObject gemItemPrefab;
    public Sprite gemIcon;
    public string gemItemName = "Gem";

    void Awake()
    {
        Instance = this;
    }

    public void RegisterAltar()
    {
        filledAltars++;

        Debug.Log("Altars filled: " + filledAltars + "/" + totalAltars);

        if (filledAltars >= totalAltars)
        {
            CompletePuzzle();
        }
    }

    void CompletePuzzle()
    {
        Debug.Log("All altars filled. Adding gem to inventory.");

        if (InventoryManager.Instance == null)
            return;

        InventoryManager.Instance.AddItem(
            gemItemPrefab,
            gemIcon,
            gemItemName
        );

        InventoryManager.Instance.RefreshUI();
    }
}