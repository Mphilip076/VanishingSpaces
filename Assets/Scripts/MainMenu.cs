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

    private bool isMoving = false;
    private bool isFadingMenu = false;
    private bool hasStarted = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.Hide();

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    void Update()
    {
        // Press Enter to start
        if (!hasStarted && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }

        // Fade out UI
        if (isFadingMenu)
        {
            menuGroup.alpha -= fadeSpeed * Time.deltaTime;

            if (menuGroup.alpha <= 0f)
            {
                menuGroup.alpha = 0f;
                isFadingMenu = false;
            }
        }

        // Move camera
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

            if (Vector3.Distance(cameraTransform.position, endPoint.position) < 0.05f)
            {
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

        // Disable UI interaction
        if (menuGroup != null)
        {
            menuGroup.interactable = false;
            menuGroup.blocksRaycasts = false;
            isFadingMenu = true;
        }

        // Delay before camera starts moving
        Invoke(nameof(BeginCameraMove), 0.5f);
    }

    void BeginCameraMove()
    {
        isMoving = true;
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}