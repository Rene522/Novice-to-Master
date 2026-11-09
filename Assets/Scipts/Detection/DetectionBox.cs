using System.Collections.Generic;
using UnityEngine;

public class DetectionBox : MonoBehaviour
{
    //The target objects
    [SerializeField] List<GameObject> targets = new List<GameObject>();
    
    //The colliders of objects inside
    private List<Collider> objectsInside = new List<Collider>();

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

    public bool isTargetInside(GameObject target)
    {
        foreach (Collider col in objectsInside)
        {
            if (col != null && col.gameObject == target)
            {
                Debug.Log(target);
                return true;
            }
        }
        
        return false;
    }
    
    
    
}