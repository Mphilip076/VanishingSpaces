using UnityEngine;

public class Lighter : PickableItem
{
    public override void Start()
    {
        base.Start();

        // If already picked up this session, destroy this scene copy
        if (PlayerPrefs.GetInt("lighter_picked_up", 0) == 1)
            Destroy(gameObject);
    }

    public override void OnPickup(Transform holdPosition)
    {
        PlayerPrefs.SetInt("lighter_picked_up", 1);
        base.OnPickup(holdPosition);
        // Lighter is now a child of the player (DontDestroyOnLoad) — persists without its own DontDestroyOnLoad
    }
}
