//using UnityEditor.VersionControl;
using UnityEngine;

public class PicturePuzzle : MonoBehaviour
{
    [Header("Picture Puzzle Settings")]
    public Vector3 pictureALocation;
    public Vector3 pictureBLocation;
    public Vector3 pictureCLocation;

    [Header("Picture Slots")]
    public PictureSlot slot1;
    public PictureSlot slot2;
    public PictureSlot slot3;

    [Header("Completion Action")]
    public GameObject completionReward;
    public Sprite completionRewardSprite;
    public string completionRewardName;
    public AudioSource completeSound;

    private bool isSolved;
    

    void Start()
    {
        isSolved = false;

        pictureCLocation = slot1.transform.position; // sun
        pictureALocation = slot2.transform.position; // moon
        pictureBLocation = slot3.transform.position; // star

        slot1.inUse = false;
        slot2.inUse = false;
        slot3.inUse = false;
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
        if(Picture.A == null || Picture.B == null || Picture.C == null){
            Debug.LogWarning("[PicturePuzzle] One or more pictures are missing!");
            return;
        }

        Debug.Log("[PicturePuzzle] Checking puzzle, A at " + Picture.A.transform.position + ", B at " + Picture.B.transform.position + ", C at " + Picture.C.transform.position);
        Debug.Log("[PicturePuzzle] Target positions: A at " + pictureALocation + ", B at " + pictureBLocation + ", C at " + pictureCLocation);

        if(Picture.A.transform.position == pictureALocation &&
           Picture.B.transform.position == pictureBLocation &&
           Picture.C.transform.position == pictureCLocation)
        {
            OnSolve();
        }
    }

    private void OnSolve()
    {
        isSolved = true;
        Debug.Log("[PicturePuzzle] Puzzle Solved!");
        Picture.A.canPickUp = false;
        Picture.B.canPickUp = false;
        Picture.C.canPickUp = false;
        InventoryManager.Instance.AddItem(completionReward, completionRewardSprite, completionRewardName);
    }
}
