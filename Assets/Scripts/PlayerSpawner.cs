using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        PlayerMovement existingPlayer = FindAnyObjectByType<PlayerMovement>();

        if (existingPlayer != null)
        {
            CharacterController cc = existingPlayer.GetComponent<CharacterController>();
            cc.enabled = false;
            existingPlayer.transform.position = transform.position;
            existingPlayer.transform.rotation = transform.rotation;
            cc.enabled = true;
        }
        else
        {
            Instantiate(playerPrefab, transform.position, transform.rotation);
        }
    }
}