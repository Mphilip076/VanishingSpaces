using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f;

    [Header("Mouse Look")]
    public Transform cameraTransform;

    [Header("Footstep Sounds")]
    public AudioClip[] walkClips;
    public AudioClip[] runClips;
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.3f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private Animator animator;
    private AudioSource footstepSource;
    private float footstepTimer = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        footstepSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;

        xRotation = 0f;
        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        MouseLook();
        Move();
    }

    void MouseLook()
    {
        float sensitivity = SettingsManager.mouseSensitivity;
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && (x != 0 || z != 0);
        bool isMoving = (x != 0 || z != 0) && controller.isGrounded;
        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        float currentSpeed = new Vector3(x, 0, z).magnitude;
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("IsRunning", isRunning);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Footstep logic
        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayFootstep(isRunning);
                footstepTimer = isRunning ? runStepInterval : walkStepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    void PlayFootstep(bool isRunning)
    {
        AudioClip[] clips = isRunning ? runClips : walkClips;
        if (clips.Length == 0) return;

        int index = Random.Range(0, clips.Length);
        footstepSource.pitch = Random.Range(0.9f, 1.1f);
        footstepSource.PlayOneShot(clips[index]);
    }
}