using UnityEngine;
using System.Collections;

public class FlashlightFlicker : MonoBehaviour
{
    public Light flashlight;

    [Header("Timing")]
    public float minTimeBetweenFlickers = 5f;
    public float maxTimeBetweenFlickers = 15f;

    [Header("Flicker Settings")]
    public int minFlickers = 2;
    public int maxFlickers = 6;
    public float flickerDuration = 0.05f;

    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light>();

        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minTimeBetweenFlickers, maxTimeBetweenFlickers);
            yield return new WaitForSeconds(waitTime);

            // Only flicker if flashlight is currently ON!
            if (!flashlight.enabled) continue;

            int flickerCount = Random.Range(minFlickers, maxFlickers);

            for (int i = 0; i < flickerCount; i++)
            {
                // Double check each flicker in case player turned it off mid-flicker
                if (!flashlight.enabled) break;

                flashlight.enabled = false;
                yield return new WaitForSeconds(flickerDuration);

                flashlight.enabled = true;
                yield return new WaitForSeconds(flickerDuration);
            }
        }
    }
}