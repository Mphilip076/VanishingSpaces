using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ItemPickup : MonoBehaviour
{
    [Header("Settings")]
    public Transform holdPosition;
    public float pickupRange = 4f;

    [Header("Keys")]
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;
    public KeyCode flashlightKey = KeyCode.F;

    [Header("UI")]
    public GameObject pickupPromptUI;
    public TextMeshProUGUI promptText;

    private PickableItem heldItem = null;
    private Light heldLight = null;
    private bool flashlightOn = false;

    void Start()
    {
        if (pickupPromptUI == null)
            pickupPromptUI = GameObject.Find("PickupPromptUI");

        if (promptText == null)
        {
            GameObject promptObj = GameObject.Find("PromptText");
            if (promptObj != null)
                promptText = promptObj.GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        CheckNearbyItems();

        if (Input.GetKeyDown(pickupKey))
            TryPickUp();

        if (Input.GetKeyDown(dropKey))
            DropItem();

        if (Input.GetKeyDown(flashlightKey))
            TryToggleFlashlight();

        heldItem = InventoryManager.Instance.GetSelectedItem()?.GetComponent<PickableItem>();
    }

    void CheckNearbyItems()
    {
        if (pickupPromptUI == null) return;

        Collider[] hits = Physics.OverlapSphere(
            transform.position, pickupRange
        );

        PickableItem nearest = null;

        foreach (var hit in hits)
        {
            // Skip objects tagged as Interactable!
            if (hit.CompareTag("Interactable")) continue;

            PickableItem item = hit.GetComponent<PickableItem>();
            if (item != null)
            {
                Debug.Log("[ItemPickup] Detected nearby object: " + hit.name);
                nearest = item;
                break;
            }
        }

        if (nearest != null)
        {
            pickupPromptUI.SetActive(true);
            if (promptText != null)
                promptText.text = "Press E to pick up";
        }
        else
        {
            pickupPromptUI.SetActive(false);
        }
    }

    void TryPickUp()
    {
        Debug.Log("[ItemPickup] Attempting to pick up items");
        Collider[] hits = Physics.OverlapSphere(
            transform.position, pickupRange
        );

        foreach (var hit in hits)
        {
            Debug.Log("[ItemPickup] Checking nearby object: " + hit.name);
            if (hit.CompareTag("Pickupable")) {
                PickableItem item = hit.GetComponent<PickableItem>();
                Debug.Log("[ItemPickup] Found pickable item: " + hit.name);

                if (item != null)
                {
                    PickUp(item);
                    break;
                }
            }
        }
    }

    void PickUp(PickableItem item)
    {
        Debug.Log("[ItemPickup] Attempting to pick up item: " + item.name);
        bool added = InventoryManager.Instance.AddItem(
            item.gameObject,
            item.itemIcon,
            item.itemName
        );

        if (!added){ 
            Debug.Log("[ItemPickup] Failed to pick up item: " + item.name);
            return;
        }

        Debug.Log("[ItemPickup] Picked up: " + item.name);

        heldItem = item;
        item.OnPickup(holdPosition);

        if (item.isFlashlight)
        {
            heldLight = item.GetComponentInChildren<Light>(true);
            flashlightOn = false;
            if (heldLight != null)
                heldLight.enabled = false;
        }

        InventoryManager.Instance.Print();
    }

    void DropItem()
    {
        if (heldItem == null)
        {
            Debug.LogWarning("[ItemPickup] No item to drop!");
            return;
        }

        if (heldLight != null)
        {
            heldLight.enabled = false;
            heldLight = null;
            flashlightOn = false;
        }

        InventoryManager.Instance.RemoveItem(heldItem.gameObject);
        heldItem.OnDrop();
        heldItem = null;
        Debug.Log("[ItemPickup] Item dropped.");
    }

    void TryToggleFlashlight()
    {
        GameObject selectedItem = InventoryManager.Instance.GetSelectedItem();
        if (selectedItem == null) return;

        PickableItem pickable = selectedItem.GetComponent<PickableItem>();
        if (pickable != null && pickable.isFlashlight)
            ToggleFlashlight();
    }

    void ToggleFlashlight()
    {
        if (heldLight == null) return;
        flashlightOn = !flashlightOn;
        heldLight.enabled = flashlightOn;
    }

    public void TurnOffFlashlight()
    {
        if (heldLight != null && flashlightOn)
        {
            flashlightOn = false;
            heldLight.enabled = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}