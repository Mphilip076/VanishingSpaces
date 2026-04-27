using System.Collections.Generic;
using UnityEngine;

public class ToiletRoll : InteractableItem
{
    public static int rollsMoved = 0;
    public string id;
    private bool moved = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey(id))
        {
            Destroy(gameObject);
            return;
        }

        interactMessage = "Press E to move toilet roll";
        interactKey = KeyCode.E;
        interactRange = 2f;
    }

    public override void OnInteract()
    {
        if(moved == true) return;

        moved = true;
        rollsMoved++;
        PlayerPrefs.SetInt(id, 1);
        Invoke("FinishInteract", 1);
        ShowShortMessage($"Rolls found: {rollsMoved}/5", 1);
    }

    private void FinishInteract()
    {
        Destroy(gameObject);
        RollTower.trigger = true;
    }


}
