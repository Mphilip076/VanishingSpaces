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
    private Sprite[] heldItemIcons = new Sprite[10];
    private string[] heldItemNames = new string[10];
    private int selectedSlot = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Print()
    {
        Debug.Log("[Inventory Manager] Printing Inventory");
        for(int i = 0; i < 10; i++)
        {
            if(heldItems[i] == null) Debug.Log($"[Inventory Manager] Slot {i} is empty");
            else Debug.Log($"[Inventory Manager] Slot {i} contains {heldItemNames[i]}");
        }
        Debug.Log("[Inventory Manager] Done.");
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
    }

    public void Hide()
    {
        Canvas canvas = transform.root.GetComponent<Canvas>();
        if (canvas != null) canvas.enabled = false;
    }

    public void Show()
    {
        Canvas canvas = transform.root.GetComponent<Canvas>();
        if (canvas != null) canvas.enabled = true;
    }

    void SelectSlot(int index)
    {
        slotBackgrounds[selectedSlot].color = normalColor;
        selectedSlot = index;
        slotBackgrounds[selectedSlot].color = selectedColor;
    }

    public bool AddItem(GameObject item, Sprite icon, string itemName)
    {
        Debug.Log("[Inventory Manager] Attempting to add item " + itemName);
        for (int i = 0; i < heldItems.Length; i++)
        {
            if(heldItemNames[i] != null) Debug.Log($"[Inventory Manager] HeldItems[{i}] = {heldItemNames[i]}");
            if (heldItems[i] == null && heldItemNames[i] == null)
            {
                heldItems[i] = item;
                heldItemNames[i] = itemName;
                heldItemIcons[i] = icon;
                slotIcons[i].sprite = icon;
                slotIcons[i].enabled = true;
                Debug.Log("[Inventory Manager] Item " + item.name + " added at index " + i);
                Print();
                return true;
            }
        }

        Debug.Log("[Inventory Manager] failed to add item " + item.name);
        Print();
        return false;
    }

    public void RemoveItem(GameObject item)
    {
        Debug.Log("[Inventory Manager] Removing item " + item.name);
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

    // public void RemoveItemByName(string name)
    // {
    //     Debug.Log("[Inventory Manager] Removing item " + name);
    //     for (int i = 0; i < heldItems.Length; i++)
    //     {
    //         if(heldItems[i] == null) continue;
    //         if (heldItemNames[i] == name)
    //         {
    //             heldItems[i] = null;
    //             heldItemNames[i] = null;
    //             heldItemIcons[i] = null;
    //             slotIcons[i].sprite = null;
    //             slotIcons[i].enabled = false;
    //             return;
    //         }
    //     }
    // }
    public void RemoveItemByName(string name){
        Debug.Log("[Inventory Manager] Removing item " + name);

        for (int i = 0; i < heldItemNames.Length; i++)
        {
            if (heldItemNames[i] == name)
            {
                heldItems[i] = null;
                heldItemNames[i] = null;
                heldItemIcons[i] = null;

                slotIcons[i].sprite = null;
                slotIcons[i].enabled = false;

                Debug.Log("[Inventory Manager] Removed " + name + " from slot " + i);
                return;
            }
        }

        Debug.Log("[Inventory Manager] Could not find item named " + name);
    }

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

    public GameObject GetItemByName(string name)
    {
        foreach(var item in heldItems)
        {
            if(item.name == name)
            {
                return item;
            }
        }

        return null;
    }

    public GameObject GetSelectedItem()
    {
        return heldItems[selectedSlot];
    }
}