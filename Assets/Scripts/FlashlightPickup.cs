using UnityEngine;

public class FlashlightPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform holdPosition;    // Assign HoldPosition empty object
    public float pickupRange = 2.5f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;
    public KeyCode flashlightKey = KeyCode.F;

    [Header("UI Prompt (optional)")]
    public GameObject pickupPromptUI; // Assign a UI text like "Press E to pick up"

    private GameObject heldItem = null;
    private Light heldLight = null;
    private bool flashlightOn = false;

    void Update()
    {
        CheckForPickup();

        if (Input.GetKeyDown(pickupKey) && heldItem == null)
            TryPickUp();

        if (Input.GetKeyDown(dropKey) && heldItem != null)
            DropItem();

        if (Input.GetKeyDown(flashlightKey) && heldLight != null)
            ToggleFlashlight();
    }

    void CheckForPickup()
    {
        // Show/hide pickup prompt
        if (pickupPromptUI == null) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);
        bool nearPickupable = false;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Pickupable"))
            {
                nearPickupable = true;
                break;
            }
        }

        pickupPromptUI.SetActive(nearPickupable);
    }

    void TryPickUp()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Pickupable"))
            {
                PickUpItem(hit.gameObject);
                break;
            }
        }
    }

    void PickUpItem(GameObject item)
    {
        heldItem = item;

        // Disable physics while held
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Disable trigger so it doesnt re-trigger pickup
        Collider col = item.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Attach to hold position
        item.transform.SetParent(holdPosition);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        // Get light component
        heldLight = item.GetComponentInChildren<Light>();

        Debug.Log("Picked up: " + item.name);
    }

    void DropItem()
    {
        if (heldItem == null) return;

        // Re-enable physics
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        // Re-enable collider
        Collider col = heldItem.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        // Turn off light when dropped
        if (heldLight != null) heldLight.enabled = false;
        flashlightOn = false;

        // Detach from player
        heldItem.transform.SetParent(null);
        heldItem = null;
        heldLight = null;

        Debug.Log("Dropped item!");
    }

    void ToggleFlashlight()
    {
        if (heldLight == null) return;

        flashlightOn = !flashlightOn;
        heldLight.enabled = flashlightOn;

        Debug.Log("Flashlight: " + (flashlightOn ? "ON" : "OFF"));
    }

    // Visualize pickup range in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}