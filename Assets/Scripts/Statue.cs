using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class WeepingStatue : MonoBehaviour
{
    public Transform player;
    public Light flashlight;
    public NavMeshAgent agent;
    public Animator animator;

    public AudioSource moveAudioSource;
    public AudioSource whisperAudioSource;
    public AudioSource flashlightHitAudioSource;
    public AudioSource attackJumpscareAudioSource;

    public float raycastHeight = 1.2f;

    public float whisperResumeDelay = 0.25f;
    public float maxWhisperDistance = 15f;
    public float minWhisperVolume = 0.05f;
    public float maxWhisperVolume = 0.35f;

    public float attackDistance = 2f;
    public float jumpscareDuration = 1.2f;
    public float attackDelayAfterSpawn = 2f;
    public float jumpscareForwardOffset = 0.05f;

    public Transform jumpscarePoint;

    private bool whisperWaitingToPlay = false;
    private bool isAttacking = false;
    private bool wasHitByLightLastFrame = false;
    private float spawnTime;

    void Start()
    {
        spawnTime = Time.time;

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponent<Animator>();

        FindRuntimeReferences();
    }

    void Update()
    {
        FindRuntimeReferences();

        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh || player == null || isAttacking)
            return;

        bool hitByLight = IsHitByFlashlight();

        // flashlight hit sound only when light first touches statue
        if (hitByLight && !wasHitByLightLastFrame)
        {
            if (flashlightHitAudioSource != null)
                flashlightHitAudioSource.Play();
        }

        if (hitByLight)
        {
            agent.isStopped = true;

            if (animator != null)
                animator.speed = 0f;

            if (moveAudioSource != null && moveAudioSource.isPlaying)
                moveAudioSource.Stop();

            StopWhisperImmediately();
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            if (animator != null)
                animator.speed = 1f;

            if (moveAudioSource != null && !moveAudioSource.isPlaying)
                moveAudioSource.Play();

            StartWhisperWithDelay();
            UpdateWhisperIntensity();

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (Time.time >= spawnTime + attackDelayAfterSpawn && distanceToPlayer <= attackDistance)
            {
                StartCoroutine(AttackSequence());
            }
        }

        wasHitByLightLastFrame = hitByLight;
    }

    void FindRuntimeReferences()
    {
        if (player == null)
        {
            PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
            if (pm != null)
                player = pm.transform;
        }

        if (flashlight == null && player != null)
        {
            Light[] lights = player.GetComponentsInChildren<Light>(true);

            foreach (Light l in lights)
            {
                if (l.type == LightType.Spot)
                {
                    flashlight = l;
                    break;
                }
            }
        }

        if (jumpscarePoint == null)
        {
            GameObject pointObject = GameObject.FindGameObjectWithTag("JumpscarePoint");
            if (pointObject != null)
                jumpscarePoint = pointObject.transform;
        }
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true;

        if (agent != null)
            agent.isStopped = true;

        if (moveAudioSource != null && moveAudioSource.isPlaying)
            moveAudioSource.Stop();

        StopWhisperImmediately();

        // make sure statue is no longer frozen
        if (animator != null)
            animator.speed = 1f;

        // snap statue right into the camera area
        if (jumpscarePoint != null)
        {
            transform.position = jumpscarePoint.position + jumpscarePoint.forward * jumpscareForwardOffset;
            transform.rotation = jumpscarePoint.rotation;
        }

        // play attack animation
        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
            animator.Update(0f);
        }

        // play actual attack scare sound
        if (attackJumpscareAudioSource != null)
        {
            attackJumpscareAudioSource.Stop();
            attackJumpscareAudioSource.Play();
        }

        yield return new WaitForSeconds(jumpscareDuration);

        SendPlayerToRandomRoom();
    }

    void SendPlayerToRandomRoom()
    {
        if (Room.allRooms == null || Room.allRooms.Count == 0)
            return;

        Room chosenRoom = Room.allRooms[Random.Range(0, Room.allRooms.Count)];

        int tries = 0;
        while ((chosenRoom == Room.currentRoom || chosenRoom.SceneName() == "StartScene") && tries < 30)
        {
            chosenRoom = Room.allRooms[Random.Range(0, Room.allRooms.Count)];
            tries++;
        }

        if (chosenRoom != null)
            Room.SetScene(chosenRoom.SceneName());
    }

    void StartWhisperWithDelay()
    {
        if (whisperAudioSource == null || whisperAudioSource.isPlaying || whisperWaitingToPlay)
            return;

        whisperWaitingToPlay = true;
        Invoke(nameof(PlayWhisper), whisperResumeDelay);
    }

    void PlayWhisper()
    {
        whisperWaitingToPlay = false;

        if (whisperAudioSource != null && !whisperAudioSource.isPlaying)
            whisperAudioSource.Play();
    }

    void StopWhisperImmediately()
    {
        whisperWaitingToPlay = false;
        CancelInvoke(nameof(PlayWhisper));

        if (whisperAudioSource != null && whisperAudioSource.isPlaying)
            whisperAudioSource.Stop();
    }

    void UpdateWhisperIntensity()
    {
        if (whisperAudioSource == null || player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);
        float t = 1f - Mathf.Clamp01(distance / maxWhisperDistance);

        whisperAudioSource.volume = Mathf.Lerp(minWhisperVolume, maxWhisperVolume, t);
        whisperAudioSource.pitch = Mathf.Lerp(0.9f, 1.05f, t);
    }

    bool IsHitByFlashlight()
    {
        if (flashlight == null || !flashlight.enabled)
            return false;

        Vector3 statueTarget = transform.position + Vector3.up * raycastHeight;
        Vector3 directionToStatue = (statueTarget - flashlight.transform.position).normalized;

        float angle = Vector3.Angle(flashlight.transform.forward, directionToStatue);
        if (angle > flashlight.spotAngle * 0.5f)
            return false;

        float distance = Vector3.Distance(flashlight.transform.position, statueTarget);

        if (Physics.Raycast(flashlight.transform.position, directionToStatue, out RaycastHit hit, distance))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
                return true;
        }

        return false;
    }
}