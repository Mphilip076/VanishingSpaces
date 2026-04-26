using UnityEngine;
using TMPro;

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

    [Header("End Text")]
    public TMP_Text storyEndText;
    [TextArea(3, 10)]
    public string storyEndMessage = "You made it outside.\nBut the house is still watching.";

    public TMP_Text thankYouText;
    public string thankYouMessage = "Thank you for playing.";

    public float storyTextDelay = 1f;
    public float thankYouDelay = 4f;

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

        if (storyEndText != null)
        {
            storyEndText.text = "";
            storyEndText.gameObject.SetActive(false);
        }

        if (thankYouText != null)
        {
            thankYouText.text = "";
            thankYouText.gameObject.SetActive(false);
        }

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
        if (endingFinished)
            return;

        endingFinished = true;
        isMoving = false;
        isFadingToBlack = false;

        if (runningAudio != null)
            runningAudio.Stop();

        Invoke(nameof(ShowStoryEndText), storyTextDelay);
        Invoke(nameof(ShowThankYouText), storyTextDelay + thankYouDelay);
    }

    void ShowStoryEndText()
    {
        if (storyEndText != null)
        {
            storyEndText.gameObject.SetActive(true);
            storyEndText.text = storyEndMessage;
        }
    }

    void ShowThankYouText()
    {
        if (thankYouText != null)
        {
            thankYouText.gameObject.SetActive(true);
            thankYouText.text = thankYouMessage;
        }
    }
}