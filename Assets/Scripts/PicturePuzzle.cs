using UnityEngine;

public class PicturePuzzle : MonoBehaviour
{
    [Header("Picture Puzzle Settings")]
    public Vector3 pictureALocation;
    public Vector3 pictureBLocation;
    public Vector3 pictureCLocation;
    
    public GameObject completionReward;
    public Sprite completionRewardSprite;
    public string completionRewardName;

    private bool isSolved;
    

    void Start()
    {
        isSolved = false;

        pictureCLocation = Picture.pos1; // sun
        pictureALocation = Picture.pos2; // moon
        pictureBLocation = Picture.pos3; // star
    }

    void Update()
    {
        if(isSolved) return;

        CheckPuzzle();
    }

    public void CheckPuzzle()
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

    public void OnSolve()
    {
        isSolved = true;
        Debug.Log("[PicturePuzzle] Puzzle Solved!");
        Picture.A.canPickUp = false;
        Picture.B.canPickUp = false;
        Picture.C.canPickUp = false;
        InventoryManager.Instance.AddItem(completionReward, completionRewardSprite, completionRewardName);
    }
}
