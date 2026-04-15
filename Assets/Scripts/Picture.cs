using UnityEngine;

public class Picture : PickableItem
{
    public static float snapDistance = 7f;

    [Header("Pictures")]
    public static Picture A = null;
    public static Picture B = null;
    public static Picture C = null;

    public static PictureSlot slot1;
    public static PictureSlot slot2;
    public static PictureSlot slot3;

    public override void Start()
    {      
        base.Start();  

        // Prevent duplicates
        if(this.name == "Picture A" && A == null)
        {
            Debug.Log("[Picture] Assigned Picture A");
            A = this;
            DontDestroyOnLoad(this);
        }
        else if(this.name == "Picture B" && B == null)
        {
            Debug.Log("[Picture] Assigned Picture B");
            B = this;
            DontDestroyOnLoad(this);
        }
        else if(this.name == "Picture C" && C == null)
        {
            Debug.Log("[Picture] Assigned Picture C");
            C = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("[Picture] Duplicate picture found, destroying");
            Destroy(gameObject);
        }
    }

    public static bool CanPlace()
    {
        if(Room.currentRoom.SceneName() != "LivingRoom") return false;
        if(slot1.inUse && slot2.inUse && slot3.inUse) return false;

        // Does the player have a painting
        Picture p = null;

        if(InventoryManager.Instance.HasItem("Picture A")) p = A;
        else if(InventoryManager.Instance.HasItem("Picture B")) p = B;
        else if(InventoryManager.Instance.HasItem("Picture C")) p = C;
        else return false; // No

        // Location 1
        if(slot1.inUse == false && Vector3.Distance(p.transform.position, slot1.transform.position) <= snapDistance) return true;
        if(slot2.inUse == false && Vector3.Distance(p.transform.position, slot2.transform.position) <= snapDistance) return true;
        if(slot3.inUse == false && Vector3.Distance(p.transform.position, slot3.transform.position) <= snapDistance) return true;

        return false;
    }

    private bool GoToPosition(int slotNum)

    {
        if(slotNum == 1 && slot1.inUse) return false;
        if(slotNum == 2 && slot2.inUse) return false;
        if(slotNum == 3 && slot3.inUse) return false;

        switch(slotNum)
        {
            case 1:
                base.OnDrop();
                transform.position = slot1.transform.position;
                transform.rotation = slot1.transform.rotation;
                slot1.inUse = true;
                slot1.placeSound.Play();
                return true;
            case 2:
                base.OnDrop();
                transform.position = slot2.transform.position;
                transform.rotation = slot2.transform.rotation;
                slot2.inUse = true;
                slot2.placeSound.Play();
                return true;
            case 3:
                base.OnDrop();
                transform.position = slot3.transform.position;
                transform.rotation = slot3.transform.rotation;
                slot3.inUse = true;
                slot3.placeSound.Play();
                return true;


        }

        return false;
    }

    private bool WithinDistance(int posNum)
    {
        if(posNum < 1 || posNum > 3) return false;

        Vector3 location = Vector3.zero;

        switch(posNum)
        {
            case 1:
                location = slot1.transform.position;
                break;
            case 2:
                location = slot2.transform.position;
                break;
            case 3:
                location = slot3.transform.position;
                break;
        }

        return Vector3.Distance(transform.position, location) <= snapDistance;
    }

    public override void OnPickup(Transform holdPosition)
    {
        Debug.Log("[Picture] hold = " + holdPosition);
        // Mark position as not in use when picked up

        if(Room.currentRoom.SceneName() == "LivingRoom"){
            if(transform.position == slot1.transform.position) slot1.inUse = false;
            if(transform.position == slot2.transform.position) slot2.inUse = false;
            if(transform.position == slot3.transform.position) slot3.inUse = false;
        }

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
                        return;
                    }
                }
            }
        }        
    }
}