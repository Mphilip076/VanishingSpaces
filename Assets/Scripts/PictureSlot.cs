using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Data;

public class PictureSlot : InteractableItem
{
    public PickableItem placed;
    public AudioSource placeSound;

    public static PictureSlot s1;
    public static PictureSlot s2;
    public static PictureSlot s3;

    void Start()
    {    // Prevent duplicates
        if(this.name == "Slot1" && s1 == null)
        {
            s1 = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(this.name == "Slot2" && s2 == null)
        {
            s2 = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(this.name == "Slot3" && s3 == null)
        {
            s3 = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("[PictureSlot] Duplicate picture found, destroying");
            Destroy(gameObject);
        }

        interactKey = KeyCode.R;
        interactRange = 4f;
        interactMessage = "Press R to place an item";
        placed = null;
    }

    public override void OnInteract()
    {
        if(placed == null) Place();
        else PickUp();
    }


    void PickUp()
    {
        if(placed == null) return;
        
        // Try to add the item to the inventory
        bool added = InventoryManager.Instance.AddItem(placed.gameObject, placed.itemIcon, placed.itemName);

        // Failed
        if(!added) return;

        // Move the item
        placed.OnPickup(GameObject.Find("HoldPosition").transform);

        // Undo the changes on placing:
        placed.canPickUp = true;
        placed.canInteract = true;
        interactKey = KeyCode.R;
        interactMessage = "Press R to place an item";
        
        // Finally, reset placed:
        placed = null;
    }

    void Place()
    {
        InventoryManager i = InventoryManager.Instance;
        GameObject toPlace = i.GetSelectedItem();

        if(placed != null)
        {
            Debug.LogWarning("[PictureSlot] Player is trying to place an item but placed is not null");
            return;
        }

        // Is the player holding something?
        if(toPlace == null){ 
            Debug.Log("[PictureSlot] Currently held item null");
            return;
        }

        // Can that item be placed?
        PickableItem item = toPlace.GetComponentInParent<PickableItem>();
        if(item == null){ 
            Debug.Log("[PictureSlot] Could not retrieve PickableItem component");
            return;
        }

        // Cant that item be dropped?
        if(!item.canDrop || item.isFlashlight){
            Debug.Log("[PictureSlot] Player is trying to place in item that cannot leave the inventory");
            return;
        }

        // "drop" the item
        item.OnDrop();

        // Set the position and rotation to match this
        item.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);

        // Remove the item from the inventory        
        InventoryManager.Instance.RemoveItem(toPlace);

        // Make sure we know it's here
        placed = item;

        // You can't place an item anymore
        // Make it look like the player is interacting with the item
        // even though they're interacting with this:
        interactKey = KeyCode.E;
        interactMessage = "Press E to pick up";
        // The keycode being different prevents place->immediate pickup

        // Void the item's message:
        placed.canPickUp = false;
        placed.canInteract = false;

        Debug.Log("[PictureSlot] Placing " + toPlace.name);
    }

}
