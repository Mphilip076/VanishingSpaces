using UnityEngine;

public class EndSceneSetup : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Destroy(player);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameObject inventory = GameObject.Find("InventoryCanvas");

        if (inventory != null)
        {
            inventory.SetActive(false);
        }
    }
}