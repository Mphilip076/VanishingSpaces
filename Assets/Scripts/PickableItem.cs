using UnityEngine;

public class PickableItem : InteractableItem
{
    [Header("Item Info")]
    public string itemName = "Item";
    public Sprite itemIcon;
    public bool isFlashlight = false;
    public bool canDrop = true;
    public bool canPickUp = true;

    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Start()
    {
        interactMessage = "Press E to pick up";
        interactKey = KeyCode.E;
        isPickable = true;
    }

    public virtual void OnPickup(Transform holdPosition)
    {
        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;
        
        transform.SetParent(holdPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public virtual void OnDrop()
    {
        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;

        transform.SetParent(null);
    }
}