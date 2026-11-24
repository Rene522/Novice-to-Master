using UnityEngine;
using static TESTObjectData;
using static ItemData;

public class TestPlace : Interactable
{
    [SerializeField] private Transform placementPoint;

    [SerializeField] private Light Light;

    [SerializeField] private ShadowPuzzleManager shadowPuzzleManager;

    [SerializeField] private Hotbar hotbar;

    [SerializeField] private LanternFlicker lanternFlicker;

    [SerializeField] public GameObject placedItem;

    public GameObject placedItemPrefab;
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

            Transform spawnPoint = placementPoint != null ? placementPoint : transform;

            placedItemPrefab = item.placedObject;

            GameObject placedObject = Instantiate(item.placedObject, spawnPoint.position, spawnPoint.rotation);

            placedObject.transform.SetParent(transform);

            lanternFlicker = placedObject.GetComponentInChildren<LanternFlicker>();

            if (lanternFlicker != null)
            {
                lanternFlicker.FindLanternLight();
            }

            Rigidbody rb = placedObject.GetComponentInChildren<Rigidbody>();

            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            placedItem = placedObject;
            Debug.Log(placedObject.name);

            if (lanternFlicker != null)
            {
                if (shadowPuzzleManager.correct)
                {
                    lanternFlicker.CorrectAnswer();
                    Debug.Log("YARRRR");
                }
                else
                {
                    lanternFlicker.IncorrectAnswer();
                    Debug.Log("NAH");
                }
            }

            hotbar.RemoveSelectedItem();

            if(shadowPuzzleManager != null)
            {
                shadowPuzzleManager.checkIfCorrect();
            }
            else
            {
                return;
            }
            
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
