using UnityEngine;

public class PickableItem : InteractableItem
{
    [Header("Pickable Item Settings")]
    public string itemName = "Item";
    public Sprite itemIcon;
    public bool isFlashlight = false;
    public bool canDrop = true;
    public bool canPickUp = true;

    [Header("Flashlight Settings")]
    public Light flashlightLight;
    private bool isFlashlightOn = false;
    private bool isPickedUp = false;

    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public virtual void Start()
    {
        interactMessage = "Press E to pick up";
        interactKey = KeyCode.E;
        isPickable = true;

        // Make sure flashlight starts off
        if (isFlashlight && flashlightLight != null)
            flashlightLight.enabled = false;
    }

    void Update()
    {
        // Only toggle if this item is a flashlight AND it has been picked up
        if (isFlashlight && isPickedUp && Input.GetKeyDown(KeyCode.F))
        {
            if (flashlightLight != null)
            {
                isFlashlightOn = !isFlashlightOn;
                flashlightLight.enabled = isFlashlightOn;
            }
        }
    }

    public virtual void OnPickup(Transform holdPosition)
    {
        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;

        transform.SetParent(holdPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Mark as picked up so flashlight toggle works
        isPickedUp = true;
    }

    public virtual void OnDrop()
    {
        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;

        transform.SetParent(null);

        // Mark as dropped so flashlight toggle stops working
        isPickedUp = false;

        // Turn off flashlight when dropped
        if (isFlashlight && flashlightLight != null)
        {
            isFlashlightOn = false;
            flashlightLight.enabled = false;
        }
    }
}