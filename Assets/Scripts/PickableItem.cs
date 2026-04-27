using UnityEngine;

public class PickableItem : InteractableItem
{
    [Header("Pickable Item Settings")]
    public string itemName = "Item";
    public Sprite itemIcon;
    public bool isFlashlight = false;
    public bool canPickUp = true;

    [Header("Flashlight Settings")]
    public Light flashlightLight;
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
        // Player already has the flashlight from a previous pickup — remove this scene copy
        if (isFlashlight && FlashlightManager.hasFlashlight)
        {
            Destroy(gameObject);
            return;
        }

        interactMessage = "Press E to pick up";
        interactKey = KeyCode.E;
        isPickable = true;

        if (isFlashlight && flashlightLight != null)
            flashlightLight.enabled = false;
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
        canInteract = false;
        canPickUp = false;

        if (isFlashlight)
        {
            FlashlightManager.OnFlashlightPickup();
        }

        LionWhisper whisper = GetComponent<LionWhisper>();
        if (whisper != null) whisper.enabled = false;
        
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null) audio.Stop();
    }
}