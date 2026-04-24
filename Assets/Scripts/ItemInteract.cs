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

    // Private stuff:
    private InteractableItem closest = null;

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

        item.OnPickup(holdPosition);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxInteractRange);
    }
}

   