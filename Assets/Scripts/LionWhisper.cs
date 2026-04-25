using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LionWhisper : MonoBehaviour
{
    private Transform player;

    [Header("Distance Settings")]
    public float maxDistance = 15f;
    public float minDistance = 2f;

    [Header("Volume Settings")]
    public float minVolume = 0.05f;
    public float maxVolume = 0.6f;

    [Header("Optional Pulse Effect")]
    public bool usePulse = true;
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.15f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.spatialBlend = 1f;

        FindPlayer();

        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    void Update()
    {
        FindPlayer();

        if (player == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        float volume = Mathf.Lerp(maxVolume, minVolume, t);

        if (usePulse)
        {
            float pulse = Mathf.Sin(Time.time * pulseSpeed + transform.position.x) * pulseAmount;
            volume += pulse;
        }

        if (distance > maxDistance)
            volume = 0f;

        audioSource.volume = Mathf.Clamp(volume, 0f, maxVolume);
    }

    void FindPlayer()
    {
        if (player == null)
        {
            PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
            if (pm != null)
                player = pm.transform;
        }
    }
}