using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Vector3 openRotation;
    public float openSpeed = 2f;

    [Header("Sounds")]
    public AudioClip doorOpenSound;
    private AudioSource audioSource;

    private Quaternion closedRotation;
    private Quaternion targetRotation;
    private bool isOpening = false;

    void Start()
    {
        closedRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.eulerAngles + openRotation);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isOpening)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                openSpeed * Time.deltaTime
            );
        }
    }

    public void OpenDoor(bool playSound = true)
    {
        if (!isOpening)
        {
            isOpening = true;

            if (playSound && audioSource != null && doorOpenSound != null)
                audioSource.PlayOneShot(doorOpenSound);
        }
    }
}