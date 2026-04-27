using UnityEngine;

public class BathroomPuzzle : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public int correctToiletPosition;
    public ToiletLid toilet;
    public int numRolls;

    [Header("Item Settings")]
    public GameObject rewardObject;
    public Sprite rewardSprite;
    public string rewardName;
    public AudioSource completeSound;

    private static BathroomPuzzle instance = null;
    public static bool puzzleComplete = false;
    private bool gemGiven = false;

    void Start()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!puzzleComplete)
            CheckPuzzleCompletion();
        else if (!gemGiven)
            TryGiveGem();
    }

    void CheckPuzzleCompletion()
    {
        if (puzzleComplete) return;
        if (!FallenVase.isUpright) return;
        if (ToiletRoll.rollsMoved != numRolls) return;
        if (ToiletLid.GetPosition() != correctToiletPosition) return;

        OnSolve();
    }

    void OnSolve()
    {
        puzzleComplete = true;
        if (completeSound != null)
            completeSound.Play();

        toilet.canInteract = false;
        Invoke("TryGiveGem", 2f);

        Debug.Log("[BathroomPuzzle] Puzzle Complete!");
    }

    void TryGiveGem()
    {
        if (gemGiven) return;
        if (InventoryManager.Instance == null) return;

        bool added = InventoryManager.Instance.AddItem(rewardObject, rewardSprite, rewardName);
        if (added)
        {
            gemGiven = true;
            Debug.Log("[BathroomPuzzle] Gem added to inventory.");
        }
    }
}
