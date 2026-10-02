using UnityEngine;

public class Interactable : MonoBehaviour
{
    //this is just a base for any interactable object 
    public KeyCode interactKey = KeyCode.E;
    public virtual void InteractEffect()
    {
        Debug.Log("default interaction triggered");
    }
}
