using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        // Ensure menu can use the mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Hide inventory on start screen
        if (InventoryManager.Instance != null)
        InventoryManager.Instance.Hide();
    }

    public void StartGame()
    {
        // Show inventory when game starts
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.Show();
            
        Room.SetScene("Tutorial");
    }
}