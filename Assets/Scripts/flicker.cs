using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    public Light lightSource;

    [Header("Flash Brightness")]
    public float minFlashIntensity = 0.8f;
    public float maxFlashIntensity = 1.2f;

    [Header("Time While Off")]
    public float minOffTime = 8f;
    public float maxOffTime = 20f;

    [Header("Time While On")]
    public float minFlashTime = 0.08f;
    public float maxFlashTime = 0.25f;

    [Header("How Often This Light Actually Flashes")]
    [Range(0f, 1f)] public float flashChance = 0.25f;

    [Header("Burst Size")]
    public int minFlashCount = 1;
    public int maxFlashCount = 3;

    void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light>();

        lightSource.intensity = 0f;

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        // random startup delay so lights do not sync
        yield return new WaitForSeconds(Random.Range(0f, 5f));

        while (true)
        {
            lightSource.intensity = 0f;

            // wait in darkness for a random amount of time
            yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));

            // sometimes do nothing this cycle
            if (Random.value > flashChance)
                continue;

            int flashCount = Random.Range(minFlashCount, maxFlashCount + 1);

            for (int i = 0; i < flashCount; i++)
            {
                lightSource.intensity = Random.Range(minFlashIntensity, maxFlashIntensity);
                yield return new WaitForSeconds(Random.Range(minFlashTime, maxFlashTime));

                lightSource.intensity = 0f;
                yield return new WaitForSeconds(Random.Range(0.03f, 0.12f));
            }
        }
    }
}