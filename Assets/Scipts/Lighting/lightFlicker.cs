using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class lightFlicker : MonoBehaviour
{
    private Light lantern;
    public float minIntensity = 5f;
    public float maxIntensity = 6f;
    public float flickerTimer = .1f;

    private float timer;

    void Start()
    {
        //Find the light component on the lantern
        if (lantern == null)
        {
            lantern = GetComponent<Light>();
        }

        //Call the flickering light function
        FlickeringLight();
    }

    // Update is called once per frame
    void Update()
    {
        //Prevent the light from flickering every single frame
        timer += Time.deltaTime;
        if (timer <= flickerTimer)
        {
            return;
        }

        //Randomly change the light intensity between the min and max values
        lantern.intensity = UnityEngine.Random.Range(minIntensity, maxIntensity);

        //reset the timer
        timer = 0;
    }

    private void FlickeringLight()
    {
        //Check that the min value is smaller than the max for the script to run
        if (minIntensity < maxIntensity)
        {
            return;
        }

        //Swap the values if the min value is larger than the max value
        (minIntensity, maxIntensity) = (maxIntensity, minIntensity);
    }
}
