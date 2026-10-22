using UnityEngine;
using static TESTObjectData;

public class TestPickup : Interactable
{
    [SerializeField] private ItemData itemData;

    public static ItemData CarriedItem { get; private set; }

    public bool HasItem => CarriedItem != null;

    [SerializeField] private Light itemLight;

    public void Awake()
    {
        if (itemLight == null)
            itemLight = GetComponentInChildren<Light>();

        if (itemLight != null)
            itemLight.enabled = false;
    }

    public override string GetPromptText()
    {
        if (CarriedItem != null || itemData == null)
            return "";

        return $"Press {{key}} to pick up {itemData.itemName}";
    }

    public override void InteractEffect()
    {
        if (CarriedItem != null || itemData == null)
            return;

        CarriedItem = itemData;

        Debug.Log("Picked up: " + CarriedItem.itemName);

        Destroy(gameObject);
    }

    public static void RemoveCarriedItem()
    {
        CarriedItem = null;
    }

    public void SetItemData(ItemData item)
    {
        itemData = item;
    }

    public void SetLanternLight(bool isOn)
    {
        if (itemLight == null)
            itemLight = GetComponentInChildren<Light>(true);

        if (itemLight != null)
        {
            itemLight.enabled = isOn;
            itemLight.gameObject.SetActive(true);
        }

    }
}
