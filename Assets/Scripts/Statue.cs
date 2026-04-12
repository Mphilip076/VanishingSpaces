using UnityEngine;
using UnityEngine.AI;

public class WeepingStatue : MonoBehaviour
{
    public Transform player;
    public Light flashlight;
    public NavMeshAgent agent;
    public Animator animator;

    public AudioSource moveAudioSource;
    public AudioSource whisperAudioSource;

    public float raycastHeight = 1.2f;

    public float whisperResumeDelay = 0.25f;
    public float maxWhisperDistance = 15f;
    public float minWhisperVolume = 0.05f;
    public float maxWhisperVolume = 0.35f;

    private bool whisperWaitingToPlay = false;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponent<Animator>();

        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length > 0 && moveAudioSource == null)
            moveAudioSource = sources[0];

        if (sources.Length > 1 && whisperAudioSource == null)
            whisperAudioSource = sources[1];
    }

    void Update()
    {
        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh)
            return;

        bool hitByLight = IsHitByFlashlight();

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
        }
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