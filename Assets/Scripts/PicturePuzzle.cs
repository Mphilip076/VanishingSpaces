//using UnityEditor.VersionControl;
using UnityEngine;

public class PicturePuzzle : MonoBehaviour
{

    [Header("Completion Action")]
    public GameObject completionReward;
    public Sprite completionRewardSprite;
    public string completionRewardName;
    public AudioSource completeSound;

    private static bool isSolved;
    private static PicturePuzzle instance;

    void Start()
    {
        if(instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        isSolved = false;
    }

    void Update()
    {
        if(isSolved) return;

        CheckPuzzle();
    }

    public bool PuzzleSolved()
    {
        return isSolved;
    }

    private void CheckPuzzle()
    {
        // make sure everything exists
        if( Picture.A == null || Picture.B == null || Picture.C == null ||
            PictureSlot.s1 == null || PictureSlot.s2 == null || PictureSlot.s3 == null)
        {
            return;
        }

        // Slot 1 = moon
        // Slot 2 = star
        // Slot 3 = sun

        // Pic A = moon
        // Pic B = star 
        // Pic C = sun

        if( PictureSlot.s1.placed.gameObject == Picture.A.gameObject &&
            PictureSlot.s2.placed.gameObject == Picture.B.gameObject &&
            PictureSlot.s3.placed.gameObject == Picture.C.gameObject )
        {
            OnSolve();
        }
    }

    private void OnSolve()
    {
        isSolved = true;
        Debug.Log("[PicturePuzzle] Puzzle Solved!");

        // Make sure these items cannot be moved!
        Picture.A.canPickUp = false;
        Picture.A.canInteract = false;
        Picture.B.canPickUp = false;
        Picture.B.canInteract = false;
        Picture.C.canPickUp = false;
        Picture.C.canInteract = false;
        PictureSlot.s1.canInteract = false;
        PictureSlot.s2.canInteract = false;
        PictureSlot.s3.canInteract = false;
        
        // Give the player the reward
        completeSound.Play();
        Invoke("Givegem", 1.5f);
    }

    void Givegem()
    {
        InventoryManager.Instance.AddItem(completionReward, completionRewardSprite, completionRewardName);
    }
}
