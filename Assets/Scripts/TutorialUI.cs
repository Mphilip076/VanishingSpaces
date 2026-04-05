using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialPanel;

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.Tab;

    private bool isOpen = false;

    void Start()
    {
        isOpen = true;
        tutorialPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            TogglePanel();
    }

    void TogglePanel()
    {
        isOpen = !isOpen;
        tutorialPanel.SetActive(isOpen);

        Cursor.lockState = isOpen ?
            CursorLockMode.None :
            CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }

    public void OnHelpButtonClick()
    {
        TogglePanel();
    }
}