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
    }

    // Update is called once per frame
    void Update()
    {
        if(puzzleComplete) return;

        CheckPuzzleCompletion();
    }

    void CheckPuzzleCompletion()
    {
        if(puzzleComplete) return;
        if(!FallenVase.isUpright) return;
        if(ToiletRoll.rollsMoved != numRolls) return;
        if(ToiletLid.GetPosition() != correctToiletPosition) return;

        OnSolve();
    }

    void OnSolve()
    {
        puzzleComplete = true;
        if(completeSound != null)
            completeSound.Play();

        Invoke("GiveGem", 2);
        toilet.canInteract = false;

        Debug.Log("[BathroomPuzzle] Puzzle Complete!");
    }

    void GiveGem()
    {
        InventoryManager.Instance.AddItem(rewardObject, rewardSprite, rewardName);
    }
}
