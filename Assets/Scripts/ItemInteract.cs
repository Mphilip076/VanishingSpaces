using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using System.Runtime.InteropServices;

public class ItemInteract : MonoBehaviour
{
    [Header("Settings")]
    public Transform holdPosition;
    public float maxInteractRange = 3f;

    [Header("Keys")]
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;

    // Private stuff:
    private InteractableItem closest = null;

    void Start()
    {
        
    }

    void Update()
    {
        CheckNearbyItems();

        if(closest == null)
        {
            // No interactable items nearby
            // Hide the prompt
            PersistCanvas.HidePrompt();
        }
        else
        {
            Debug.Log("[ItemInteract] Closest = " + closest.name);
            // there is an item
            closest.ShowPrompt(); 

            if (closest.isPickable)
            {
                // A pickable item
                if (Input.GetKeyDown(pickupKey))
                {
                    // Attempt to pick up the item
                    TryPickUp();
                }
            } 
            else
            {
                // Not pickable, but it is interactable            
                // Test for their interaction
                if (Input.GetKeyDown(closest.interactKey))
                {
                    // Interact, then put the little window away
                    closest.OnInteract();
                }
            }
        }        

        // Item dropping is independent of interact
        if (Input.GetKeyDown(dropKey) && InventoryManager.Instance.GetSelectedItem() != null)
            DropItem();
    }

    // Check a radius of maxInteractRange from the player's position for items that can be interacted with
    // Update the closest item the player can interact with
    void CheckNearbyItems()
    {
        // Check around the player for collisions
        Collider[] hits = Physics.OverlapSphere(transform.position, maxInteractRange);

        InteractableItem nearest = null;
        float nearestDistance = float.MaxValue;

        // For each collision
        foreach(var hit in hits)
        {
            InteractableItem item = hit.GetComponentInParent<InteractableItem>();

            if(item != null && item.canInteract)
            {
                // A valid interact

                // Compute the distance
                float distance = Vector3.Distance(
                    transform.position, hit.transform.position
                );

                // Check if it is the closest, and whether the player is close enough to interact
                if (distance < nearestDistance && item.interactRange >= distance)
                {
                    // This one is the closest!
                    nearestDistance = distance;
                    nearest = item;
                }
            }
        }

        // Set the closest item
        closest = nearest; // even if null
    }

    void TryPickUp()
    {
        Debug.Log("[ItemInteract] in trypickup");
        if(closest == null) return; // nothing to interact with
        if(!closest.isPickable) return; // not a PickableItem

        PickableItem nearest = closest.GetComponentInParent<PickableItem>();

        if(nearest == null) return; // Somehow not a PickableItem?
        if(!nearest.canPickUp) return; // Doesn't want to be picked up

        PickUp(nearest);
    }

    void PickUp(PickableItem item)
    {
        bool added = InventoryManager.Instance.AddItem(
            item.gameObject,
            item.itemIcon,
            item.itemName
        );

        if (!added) return;

        Debug.Log("[ItemInteract] hold = " + holdPosition + " and " + (holdPosition == null));
        item.OnPickup(holdPosition);

        Debug.Log("[ItemInteract] Picked up successfully");
    }

    void DropItem()
    {
        Debug.Log("[ItemInteract] Dropping item");

        // What are we dropping?
        GameObject selectedItem = InventoryManager.Instance.GetSelectedItem();
        if(selectedItem == null) return; // nothing to drop

        PickableItem item = selectedItem.GetComponentInParent<PickableItem>();
        if(item != null)
        {
            // This is a pickable item
            if(!item.canDrop) return; // Don't drop this...

            // Drop it!
            item.OnDrop();
            InventoryManager.Instance.RemoveItem(selectedItem);
            Debug.Log("[ItemInteract] Dropping " + selectedItem.name);
        }

        // Not a pickable item ->
        // If you drop it, you cannot get it back?! ->
        // Don't drop it.
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxInteractRange);
    }
}

   