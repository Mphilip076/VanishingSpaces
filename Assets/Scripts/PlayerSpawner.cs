using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        PlayerMovement existingPlayer = FindAnyObjectByType<PlayerMovement>();
        Transform playerTransform = null;

        if (existingPlayer != null)
        {
            CharacterController cc = existingPlayer.GetComponent<CharacterController>();

            if (cc != null)
                cc.enabled = false;

            existingPlayer.transform.position = transform.position;
            existingPlayer.transform.rotation = transform.rotation;

            if (cc != null)
                cc.enabled = true;

            playerTransform = existingPlayer.transform;
        }
        else
        {
            GameObject newPlayer = Instantiate(playerPrefab, transform.position, transform.rotation);
            playerTransform = newPlayer.transform;
        }

        WeepingStatue statue = FindAnyObjectByType<WeepingStatue>();
        if (statue != null && playerTransform != null)
        {
            statue.player = playerTransform;
            statue.flashlight = playerTransform.GetComponentInChildren<Light>();
        }
    }
}