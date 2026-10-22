using UnityEngine;
using static TESTObjectData;

public class TestPickup : Interactable
{
    [SerializeField] private TESTObjectData itemData;

    public static TESTObjectData CarriedItem { get; private set; }

    public bool HasItem => CarriedItem != null;

    [SerializeField] private Light Light;

    public void Awake()
    {
        if (Light == null)
            Light = GetComponentInChildren<Light>();

        if (Light != null)
            Light.enabled = false;
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

    public void SetItemData(TESTObjectData item)
    {
        itemData = item;
    }

    public void SetLanternLight(bool isOn)
    {
        if (Light == null)
            Light = GetComponentInChildren<Light>(true);

        if (Light != null)
        {
            Light.enabled = isOn;
            Light.gameObject.SetActive(true);
        }

    }
}
