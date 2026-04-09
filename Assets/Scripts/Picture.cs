using Unity.VisualScripting;
using UnityEngine;

public class Picture : PickableItem
{
    [Header("Picture Info")]
    public Transform currentPosition; // Position where the picture starts in
    public Transform correctPosition; // Position where the picture should be placed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPosition = this.transform; // Initialize current position to the starting position of the picture
    }

    // Check if the picture is placed in the correct position
    public bool CheckCorrectPlacement()
    {
        if(currentPosition == correctPosition)
        {
            isPlacedCorrectly = true; // Set the flag to true if the picture is in the correct position
            Debug.Log("[Picture Puzzle]Picture " + base.itemName + " is placed correctly!"); // Log a message for debugging purposes
        }
        else
        {
            isPlacedCorrectly = false; // Set the flag to false if the picture is not in the correct position
        }
    }

    public bool CheckPuzzleCompletion()
    {
        if(GameObject.Find("Picture A").GetComponent<Picture>().CheckCorrectPlacement()) return false;
        if(GameObject.Find("Picture B").GetComponent<Picture>().CheckCorrectPlacement()) return false;
        if(GameObject.Find("Picture C").GetComponent<Picture>().CheckCorrectPlacement()) return false;

        return true;
    }


    // Update is called once per frame
    void Update()
    {
        if(base.itemName.Equals("Picture A"))
            CheckPuzzleCompletion();
    }
}
