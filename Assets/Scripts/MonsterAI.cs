// // 

// using UnityEngine;
// using UnityEngine.AI;

// public class MonsterAI : MonoBehaviour
// {
//     [Header("Detection")]
//     public float detectionRange = 5f;

//     [Header("Jumpscare")]
//     public float jumpscareSpeed = 20f;
//     public float disappearDelay = 2f;

//     [Header("Patrol")]
//     public Transform patrolPointA;
//     public Transform patrolPointB;
//     public float patrolSpeed = 2f;

//     [Header("Jumpscare Position")]
//     public Transform windowPosition; // empty object AT the window outside!

//     [Header("Audio")]
//     public AudioSource jumpscareSound;

//     public enum State { Patrol, MoveToWindow, Jumpscare, Disappear }
//     public State currentState = State.Patrol;

//     private NavMeshAgent agent;
//     private Animator animator;
//     private Transform player;
//     private Transform currentPatrolTarget;
//     private float disappearTimer = 0f;
//     private bool hasJumpscared = false;

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         animator = GetComponentInChildren<Animator>();

//         GameObject playerObj = GameObject.FindWithTag("Player");
//         if (playerObj != null)
//             player = playerObj.transform;

//         currentPatrolTarget = patrolPointA;
//         currentState = State.Patrol;
//     }

//     void Update()
//     {
//         if (player == null) return;

//         float distanceToPlayer = Vector3.Distance(
//             transform.position, player.position
//         );

//         switch (currentState)
//         {
//             case State.Patrol:
//                 HandlePatrol(distanceToPlayer);
//                 break;
//             case State.MoveToWindow:
//                 HandleMoveToWindow();
//                 break;
//             case State.Jumpscare:
//                 HandleJumpscare();
//                 break;
//             case State.Disappear:
//                 HandleDisappear();
//                 break;
//         }
//     }

//     void HandlePatrol(float distance)
//     {
//         agent.isStopped = false;
//         agent.speed = patrolSpeed;
//         animator.SetFloat("Speed", 0.5f);

//         if (distance < detectionRange && CanSeePlayer() && !hasJumpscared)
//         {
//             // Move to window position first!
//             currentState = State.MoveToWindow;
//             return;
//         }

//         if (patrolPointA == null || patrolPointB == null) return;

//         agent.SetDestination(currentPatrolTarget.position);

//         if (agent.velocity.sqrMagnitude > 0.1f)
//         {
//             Quaternion targetRotation = Quaternion.LookRotation(
//                 agent.velocity.normalized
//             );
//             transform.rotation = Quaternion.Slerp(
//                 transform.rotation,
//                 targetRotation,
//                 Time.deltaTime * 5f
//             );
//         }

//         if (agent.remainingDistance < 0.5f)
//         {
//             currentPatrolTarget = currentPatrolTarget == patrolPointA
//                 ? patrolPointB
//                 : patrolPointA;
//         }
//     }

//     void HandleMoveToWindow()
//     {
//         // Run super fast to window position!
//         agent.isStopped = false;
//         agent.speed = jumpscareSpeed;
//         animator.SetFloat("Speed", 1f);

//         if (windowPosition != null)
//             agent.SetDestination(windowPosition.position);

//         // Reached window → do jumpscare!
//         if (agent.remainingDistance < 0.8f)
//         {
//             currentState = State.Jumpscare;
//             agent.isStopped = true;
//         }
//     }

//     void HandleJumpscare()
//     {
//         agent.isStopped = true;
//         animator.SetFloat("Speed", 0f);

//         // Face the player through window
//         Vector3 direction = (player.position - transform.position).normalized;
//         direction.y = 0f;
//         transform.rotation = Quaternion.LookRotation(direction);

//         // Play jumpscare sound
//         if (jumpscareSound != null && !jumpscareSound.isPlaying)
//             jumpscareSound.Play();

//         hasJumpscared = true;
//         disappearTimer = 0f;
//         currentState = State.Disappear;
//     }

//     void HandleDisappear()
//     {
//         agent.isStopped = true;
//         disappearTimer += Time.deltaTime;

//         if (disappearTimer >= disappearDelay)
//             gameObject.SetActive(false);
//     }

//     bool CanSeePlayer()
//     {
//         if (player == null) return false;

//         Vector3 directionToPlayer = player.position - transform.position;
//         float distanceToPlayer = directionToPlayer.magnitude;

//         RaycastHit hit;
//         if (Physics.Raycast(
//             transform.position + Vector3.up,
//             directionToPlayer.normalized,
//             out hit,
//             distanceToPlayer))
//         {
//             if (hit.transform.CompareTag("Player"))
//                 return true;

//             return false;
//         }

//         return false;
//     }

//     void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, detectionRange);
//     }
// }

using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRange = 5f;

    [Header("Jumpscare")]
    public float disappearDelay = 1.5f;

    [Header("Patrol")]
    public Transform patrolPointA;
    public Transform patrolPointB;
    public float patrolSpeed = 2f;

    [Header("Audio")]
    public AudioSource jumpscareSound;

    public enum State { Patrol, Jumpscare, Disappear }
    public State currentState = State.Patrol;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private Transform currentPatrolTarget;
    private float disappearTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        jumpscareSound = GetComponent<AudioSource>(); // auto finds it!

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        currentPatrolTarget = patrolPointA;
        currentState = State.Patrol;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(
            transform.position, player.position
        );

        switch (currentState)
        {
            case State.Patrol:
                HandlePatrol(distanceToPlayer);
                break;
            case State.Jumpscare:
                HandleJumpscare();
                break;
            case State.Disappear:
                HandleDisappear();
                break;
        }
    }

    void HandlePatrol(float distance)
    {
        agent.isStopped = false;
        agent.speed = patrolSpeed;
        animator.SetFloat("Speed", 0.5f);

        if (patrolPointA == null || patrolPointB == null) return;

        agent.SetDestination(currentPatrolTarget.position);

        // Switch patrol target when reached
        if (agent.remainingDistance < 0.5f)
        {
            currentPatrolTarget = currentPatrolTarget == patrolPointA
                ? patrolPointB
                : patrolPointA;
        }

        // Player near window → jumpscare!
        if (distance < detectionRange)
        {
            currentState = State.Jumpscare;
        }
    }

    void HandleJumpscare()
    {
        agent.isStopped = true;

        // Teleport right in front of player!
        Vector3 jumpscarePos = player.position + player.forward * 1.5f;
        transform.position = jumpscarePos;

        // Face the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.LookRotation(direction);

        // Play sound
        if (jumpscareSound != null && !jumpscareSound.isPlaying)
            jumpscareSound.Play();

        animator.SetFloat("Speed", 0f);

        disappearTimer = 0f;
        currentState = State.Disappear;
    }

    void HandleDisappear()
    {
        disappearTimer += Time.deltaTime;

        if (disappearTimer >= disappearDelay)
            gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}