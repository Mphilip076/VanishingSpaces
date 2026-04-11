using UnityEngine;

public class PicturePuzzle : MonoBehaviour
{
    [Header("Picture References")]
    public Picture pictureA;
    public Picture pictureB;
    public Picture pictureC;
    private bool solved = false;


    // The users need to find the pictures and place them in the correct positions to solve the puzzle.
    // The pictures can be picked up and moved around, and 
    //      they will snap to the correct positions when dropped near them.


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the static position references in the Picture class
        Picture.pos1 = new Vector3(-4.80f, 1.60f, 3.35f);
        Picture.pos2 = new Vector3(-4.12f, 1.60f, 11.07f); 
        Picture.pos3 = new Vector3(0.18f, 1.52f, 7.05f); 
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
