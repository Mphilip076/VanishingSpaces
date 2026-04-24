using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    private static PauseMenu instance;

    [Header("Panels")]
    public GameObject pausePanel;    // Assign in Inspector!
    public GameObject settingsPanel; // Assign in Inspector!

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            EnsureEventSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void EnsureEventSystem()
    {
        if (FindAnyObjectByType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
            DontDestroyOnLoad(es);
        }
        else
        {
            DontDestroyOnLoad(FindAnyObjectByType<EventSystem>().gameObject);
        }
    }

    void Start()
    {
        // Add these lines
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        EnsureEventSystem();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "StartScene") return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
                CloseSettings();
            else if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        if (pausePanel == null) return;

        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EventSystem.current?.SetSelectedGameObject(null);

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = false;
    }

    public void Resume()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = true;
    }

    public void OpenSettings()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
        EventSystem.current?.SetSelectedGameObject(null);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(true);
        EventSystem.current?.SetSelectedGameObject(null);
    }

    public void BackToMainMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerPrefs.DeleteAll();

        // Destroy every DontDestroyOnLoad object (player, inventory, canvas, etc.)
        foreach (GameObject go in gameObject.scene.GetRootGameObjects())
            Destroy(go);

        instance = null;

        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }
}