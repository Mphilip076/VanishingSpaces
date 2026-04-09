using NUnit.Framework.Internal;
using UnityEngine;

public class Battery : PickableItem
{    
    [Header("Battery Info")]
    public int maxCharge = 100; // Maximum amount the battery can hold
    public int minCharge = 0; // Minimum amount the battery can hold
    private int currentCharge; // Current charge level of the battery


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCharge = Random.Range(minCharge, maxCharge); // Initialize battery with a random charge level between min and max
    }

    public void UseBattery()
    {
        FlashlightControl.batteryLevel += currentCharge; // Increase the flashlight's battery level by the current charge of this battery
        if (FlashlightControl.batteryLevel > 100)
        {
            FlashlightControl.batteryLevel = 100; // Cap the flashlight's battery level at 100
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
