using UnityEngine;
using static TESTObjectData;

public class TestPlace : Interactable
{
    [SerializeField] private TESTObjectData acceptedItem;
    [SerializeField] private Transform placementPoint;

    [SerializeField] private Light Light;

    [SerializeField] private ShadowPuzzleManager shadowPuzzleManager;


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
            return;

        shadowPuzzleManager.checkIfCorrect();

        Transform spawnPoint = placementPoint != null ? placementPoint : transform;

        GameObject placedObject = Instantiate(carriedItem.placedPrefab, spawnPoint.position, spawnPoint.rotation);

        TestPickup placedPickup = placedObject.GetComponent<TestPickup>();

        if (placedPickup == null)
        {
            return;
        }

        placedPickup.SetItemData(carriedItem);

        if (shadowPuzzleManager != null && shadowPuzzleManager.correct == true)
        {
            placedPickup.SetLanternLight(true);
            itemPlaced = true;
            Debug.Log("YIPPEE");
        }
        else
        {
            Debug.Log("Nah");
            placedPickup.SetLanternLight(false);
        }

        TestPickup.RemoveCarriedItem();
    }
}
