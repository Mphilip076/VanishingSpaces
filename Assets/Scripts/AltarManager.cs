using UnityEngine;

public class AltarManager : MonoBehaviour
{
    public static AltarManager Instance;

    public int totalAltars = 5;
    public int filledAltars = 0;
    private bool isComplete = false;
    private bool gemGiven = false;

    [Header("Gem Reward")]
    public GameObject gemItemPrefab;
    public Sprite gemIcon;
    public string gemItemName = "White Gem";
    public AudioSource completionSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isComplete && !gemGiven)
            TryGiveGem();
    }

    public void RegisterAltar()
    {
        if (isComplete) return;

        filledAltars++;
        Debug.Log("Altars filled: " + filledAltars + "/" + totalAltars);

        if (filledAltars == totalAltars)
        {
            if (completionSound != null)
                completionSound.Play();

            isComplete = true;
            Invoke("CompletePuzzle", 2);
        }
        else if (filledAltars > totalAltars)
        {
            Debug.LogWarning("There are more filled altars than total altars!");
        }
    }

    void CompletePuzzle()
    {
        Debug.Log("All altars filled. Attempting to add gem to inventory.");
        TryGiveGem();
    }

    void TryGiveGem()
    {
        if (InventoryManager.Instance == null) return;

        bool added = InventoryManager.Instance.AddItem(gemItemPrefab, gemIcon, gemItemName);
        if (added)
        {
            gemGiven = true;
            Debug.Log("[AltarManager] Gem added to inventory.");
        }
    }
}
