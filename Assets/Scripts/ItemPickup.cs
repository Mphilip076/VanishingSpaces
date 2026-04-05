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

        if (Input.GetKeyDown(pickupKey) && heldItem == null)
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

        foreach (var hit in hits)
        {
            PickableItem item = hit.GetComponent<PickableItem>();
            if (item != null)
            {
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
        Collider[] hits = Physics.OverlapSphere(
            transform.position, pickupRange
        );

        foreach (var hit in hits)
        {
            PickableItem item = hit.GetComponent<PickableItem>();
            if (item != null)
            {
                PickUp(item);
                break;
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

        // Keep item alive across scenes!
        DontDestroyOnLoad(item.gameObject);

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
        if (heldItem == null) return;

        if (heldLight != null)
        {
            heldLight.enabled = false;
            heldLight = null;
            flashlightOn = false;
        }

        InventoryManager.Instance.RemoveItem(heldItem.gameObject);
        heldItem.OnDrop();
        heldItem = null;
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