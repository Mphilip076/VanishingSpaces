using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject tutorialPanel;

    void Start()
    {
        // Ensure menu can use the mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Hide inventory on start screen
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.Hide();

        // Make sure tutorial is hidden at start
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    public void StartGame()
    {
        // Show inventory when game starts
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.Show();

        Room.SetScene("Tutorial");
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}