using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    [Header("Settings")]
    public Transform holdPosition;
    public float pickupRange = 3f;

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

        if (Input.GetKeyDown(dropKey) && heldItem != null)
            DropItem();

        if (Input.GetKeyDown(flashlightKey))
            TryToggleFlashlight();
    }

    void CheckNearbyItems()
    {
        if (pickupPromptUI == null) return;

        Collider[] hits = Physics.OverlapSphere(
            transform.position, pickupRange
        );

        PickableItem nearest = null;
        float nearestDistance = float.MaxValue;
        ChestPuzzle nearestChest = null;
        DoorLock nearestDoor = null;
        ScrollNote nearestScroll = null;

        foreach (var hit in hits)
        {
            ChestPuzzle chest = hit.GetComponentInParent<ChestPuzzle>();
            if (chest != null)
            {
                nearestChest = chest;
                break;
            }

            DoorLock door = hit.GetComponentInParent<DoorLock>();
            if (door != null)
            {
                nearestDoor = door;
                break;
            }

            ScrollNote scroll = hit.GetComponentInParent<ScrollNote>();
            if (scroll != null)
            {
                nearestScroll = scroll;
                break;
            }

            if (hit.CompareTag("Interactable")) continue;

            PickableItem item = hit.GetComponent<PickableItem>();
            if (item != null && item.canPickUp)
            {
                float distance = Vector3.Distance(
                    transform.position, hit.transform.position
                );

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = item;
                }
            }
        }

        if (nearestChest != null)
        {
            pickupPromptUI.SetActive(true);
            if (promptText != null)
                promptText.text = "Press O to unlock";
        }
        else if (nearestDoor != null)
        {
            pickupPromptUI.SetActive(true);
            if (promptText != null)
                promptText.text = "Press E to unlock";
        }
        else if (nearestScroll != null)
        {
            pickupPromptUI.SetActive(true);
            if (promptText != null)
                promptText.text = "Press E to read";
        }
        else if (nearest != null)
        {
            pickupPromptUI.SetActive(true);
            if (promptText != null)
            {
                if (nearest.isFlashlight && heldItem == null)
                    promptText.text = "Press E to hold";
                else
                    promptText.text = "Press E to pick up";
            }
        }
        else
        {
            pickupPromptUI.SetActive(false);
        }
    }

    void TryPickUp()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position, pickupRange
        );

        PickableItem nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Interactable")) continue;

            PickableItem item = hit.GetComponent<PickableItem>();
            if (item != null && item.canPickUp)
            {
                float distance = Vector3.Distance(
                    transform.position, hit.transform.position
                );

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = item;
                }
            }
        }

        if (nearest == null) return;

        if (nearest.isFlashlight && heldItem == null)
        {
            PickUp(nearest);
        }
        else if (!nearest.isFlashlight)
        {
            bool added = InventoryManager.Instance.AddItem(
                nearest.gameObject,
                nearest.itemIcon,
                nearest.itemName
            );

            if (added)
            {
                nearest.GetComponent<Collider>().enabled = false;
                nearest.gameObject.SetActive(false);
            }
        }
    }

    void PickUp(PickableItem item)
    {
        bool added = InventoryManager.Instance.AddItem(
            item.gameObject,
            item.itemIcon,
            item.itemName
        );

        if (!added) return;

        heldItem = item;
        item.OnPickup(holdPosition);

        if (item.isFlashlight)
        {
            heldLight = item.GetComponentInChildren<Light>(true);
            flashlightOn = false;
            if (heldLight != null)
                heldLight.enabled = false;
        }
    }

    void DropItem()
    {
        if (heldItem != null)
        {
            GameObject selectedItem = InventoryManager.Instance.GetSelectedItem();
            if (selectedItem == heldItem.gameObject)
            {
                if(heldItem.canDrop == false) return;

                if (heldLight != null)
                {
                    heldLight.enabled = false;
                    heldLight = null;
                    flashlightOn = false;
                }

                InventoryManager.Instance.RemoveItem(heldItem.gameObject);
                heldItem.OnDrop();
                heldItem = null;
                return;
            }
        }

        GameObject selected = InventoryManager.Instance.GetSelectedItem();
        if (selected == null) return;
        
        // Don't drop the pictures
        if(selected.name == "Picture A" || selected.name == "Picture B" || selected.name == "Picture C") return;

        InventoryManager.Instance.RemoveItem(selected);

        selected.transform.position = transform.position + transform.forward * 1.5f;
        selected.transform.rotation = Quaternion.identity;
        selected.SetActive(true);

        Rigidbody rb = selected.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
        }
        Collider col = selected.GetComponent<Collider>();
        if (col != null) col.enabled = true;
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