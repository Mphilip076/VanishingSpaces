using System.Collections.Generic;
using UnityEngine;

public class ToiletRoll : InteractableItem
{
    public static int rollsMoved = 0;
    private static int id = 1;
    private bool moved = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(id > 5)
        {
            Destroy(gameObject);
            return;
        }

        id++;

        DontDestroyOnLoad(gameObject);

        interactMessage = "Press E to move toilet roll";
        interactKey = KeyCode.E;
        interactRange = 2f;
    }

    public override void OnInteract()
    {
        if(moved == true) return;

        moved = true;
        rollsMoved++;
        Invoke("FinishInteract", 1);
        ShowShortMessage($"Rolls found: {rollsMoved}/5", 1);
    }

    private void FinishInteract()
    {
        Destroy(gameObject);
        RollTower.trigger = true;
    }


}
