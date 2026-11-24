using UnityEngine;
using System.Runtime.CompilerServices;
using System;
using System.Collections;

public class LanternFlicker : MonoBehaviour
{
    private Light lantern;

    public float minIntensity = 10f;
    public float maxIntensity = 25f;
    public float flickerTimer = .1f;

    public float totalTime = 2f;

    private float timer;

    private Coroutine flickerCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindLanternLight();
    }

    public void FindLanternLight()
    {
        lantern = GetComponentInChildren<Light>(true);

    }

    public void CorrectAnswer()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }

        lantern.enabled = true;
        lantern.intensity = maxIntensity;
    }

    public void IncorrectAnswer()
    {
        if (flickerCoroutine != null)
            StopCoroutine(flickerCoroutine);

        flickerCoroutine = StartCoroutine(FlickeringLight(minIntensity, maxIntensity));
    }

    public IEnumerator FlickeringLight(float minIntensity, float maxIntensity)
    {

        lantern.enabled = true;

        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            float randomIntensity = UnityEngine.Random.Range(minIntensity, maxIntensity);

            lantern.intensity = randomIntensity;

            Debug.Log("Intensity: " + randomIntensity);

            yield return new WaitForSeconds(flickerTimer);

            elapsedTime += flickerTimer;
        }

        lantern.enabled = false;

        flickerCoroutine = null;

    }

}
