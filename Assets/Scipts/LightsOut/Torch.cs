using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool isOn;

    public GameObject lightSource;


    public void Toggle()
    {
        isOn = !isOn;
        lightSource.SetActive(isOn);
    }
}
