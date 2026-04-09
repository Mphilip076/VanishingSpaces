using UnityEngine;

public class PicturePuzzle : MonoBehaviour
{
    [Header("Picture References")]
    public Picture pictureA;
    public Picture pictureB;
    public Picture pictureC;
    private bool solved = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Current positions
        pictureA.currentPosition = pictureA.transform; // Initialize current position to the starting position of the picture
        pictureB.currentPosition = pictureB.transform; // Initialize current position to the starting position of
        pictureC.currentPosition = pictureC.transform; // Initialize current position to the starting position of the picture

        // Possible positions
        Picture.pos1 = pictureA.currentPosition;
        Picture.pos2 = pictureB.currentPosition;
        Picture.pos3 = pictureC.currentPosition;

        // Correct positions
        pictureA.correctPosition = pictureB.correctPosition; // Set the correct position for picture A
        pictureB.correctPosition = pictureC.correctPosition; // Set the correct position for picture B
        pictureC.correctPosition = pictureA.correctPosition; // Set the correct position for picture C
    }

    // Update is called once per frame
    void Update()
    {
        if(solved) return;

        if(pictureA.CheckCorrectPlacement() && pictureB.CheckCorrectPlacement() && pictureC.CheckCorrectPlacement())
        {
            solved = true;
            Debug.Log("[Picture Puzzle] Puzzle Solved!");
        }


    }
}
