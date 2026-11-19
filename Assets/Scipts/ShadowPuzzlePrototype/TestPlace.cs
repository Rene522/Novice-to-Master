using UnityEngine;
using static TESTObjectData;
using static ItemData;

public class TestPlace : Interactable
{
    [SerializeField] private Transform placementPoint;

    [SerializeField] private Light Light;

    //[SerializeField] private ShadowPuzzleManager shadowPuzzleManager;

    [SerializeField] private Hotbar hotbar;

    [SerializeField] private GameObject placedItem;
    [SerializeField] public Rigidbody rb;

    public bool itemPlaced = false;


    void Update()
    {
        if (itemPlaced && placedItem == null)
        {
            itemPlaced = false;
        }
    }
    public override string GetPlacePrompt()
    {
        ItemData item = hotbar.GetSelectedItem();

        if (item == null || item.placedObject == null)
            return "";

        return $"Press {{key}} to place {item.itemName}";
    }

    public override void PlaceEffect()
    {
        if (!itemPlaced)
        {
            ItemData item = hotbar.GetSelectedItem();

            itemPlaced = true;        

            if (item == null || item.placedObject == null)
                return;

            // shadowPuzzleManager.checkIfCorrect();

            Transform spawnPoint = placementPoint != null ? placementPoint : transform;

            GameObject placedObject = Instantiate(item.placedObject, spawnPoint.position, spawnPoint.rotation);

            placedObject.transform.SetParent(transform);

            Rigidbody rb = placedObject.GetComponentInChildren<Rigidbody>();

            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            placedItem = placedObject;
            Debug.Log(placedObject.name);

            //if (shadowPuzzleManager != null && shadowPuzzleManager.correct == true)
            //{
            // itemPlaced = true;
            // Debug.Log("YIPPEE");
            //}
            // else
            //{
            // Debug.Log("Nah");
            //}

            //TestPickup.RemoveCarriedItem();

            hotbar.RemoveSelectedItem();
        }
        else
        {
            Debug.Log("Item already there bro");
        }
    }

    public override string GetPromptText()
    {
        return "";
    }

}
