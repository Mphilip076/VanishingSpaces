using Unity.VisualScripting;
using UnityEngine;

public class Picture : PickableItem
{
    [Header("Picture Info")]
    public Transform currentPosition; // Position where the picture starts in
    public Transform correctPosition; // Position where the picture should be placed

    [Header("Distance Threshold for Placement")]
    public float distanceThreshold = 2f;

    [Header("Puzzle State")]
    public static Transform pos1;
    public static bool pos1inUse = true;
    public static Transform pos2;
    public static bool pos2inUse = true;
    public static Transform pos3;
    public static bool pos3inUse = true;


    // Check if the picture is placed in the correct position
    public bool CheckCorrectPlacement()
    {
        return currentPosition.position == correctPosition.position;
    }

    public bool CheckPuzzleCompletion()
    {
        if(!GameObject.Find("Picture A").GetComponent<Picture>().CheckCorrectPlacement()) return false;
        if(!GameObject.Find("Picture B").GetComponent<Picture>().CheckCorrectPlacement()) return false;
        if(!GameObject.Find("Picture C").GetComponent<Picture>().CheckCorrectPlacement()) return false;

        return true;
    }

    public new void OnDrop()
    {
        if (!pos1inUse) {
            // Check if dropped near pos1
            float distanceToPos1 = Vector3.Distance(transform.position, pos1.position);
            if (distanceToPos1 < distanceThreshold){
                base.OnDrop();
                currentPosition = pos1;
                pos1inUse = true;

                Debug.Log("[" + name + "] Dropped near pos1, snapping to position 1.");
                return;
            }
        }else if (!pos2inUse) {
            // Check if dropped near pos2
            float distanceToPos2 = Vector3.Distance(transform.position, pos2.position);
            if (distanceToPos2 < distanceThreshold){
                base.OnDrop();
                currentPosition = pos2;
                pos2inUse = true;
                Debug.Log("[" + name + "] Dropped near pos2, snapping to position 2.");
                return;
            }
        }else if (!pos3inUse) {
            // Check if dropped near pos3
            float distanceToPos3 = Vector3.Distance(transform.position, pos3.position);
            if (distanceToPos3 < distanceThreshold){
                base.OnDrop();
                currentPosition = pos3;
                pos3inUse = true;
                Debug.Log("[" + name + "] Dropped near pos3, snapping to position 3.");
                return;
            }
        }

        // If not dropped near any of the positions, do nothing
    }

}
