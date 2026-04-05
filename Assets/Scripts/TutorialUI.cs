using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialPanel;

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.Tab;
    public string tutorialSceneName = "Tutorial";

    private bool isOpen = false;

    void Start()
    {
        isOpen = false;
        tutorialPanel.SetActive(false);

        // Only auto open in Tutorial scene!
        if (SceneManager.GetActiveScene().name == tutorialSceneName)
        {
            isOpen = true;
            tutorialPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
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
}