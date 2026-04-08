using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Slots (assign 10 of each)")]
    public Image[] slotIcons;
    public Image[] slotBackgrounds;
    public TextMeshProUGUI[] slotKeys;

    [Header("Selection Colors")]
    public Color selectedColor = new Color(1f, 1f, 1f, 0.4f);
    public Color normalColor = new Color(0f, 0f, 0f, 0.6f);

    private GameObject[] heldItems = new GameObject[10];
    private Sprite[] heldItemIcons = new Sprite[10];  // store icons separately
    private string[] heldItemNames = new string[10];
    private int selectedSlot = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject); // persist the whole canvas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        foreach (var icon in slotIcons)
            icon.enabled = false;

        slotBackgrounds[selectedSlot].color = selectedColor;
    }

    void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectSlot(i);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
            SelectSlot(9);
    }

    void SelectSlot(int index)
    {
        slotBackgrounds[selectedSlot].color = normalColor;
        selectedSlot = index;
        slotBackgrounds[selectedSlot].color = selectedColor;

        ItemPickup itemPickup = FindAnyObjectByType<ItemPickup>();
        if (itemPickup != null)
            itemPickup.TurnOffFlashlight();
    }

    public bool AddItem(GameObject item, Sprite icon, string itemName)
    {
        for (int i = 0; i < heldItems.Length; i++)
        {
            if (heldItems[i] == null)
            {
                heldItems[i] = item;
                heldItemNames[i] = itemName;
                heldItemIcons[i] = icon;  // store icon separately
                slotIcons[i].sprite = icon;
                slotIcons[i].enabled = true;
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(GameObject item)
    {
        for (int i = 0; i < heldItems.Length; i++)
        {
            if (heldItems[i] == item)
            {
                heldItems[i] = null;
                heldItemNames[i] = null;
                heldItemIcons[i] = null;
                slotIcons[i].sprite = null;
                slotIcons[i].enabled = false;
                return;
            }
        }
    }

    // Call this after scene loads to refresh UI icons
    public void RefreshUI()
    {
        for (int i = 0; i < heldItems.Length; i++)
        {
            if (heldItemIcons[i] != null)
            {
                slotIcons[i].sprite = heldItemIcons[i];
                slotIcons[i].enabled = true;
            }
            else
            {
                slotIcons[i].sprite = null;
                slotIcons[i].enabled = false;
            }
        }

        for (int i = 0; i < slotBackgrounds.Length; i++)
            slotBackgrounds[i].color = normalColor;

        slotBackgrounds[selectedSlot].color = selectedColor;
    }

    public bool HasItem(string itemName)
    {
        for (int i = 0; i < heldItemNames.Length; i++)
        {
            if (heldItemNames[i] == itemName)
                return true;
        }
        return false;
    }

    public GameObject GetItemAtSlot(int index)
    {
        if (index < 0 || index >= heldItems.Length) return null;
        return heldItems[index];
    }

    public GameObject GetSelectedItem()
    {
        return heldItems[selectedSlot];
    }
}