using UnityEngine;
using static TESTObjectData;

public class TestPlace : Interactable
{
    [SerializeField] private TESTObjectData acceptedItem;
    [SerializeField] private Transform placementPoint;

    [SerializeField] private Light Light;


    public bool itemPlaced = false;

    public override string GetPromptText()
    {
        TESTObjectData carriedItem = TestPickup.CarriedItem;

        if (carriedItem == null || acceptedItem == null)
            return "";

        if (carriedItem != acceptedItem)
            return "";

        return $"Press {{key}} to place {carriedItem.itemName}";
    }

    public override void InteractEffect()
    {
        TESTObjectData carriedItem = TestPickup.CarriedItem;

        if (carriedItem == null || acceptedItem == null)
            return;

        if (carriedItem != acceptedItem)
            return;

        if (carriedItem.placedPrefab == null)
        {
            return;
        }

        Transform spawnPoint = placementPoint != null ? placementPoint : transform;

        GameObject placedObject = Instantiate(carriedItem.placedPrefab, spawnPoint.position,spawnPoint.rotation);

        TestPickup placedPickup = placedObject.GetComponent<TestPickup>();

        if (placedPickup != null)
        {
            placedPickup.SetItemData(carriedItem);
            placedPickup.SetLanternLight(true);

            itemPlaced = true;
            Debug.Log("YIPPEE");
        }

        TestPickup.RemoveCarriedItem();
    }
}
