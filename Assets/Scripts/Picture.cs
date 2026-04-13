using Unity.VisualScripting;
using UnityEngine;

public class Picture : PickableItem
{
    public static Picture A;
    public static Picture B;
    public static Picture C;

    [Header("Positions")]
    public static float snapDistance = 5f;


    // Static positions for the pictures
    public static Vector3 pos1 = new Vector3(-4.12f, 1.60f, 3.35f); // sun
    public static Vector3 pos2 = new Vector3(-4.12f, 1.60f, 11.07f); // moon
    public static Vector3 pos3 = new Vector3(0.18f, 1.52f, 7.05f); //star
    public static bool pos1inUse = false;
    public static bool pos2inUse = false;
    public static bool pos3inUse = false;


    void Start()
    {
        // Prevent duplicates
        if(this.name == "Picture A" && A == null)
        {
            Debug.Log("[Picture] Assigned Picture A");
            A = this;
        }
        else if(this.name == "Picture B" && B == null)
        {
            Debug.Log("[Picture] Assigned Picture B");
            B = this;
        }
        else if(this.name == "Picture C" && C == null)
        {
            Debug.Log("[Picture] Assigned Picture C");
            C = this;
        }
        else
        {
            Debug.Log("[Picture] Duplicate picture found, destroying");
            Destroy(gameObject);
        }

        canPickUp = true;
        canDrop = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Room.currentRoom.SceneName() == "LivingRoom")
        {
            SnapDrop();
        }
    }

    private void SetRotation(int i)
    {
        if(i == 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        if(i == 1)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if(i == 2)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if(i == 3)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }

    private bool GoToPosition(int posNum)
    {
        if(posNum == 1 && pos1inUse) return false;
        if(posNum == 2 && pos2inUse) return false;
        if(posNum == 3 && pos3inUse) return false;

        switch(posNum)
        {
            case 1:
                base.OnDrop();
                SetRotation(1);
                transform.position = pos1;
                pos1inUse = true;
                return true;
            case 2:
                base.OnDrop();
                SetRotation(2);
                transform.position = pos2;
                pos2inUse = true;
                return true;
            case 3:
                base.OnDrop();
                SetRotation(3);
                transform.position = pos3;
                pos3inUse = true;
                return true;
        }

        return false;
    }

    public static bool CanPlace()
    {
        if(Room.currentRoom.SceneName() != "LivingRoom") return false;
        if(pos1inUse && pos2inUse && pos3inUse) return false;

        // Does the player have a painting
        Picture p = null;

        if(InventoryManager.Instance.HasItem("Picture A")) p = A;
        else if(InventoryManager.Instance.HasItem("Picture B")) p = B;
        else if(InventoryManager.Instance.HasItem("Picture C")) p = C;
        else return false; // No

        // Location 1
        if(pos1inUse == false && Vector3.Distance(p.transform.position, pos1) <= snapDistance) return true;
        if(pos2inUse == false && Vector3.Distance(p.transform.position, pos2) <= snapDistance) return true;
        if(pos3inUse == false && Vector3.Distance(p.transform.position, pos3) <= snapDistance) return true;

        return false;        
    }

    private bool WithinDistance(int posNum)
    {
        if(posNum < 1 || posNum > 3) return false;

        Vector3 location = Vector3.zero;

        switch(posNum)
        {
            case 1:
                location = pos1;
                break;
            case 2:
                location = pos2;
                break;
            case 3:
                location = pos3;
                break;
        }

        return Vector3.Distance(transform.position, location) <= snapDistance;
    }

    public override void OnPickup(Transform holdPosition)
    {
        // Mark position as not in use when picked up
        if(transform.position == pos1) pos1inUse = false;
        if(transform.position == pos2) pos2inUse = false;
        if(transform.position == pos3) pos3inUse = false;

        base.OnPickup(holdPosition);
    }

    public void SnapDrop()
    {
        if(Room.currentRoom.SceneName() == "LivingRoom")
        {
            // Snap to correct position
            // Is it within range?
            for(int i = 1; i <= 3; i++)
            {
                // Check if it's within distance of the position
                if(WithinDistance(i)) {
                    // Try to snap to position, if it's not in use
                    // GoToPosition calls base.OnDrop() if successful
                    if(GoToPosition(i)) {
                        // Successfully snapped to position
                        Debug.Log("[Picture]Snapped to position " + i);
                        InventoryManager.Instance.RemoveItem(gameObject);
                        return;
                    }
                }
            }

            // Not within range of any position, or position is occupied
            // Do nothing
        }
    }
}
