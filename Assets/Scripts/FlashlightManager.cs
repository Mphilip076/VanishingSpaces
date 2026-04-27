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
        // Load saved state — defaults to 100%/no flashlight on fresh game (PlayerPrefs cleared by BackToMainMenu)
        batteryLevel = PlayerPrefs.GetFloat("fl_battery", 100f);
        hasFlashlight = PlayerPrefs.GetInt("fl_hasFlashlight", 0) == 1;
    }

    void Start()
    {
        if (flashlightLight != null)
            flashlightLight.enabled = false;

        if (batteryText != null)
            batteryText.gameObject.SetActive(hasFlashlight);
    }

    void Update()
    {
        if (hasFlashlight && Input.GetKeyDown(KeyCode.F))
            ToggleFlashlight();

        if (isOn)
        {
            batteryLevel -= drainRate * Time.deltaTime;
            batteryLevel = Mathf.Clamp(batteryLevel, 0f, 100f);
            PlayerPrefs.SetFloat("fl_battery", batteryLevel);

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

    public static void OnFlashlightPickup()
    {
        hasFlashlight = true;
        PlayerPrefs.SetInt("fl_hasFlashlight", 1);

        if (Instance != null && Instance.batteryText != null)
            Instance.batteryText.gameObject.SetActive(true);
    }

    private void UpdateUI()
    {
        if (batteryText != null)
            batteryText.text = "Battery: " + Mathf.RoundToInt(batteryLevel) + "%";
    }
}