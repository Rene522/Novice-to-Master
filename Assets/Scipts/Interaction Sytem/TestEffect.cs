using UnityEngine;

public class TestEffect : Interactable
{
    //public GameObject Spawnable;
    //public Transform spawnLocation;
    public override void InteractEffect()
    {
        Debug.Log("You Have Interacted With The Object");
        //Instantiate(Spawnable, spawnLocation.position, Quaternion.identity);
    }
}
