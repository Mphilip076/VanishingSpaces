using System.Collections.Generic;
using UnityEngine;

public class LionPickup : PickableItem
{
    public string lionID;

    private static List<string> existing = new List<string>();

    public override void Start()
    {
        if(existing.Exists(x => lionID == x))
        {
            // This item already exists in the inventory or on a statue
            Destroy(gameObject);
            return;
        }

        interactMessage = "Press E to pick up lion statue";
        base.Start();
    }

    public override void OnPickup(Transform holdPosition)
    {
        base.OnPickup(holdPosition);

        existing.Add(lionID);
        Destroy(gameObject);
    }

    public static void ResetAll()
    {
        existing.Clear();
    }
}