using UnityEngine;
using TMPro;

public class FlashlightManager : MonoBehaviour
{
    public static FlashlightManager Instance;

    [Header("Battery")]
    public static float batteryLevel = 100f;
    public float drainRate = 0.5f;
    private bool isOn = false;

    [Header("Flashlight")]
    public Light flashlightLight;
    public static bool hasFlashlight = false; // Set this to true when player picks up flashlight

    [Header("UI")]
    public TextMeshProUGUI batteryText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (flashlightLight != null)
            flashlightLight.enabled = false;

        // Hide battery UI by default
        if (batteryText != null)
            batteryText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Only allow flashlight toggle if player has flashlight
        if (hasFlashlight && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        if (isOn)
        {
            batteryLevel -= drainRate * Time.deltaTime;
            batteryLevel = Mathf.Clamp(batteryLevel, 0f, 100f);

            if (batteryLevel <= 0f)
            {
                batteryLevel = 0f;
                TurnOff();
            }
        }

        UpdateUI();
    }

    public void TurnOn()
    {
        if (batteryLevel > 0f)
        {
            isOn = true;
            if (flashlightLight != null)
                flashlightLight.enabled = true;
        }
    }

    public void TurnOff()
    {
        isOn = false;
        if (flashlightLight != null)
            flashlightLight.enabled = false;
    }

    public void ToggleFlashlight()
    {
        if (isOn) TurnOff();
        else TurnOn();
    }

    // Call this when player picks up flashlight
    public static void OnFlashlightPickup()
    {
        hasFlashlight = true;

        // Show battery UI
        if (Instance != null && Instance.batteryText != null)
            Instance.batteryText.gameObject.SetActive(true);
    }

    private void UpdateUI()
    {
        if (batteryText != null)
            batteryText.text = "Battery: " + Mathf.RoundToInt(batteryLevel) + "%";
    }
}