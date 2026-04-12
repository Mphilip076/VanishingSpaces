using UnityEngine;
using UnityEngine.AI;

public class WeepingStatue : MonoBehaviour
{
    public Transform player;
    public Light flashlight;
    public NavMeshAgent agent;
    public Animator animator; // ADD THIS

    public float raycastHeight = 1.2f;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null) // ADD THIS
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh)
            return;

        bool hitByLight = IsHitByFlashlight();

        if (hitByLight)
        {
            // Freeze movement
            agent.isStopped = true;

            // Freeze animation EXACTLY where it is
            if (animator != null)
                animator.speed = 0f;
        }
        else
        {
            // Move toward player
            agent.isStopped = false;
            agent.SetDestination(player.position);

            // Resume animation
            if (animator != null)
                animator.speed = 1f;
        }
    }

    bool IsHitByFlashlight()
    {
        if (flashlight == null || !flashlight.enabled)
            return false;

        Vector3 statueTarget = transform.position + Vector3.up * raycastHeight;

        Vector3 directionToStatue = (statueTarget - flashlight.transform.position).normalized;

        // Check if inside flashlight cone
        float angle = Vector3.Angle(flashlight.transform.forward, directionToStatue);
        if (angle > flashlight.spotAngle * 0.5f)
            return false;

        float distance = Vector3.Distance(flashlight.transform.position, statueTarget);

        // Check if light is not blocked
        if (Physics.Raycast(flashlight.transform.position, directionToStatue, out RaycastHit hit, distance))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
                return true;
        }

        return false;
    }
}