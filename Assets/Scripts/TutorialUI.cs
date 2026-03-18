using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialPanel;  // Assign TutorialPanel
    public GameObject overlay;        // Assign dark Overlay image

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.Tab;

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            TogglePanel();
    }

    public void TogglePanel()
    {
        isOpen = !isOpen;

        tutorialPanel.SetActive(isOpen);
        overlay.SetActive(isOpen);

        // Unlock cursor when panel is open
        Cursor.lockState = isOpen ? 
            CursorLockMode.None : 
            CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }

    // Assign to ? button AND close X button OnClick
    public void OnHelpButtonClick()
    {
        TogglePanel();
    }

    // Close if player clicks the overlay behind panel
    public void OnOverlayClick()
    {
        if (isOpen) TogglePanel();
    }
}