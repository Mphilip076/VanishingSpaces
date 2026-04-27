//using UnityEditor.VersionControl;
using UnityEngine;

public class PicturePuzzle : MonoBehaviour
{
    [Header("Completion Action")]
    public GameObject completionReward;
    public Sprite completionRewardSprite;
    public string completionRewardName;
    public AudioSource completeSound;

    private static bool isSolved = false;
    private static PicturePuzzle instance;
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
        if (!isSolved)
            CheckPuzzle();
        else if (!gemGiven)
            TryGiveGem();
    }

    public bool PuzzleSolved()
    {
        return isSolved;
    }

    private void CheckPuzzle()
    {
        if (isSolved) return;

        if (Picture.A == null || Picture.B == null || Picture.C == null ||
            PictureSlot.s1 == null || PictureSlot.s2 == null || PictureSlot.s3 == null)
            return;

        // Slot 1 = moon (Pic A), Slot 2 = star (Pic B), Slot 3 = sun (Pic C)
        if (PictureSlot.s1.placed.gameObject == Picture.A.gameObject &&
            PictureSlot.s2.placed.gameObject == Picture.B.gameObject &&
            PictureSlot.s3.placed.gameObject == Picture.C.gameObject)
        {
            OnSolve();
        }
    }

    private void OnSolve()
    {
        isSolved = true;
        Debug.Log("[PicturePuzzle] Puzzle Solved!");

        Picture.A.canPickUp = false;
        Picture.A.canInteract = false;
        Picture.B.canPickUp = false;
        Picture.B.canInteract = false;
        Picture.C.canPickUp = false;
        Picture.C.canInteract = false;
        PictureSlot.s1.canInteract = false;
        PictureSlot.s2.canInteract = false;
        PictureSlot.s3.canInteract = false;

        completeSound.Play();
        Invoke("TryGiveGem", 1.5f);
    }

    void TryGiveGem()
    {
        if (gemGiven) return;
        if (InventoryManager.Instance == null) return;

        bool added = InventoryManager.Instance.AddItem(completionReward, completionRewardSprite, completionRewardName);
        if (added)
        {
            gemGiven = true;
            Debug.Log("[PicturePuzzle] Gem added to inventory.");
        }
    }

    void OnDestroy()
    {
        isSolved = false;
    }
}
