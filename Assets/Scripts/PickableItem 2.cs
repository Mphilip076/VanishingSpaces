using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName = "Item";
    public Sprite itemIcon;
    public bool isFlashlight = false;

    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void OnPickup(Transform holdPosition)
    {
        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;

        transform.SetParent(holdPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void OnDrop()
    {
        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;

        transform.SetParent(null);
    }
}