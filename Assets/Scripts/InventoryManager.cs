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
    private string[] heldItemNames = new string[10];
    private int selectedSlot = 0;

    void Awake()
    {
        Instance = this;
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
                slotIcons[i].sprite = null;
                slotIcons[i].enabled = false;
                return;
            }
        }
    }

    public GameObject GetSelectedItem()
    {
        return heldItems[selectedSlot];
    }
}