using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    public Light lightSource;

    public float flashIntensity = 1.5f;

    public float minOffTime = 1f;
    public float maxOffTime = 4f;

    public float minFlashTime = 0.05f;
    public float maxFlashTime = 0.2f;

    void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light>();

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // Stay OFF (dark most of the time)
            lightSource.intensity = 0f;
            yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));

            // Flash burst (quick flickers ON)
            int flashCount = Random.Range(1, 4);

            for (int i = 0; i < flashCount; i++)
            {
                lightSource.intensity = Random.Range(0.8f, flashIntensity);
                yield return new WaitForSeconds(Random.Range(minFlashTime, maxFlashTime));

                lightSource.intensity = 0f;
                yield return new WaitForSeconds(Random.Range(0.02f, 0.08f));
            }
        }
    }
}