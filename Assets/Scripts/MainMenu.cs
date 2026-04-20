using UnityEngine;
using TMPro;
using System.Collections;

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

    public AudioSource buttonAudio;

    public TMP_Text journalText;
    [TextArea(3, 10)]
    public string journalMessage;
    public float typeSpeed = 0.05f;
    public float journalDelayBeforeLoad = 1.5f;
    public AudioSource typewriterAudio;

    [Header("Skip Hint")]
    public TMP_Text skipHintText;

    private bool isMoving = false;
    private bool isFadingMenu = false;
    private bool isFadingToBlack = false;
    private bool hasStarted = false;
    private bool doorsOpened = false;
    private bool journalStarted = false;

    public GameObject creditsPanel;

    public void OpenCredits()
    {
        if (fadeGroup != null)
            fadeGroup.alpha = 1f;

        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);

        // Remove black screen
        if (fadeGroup != null)
            fadeGroup.alpha = 0f;
    }

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

        if (journalText != null)
        {
            journalText.text = "";
            journalText.gameObject.SetActive(false);
        }

        if (skipHintText != null)
            skipHintText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!hasStarted && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }

        if (isMoving && Input.GetKeyDown(KeyCode.Escape))
        {
            SkipIntro();
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
                StartJournalSequence();
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

                StartJournalSequence();
            }
        }
    }

    public void StartGame()
    {
        if (hasStarted) return;

        hasStarted = true;

        if (buttonAudio != null && buttonAudio.clip != null)
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

        if (skipHintText != null)
        {
            skipHintText.gameObject.SetActive(true);
            skipHintText.text = "Press ESC to Skip";
        }
    }

    void SkipIntro()
    {
        isMoving = false;
        isFadingMenu = false;
        isFadingToBlack = false;

        if (!doorsOpened)
        {
            doorsOpened = true;
            if (leftDoor != null) leftDoor.OpenDoor(false);
            if (rightDoor != null) rightDoor.OpenDoor(false);
        }

        if (fadeGroup != null)
            fadeGroup.alpha = 1f;

        if (menuGroup != null)
        {
            menuGroup.alpha = 0f;
            menuGroup.interactable = false;
            menuGroup.blocksRaycasts = false;
        }

        StartJournalSequence();
    }

    void StartJournalSequence()
    {
        if (journalStarted) return;
        journalStarted = true;

        StartCoroutine(TypeJournalAndLoad());
    }

    IEnumerator TypeJournalAndLoad()
    {
        if (skipHintText != null)
        {
            skipHintText.gameObject.SetActive(true);
            skipHintText.text = "Hold Q to Speed Up";
        }

        if (journalText != null)
        {
            journalText.gameObject.SetActive(true);
            journalText.text = "";

            foreach (char letter in journalMessage)
            {
                journalText.text += letter;

                if (typewriterAudio != null && typewriterAudio.clip != null && !char.IsWhiteSpace(letter))
                {
                    typewriterAudio.PlayOneShot(typewriterAudio.clip);
                }

                if (Input.GetKey(KeyCode.Q))
                    yield return new WaitForSeconds(typeSpeed * 0.1f);
                else
                    yield return new WaitForSeconds(typeSpeed);
            }
        }

        if (skipHintText != null)
            skipHintText.gameObject.SetActive(false);

        yield return new WaitForSeconds(journalDelayBeforeLoad);

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.Show();

        Room.SetScene("Tutorial");
    }

    public void OpenTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(!tutorialPanel.activeSelf);
    }
}