using UnityEngine;

public class EndAnimationManager : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform endPoint;
    public float moveSpeed = 3f;
    public float rotateSpeed = 2f;

    public CanvasGroup fadeGroup;
    public float screenFadeSpeed = 1.5f;
    public float fadeToBlackDistance = 1.5f;

    public DoorOpen leftDoor;
    public DoorOpen rightDoor;
    public float doorOpenDistance = 2f;

    public AudioSource runningAudio;

    private bool isMoving = true;
    private bool isFadingToBlack = false;
    private bool doorsOpened = false;
    private bool endingFinished = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.Hide();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (fadeGroup != null)
            fadeGroup.alpha = 0f;

        if (runningAudio != null)
            runningAudio.Play();
    }

    void Update()
    {
        if (endingFinished)
            return;

        if (isMoving)
        {
            cameraTransform.position = Vector3.MoveTowards(
                cameraTransform.position,
                endPoint.position,
                moveSpeed * Time.deltaTime
            );

            cameraTransform.rotation = Quaternion.Slerp(
                cameraTransform.rotation,
                endPoint.rotation,
                rotateSpeed * Time.deltaTime
            );

            float distance = Vector3.Distance(cameraTransform.position, endPoint.position);

            if (!doorsOpened && distance <= doorOpenDistance)
            {
                doorsOpened = true;

                if (leftDoor != null)
                    leftDoor.OpenDoor();

                if (rightDoor != null)
                    rightDoor.OpenDoor();
            }

            if (!isFadingToBlack && fadeGroup != null && distance <= fadeToBlackDistance)
            {
                isFadingToBlack = true;
            }

            if (distance < 0.05f && fadeGroup == null)
            {
                FinishEnding();
            }
        }

        if (isFadingToBlack && fadeGroup != null)
        {
            fadeGroup.alpha += screenFadeSpeed * Time.deltaTime;

            if (fadeGroup.alpha >= 1f)
            {
                fadeGroup.alpha = 1f;
                FinishEnding();
            }
        }
    }

    void FinishEnding()
    {
        endingFinished = true;
        isMoving = false;
        isFadingToBlack = false;

        if (runningAudio != null)
            runningAudio.Stop();

        // Leave the screen black here.
        // If you want to go to credits or menu later, add it here.
    }
}