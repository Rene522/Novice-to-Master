using UnityEngine;
using System.Runtime.CompilerServices;
using System;
using System.Collections;

public class LanternFlicker : MonoBehaviour
{
    private Light lantern;

    public float minIntensity = 10f;
    public float maxIntesity = 25f;
    public float flickerTimer = .1f;

    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (lantern == null)
        {
            lantern = GetComponent<Light>();
        }

        //StartCoroutine(FlickeringLight());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer <= flickerTimer) return;
        lantern.intensity = UnityEngine.Random.Range(minIntensity, maxIntesity);
        timer = 0;
    }

    private IEnumerator FlickeringLight(float minIntensity, float maxIntesity)
    {
        if (minIntensity < maxIntesity)
        {
            yield return null;
        }
        (minIntensity, maxIntesity) = (maxIntesity, minIntensity);

        yield return new WaitForSeconds(2);

        minIntensity = 0f;
        maxIntesity = 0f;

    }

}
