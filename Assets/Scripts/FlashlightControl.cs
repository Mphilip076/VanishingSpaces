using UnityEngine;

public class FlashlightControl : MonoBehaviour
{
    public GameObject lightSource;
    private bool isOn = true;

    void Update()
    {
        // Press F to toggle the flashlight on and off
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            lightSource.SetActive(isOn);
        }
    }
}