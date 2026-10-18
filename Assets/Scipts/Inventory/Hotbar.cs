using UnityEngine;
using UnityEngine.InputSystem;

public class Hotbar : MonoBehaviour
{
    [SerializeField] GameObject[] highlights;
    
    static readonly Key[] hotkeys = { Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4 };

    int selectedSlot;
    public bool isSelected = false;

    void Start()
    {
        //for now it makes sure the correct highlight its showing when the game begins
        UpdateHighlighted();
    }

    void Update()
    {
        //checks both input methods
        NumberKeys();
        ScrollWheel();
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
    }
    
}
