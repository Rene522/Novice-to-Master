using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectionBox : MonoBehaviour
{
    //The target objects
    [SerializeField] List<GameObject> targets = new List<GameObject>();
    [SerializeField] List<ItemData> items = new List<ItemData>();
    
    //The colliders of objects inside
    private List<Collider> objectsInside = new List<Collider>();


    private void Update()
    {
        //Removes items from the list that arent meant to be there
        //Goes backwards to avoid skipping an item in the list
        for (int i = objectsInside.Count - 1; i >= 0; i--)
        {
            if (objectsInside[i] == null)
            {
                objectsInside.RemoveAt(i);
            }
        }
        
        if (targets.Count > 0)
        {
            Debug.Log("Target " + IsTargetInside(targets[0]));
        }
        
        if (items.Count > 0)
        {
            Debug.Log("Item " + IsItemInside(items[0]));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!objectsInside.Contains(other))
        {
            objectsInside.Add(other);
            Debug.Log(other.gameObject.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        objectsInside.Remove(other);
    }
    
    
    //This is for when its called from other scripts so a puzzle can have a specific requirement
    public bool IsTargetInside(GameObject target)
    {
        foreach (Collider col in objectsInside)
        {
            if (col != null && col.transform.IsChildOf(target.transform))
            {
                return true;
            }
        }
        
        return false;
    }

    //This is for when an item has be dropped or placed inside
    public bool IsItemInside(ItemData item)
    {
        foreach (Collider col in objectsInside)
        {
            if (col == null) continue;
            PickupItem pickup = col.GetComponentInParent<PickupItem>();

            if (pickup != null && pickup.item == item)
            {
                return true;
            }
        }
        
        return false;
    }

    public bool AnyTargetInside()
    {
        foreach (GameObject obj in targets)
        {
            if (obj != null && IsTargetInside(obj))
            {
                return true;
            }
        }
        
        foreach (ItemData item in items)
        {
            if (item != null && IsItemInside(item))
            {
                return true;
            }
        }
        
        return false;
    }
    
}