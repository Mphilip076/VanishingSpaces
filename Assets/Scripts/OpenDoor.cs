using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Vector3 openRotation;
    public float openSpeed = 2f;

    private Quaternion closedRotation;
    private Quaternion targetRotation;
    private bool isOpening = false;

    void Start()
    {
        closedRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.eulerAngles + openRotation);
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

    public void OpenDoor()
    {
        isOpening = true;
    }
}