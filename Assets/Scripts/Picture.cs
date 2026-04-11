using Unity.VisualScripting;
using UnityEngine;

public class Picture : PickableItem
{
    [Header("Picture Info")]
    public Vector3 correctPosition; // Position where the picture should be placed

    [Header("Distance Threshold for Placement")]
    public static float distanceThreshold = 2f;

    [Header("Puzzle State")]
    public static Vector3 pos1;
    public static bool pos1inUse = false;
    public static Vector3 pos2;
    public static bool pos2inUse = false;
    public static Vector3 pos3;
    public static bool pos3inUse = false;


    // Check if the picture is placed in the correct position
    public bool CheckCorrectPlacement()
    {
        return gameObject.transform.position == correctPosition;
    }

    // Override the OnDrop method to implement snapping behavior
    public void OnDrop()
    {
        if (!pos1inUse) {
            // Check if dropped near pos1
            float distanceToPos1 = Vector3.Distance(transform.position, pos1);
            if (distanceToPos1 < distanceThreshold){
                base.OnDrop();
                gameObject.transform.position = pos1;
                pos1inUse = true;

                Debug.Log("[" + name + "] Dropped near pos1, snapping to position 1.");
                return;
            }
        }else if (!pos2inUse) {
            // Check if dropped near pos2
            float distanceToPos2 = Vector3.Distance(transform.position, pos2);
            if (distanceToPos2 < distanceThreshold){
                base.OnDrop();
                gameObject.transform.position = pos2;
                pos2inUse = true;
                Debug.Log("[" + name + "] Dropped near pos2, snapping to position 2.");
                return;
            }
        }else if (!pos3inUse) {
            // Check if dropped near pos3
            float distanceToPos3 = Vector3.Distance(transform.position, pos3);
            if (distanceToPos3 < distanceThreshold){
                base.OnDrop();
                gameObject.transform.position = pos3;
                pos3inUse = true;
                Debug.Log("[" + name + "] Dropped near pos3, snapping to position 3.");
                return;
            }
        }

        // If not dropped near any of the positions, don't snap
        base.OnDrop();
    }

}
