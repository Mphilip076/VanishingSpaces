using NUnit.Framework.Internal;
using UnityEngine;

public class Battery : PickableItem
{    
    [Header("Battery Info")]
    public static int maxCharge = 100; // Maximum amount batteries can hold
    public static int minCharge = 0; // Minimum amount batteries can hold
    private int currentCharge; // Current charge level of this battery


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCharge = Random.Range(minCharge, maxCharge); 
            // Initialize battery with a random charge level between min and max
    }

    public void OnUse()
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
