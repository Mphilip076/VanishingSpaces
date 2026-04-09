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
        return currentPosition.position == correctPosition.position;
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
