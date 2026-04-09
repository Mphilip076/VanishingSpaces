using UnityEngine;

public class FlashlightControl : MonoBehaviour
{
    public GameObject lightSource;
    private bool isOn = true;
    public static int batteryLevel = 100; // Battery level from 0 to 100
    public int drainTime = 2; // Time in seconds for the battery to drain by 1 unit

    void Update()
    {
        // Press F to toggle the flashlight on and off
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            lightSource.SetActive(isOn);
        }

        // If the flashlight is on, decrease battery level over time
        // Decreases by 1 every second, so it lasts 
        //      200 seconds or about 3 minutes and 20 seconds at full charge
        if (isOn && Time.deltaTime > drainTime)
        {
            batteryLevel--;

            if(batteryLevel <= 0)
            {
                batteryLevel = 0;
                isOn = false;
                lightSource.SetActive(false);
                Debug.Log("Flashlight battery is dead");
            }
        }
    }
}