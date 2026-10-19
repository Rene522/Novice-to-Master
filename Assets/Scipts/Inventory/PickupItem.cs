using Unity.VisualScripting;
using UnityEngine;

public class PickupItem : Interactable
{
    [SerializeField] private ItemData item;
    
    Hotbar hotbar;
    
    void Start()
    {
        hotbar = GameObject.Find("Hotbar").GetComponent<Hotbar>();
    }

    public override string GetPromptText()
    {
        return "Press {key} To pick up " +  item.itemName;
    }

    public override void InteractEffect()
    {
        if (hotbar.AddItem(item))
        {
            Destroy(gameObject);
        }
    }
}
