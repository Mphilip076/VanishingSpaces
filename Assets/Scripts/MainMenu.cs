using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject tutorialPanel;

    public Transform cameraTransform;
    public Transform endPoint;
    public float moveSpeed = 3f;
    public float rotateSpeed = 2f;

    public CanvasGroup menuGroup;
    public float fadeSpeed = 2f;

    public CanvasGroup fadeGroup;
    public float screenFadeSpeed = 1.5f;
    public float fadeToBlackDistance = 1.5f;

    public DoorOpen leftDoor;
    public DoorOpen rightDoor;
    public float doorOpenDistance = 2f;

    public AudioSource buttonAudio; // ADDED

    private bool isMoving = false;
    private bool isFadingMenu = false;
    private bool isFadingToBlack = false;
    private bool hasStarted = false;
    private bool doorsOpened = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.Hide();

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (menuGroup != null)
            menuGroup.alpha = 1f;

        if (fadeGroup != null)
            fadeGroup.alpha = 0f;
    }

    void Update()
    {
        if (!hasStarted && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }

        if (isFadingMenu && menuGroup != null)
        {
            menuGroup.alpha -= fadeSpeed * Time.deltaTime;

            if (menuGroup.alpha <= 0f)
            {
                menuGroup.alpha = 0f;
                isFadingMenu = false;
            }
        }

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
                isMoving = false;

                if (InventoryManager.Instance != null)
                    InventoryManager.Instance.Show();

                Room.SetScene("Tutorial");
            }
        }

        if (isFadingToBlack && fadeGroup != null)
        {
            fadeGroup.alpha += screenFadeSpeed * Time.deltaTime;

            if (fadeGroup.alpha >= 1f)
            {
                fadeGroup.alpha = 1f;
                isFadingToBlack = false;
                isMoving = false;

                if (InventoryManager.Instance != null)
                    InventoryManager.Instance.Show();

                Room.SetScene("Tutorial");
            }
        }
    }

    public void StartGame()
    {
        if (hasStarted) return;

        hasStarted = true;

        // PLAY BUTTON SOUND (ADDED)
        if (buttonAudio != null)
            buttonAudio.PlayOneShot(buttonAudio.clip);

        if (menuGroup != null)
        {
            menuGroup.interactable = false;
            menuGroup.blocksRaycasts = false;
            isFadingMenu = true;
        }

        Invoke(nameof(BeginCameraMove), 0.5f);
    }

    void BeginCameraMove()
    {
        isMoving = true;
    }

    public void OpenTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }
}