using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField] GameObject[] highlights;
    [SerializeField] Image[] slotIcons;
    [SerializeField] Transform hand;
    [SerializeField] private InputActionAsset controls;
    
    static readonly Key[] hotkeys = { Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4 };

    int selectedSlot;
    bool isSelected = false;
    
    ItemData[] items;
    GameObject selectedItem;
    
    InputAction drop;
    
    void Start()
    {
        for (int i = 0; i < hotkeys.Length; i++)
        {
            SlotIcon(i);
        }

        //for now it makes sure the correct highlight its showing when the game begins
        UpdateHighlighted();
    }

    void Update()
    {
        //checks both input methods
        NumberKeys();
        ScrollWheel();
        Dropping();
    }

    private void Awake()
    {
        items = new ItemData[hotkeys.Length];

        drop = controls.FindAction("Hotbar/Drop", true);
        Debug.Log(drop);
    }

    void OnEnable()
    {
        drop?.Enable();
    }
    void OnDisable()
    {
        drop?.Disable();
    }
    
    void NumberKeys()
    {
        Keyboard keybored = Keyboard.current;

        //if no number key pressed it leaves the function
        if (keybored == null) return;

        //loop that checks if each hotkey has been pressed
        for (int i = 0; i < hotkeys.Length; i++)
        {
            if (keybored[hotkeys[i]].wasPressedThisFrame)
            {
                Select(i);
            }
        }

    }

    void ScrollWheel()
    {
        Mouse mouse = Mouse.current;

        //if not scrolled it leaves the function
        if (mouse == null) return;

        float scroll = mouse.scroll.ReadValue().y;
        //Debug.Log(scroll);
        if (scroll > 0f) //moves the selected slot to the previous slot //scroll up
        {
            Select(selectedSlot - 1);

        }
        else if (scroll < 0f) //moves the selected slot to the next slot //scroll down
        {
            Select(selectedSlot + 1);
        }
    }

    void Select(int slot)
    {
        //selects the slot
        slot =  (slot + (highlights.Length)) % highlights.Length;

        if (isSelected && slot == selectedSlot)
        {
            isSelected = false;
        }
        else
        {
            selectedSlot = slot;
            isSelected = true;
        }
        
        UpdateHighlighted();

    }

    void UpdateHighlighted()
    {
        //changes what slot is highlighted
        for (int i = 0; i < highlights.Length; i++)
        {
            highlights[i].SetActive(isSelected && i == selectedSlot);
        }

        UpdateHeld();
    }
    
    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                UpdateHighlighted();
                SlotIcon(i);
                return true;
            }
        }

        return false;
    }

    void SlotIcon(int slot)
    {
        bool hasItem = items[slot];
        slotIcons[slot].enabled = hasItem;
        slotIcons[slot].sprite = hasItem ? items[slot].icon : null;
        
    }

    void UpdateHeld()
    {
        if (hand != null)
        {
            Destroy(selectedItem);
        }

        if (!isSelected)
        {
            return;
        }
        
        ItemData item = items[selectedSlot];
        if (item == null || item.held == null)
        {
            return;
        }

        selectedItem = Instantiate(item.held, hand);
        //selectedItem.transform.localPosition = Vector3.zero;
        //selectedItem.transform.localRotation = Quaternion.identity;

    }

    void Dropping()
    {
        if (drop == null)
        {
            return;
        }

        if (drop.WasPressedThisFrame())
        {
            Debug.Log("Dropping " + drop.name);
            DropHeld();
        }
    }

    void DropHeld()
    {
        if (!isSelected)
        {
            return;
        }

        ItemData item = items[selectedSlot];
        Transform cam = Camera.main.transform;
        Vector3 pos = cam.position;
        
        GameObject droppedItem = Instantiate(item.worldObject, pos, Quaternion.identity);
        
        Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(cam.transform.forward, ForceMode.Impulse);
        }
        
        //Clear and refresh
        items[selectedSlot] = null;
        SlotIcon(selectedSlot);
        UpdateHeld();

    }
    
}
