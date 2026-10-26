using UnityEngine;

public class RockPickup : Interactable
{
    public Transform HoldingArea;
    private bool isHoldingItem;
    
    public override void InteractEffect()
    {
        if (!isHoldingItem) 
        {
            PickUp();
        }
        if (isHoldingItem) 
        {
            Drop();
        }
       

    }

    public void PickUp()
    {
        gameObject.transform.position = HoldingArea.position;
        gameObject.transform.SetParent(HoldingArea);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Drop()
    {
        gameObject.transform.SetParent (null);
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
