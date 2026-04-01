using UnityEngine;

public class MovingFurniture : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 1f;
    public float unseenDelay = 0.5f;

    public Light flashlight;

    private Transform target;
    private float unseenTimer = 0f;
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponentInChildren<Renderer>();
        transform.position = pointA.position;
        target = pointB;
    }

    void Update()
    {
        if (pointA == null || pointB == null)
            return;

        if (IsInFlashlightCone())
        {
            unseenTimer = 0f;
            return;
        }

        unseenTimer += Time.deltaTime;

        if (unseenTimer >= unseenDelay)
        {
            Move();
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }

    bool IsInFlashlightCone()
    {
        if (flashlight == null || !flashlight.enabled)
            return false;

        Vector3 checkPoint = GetCheckPoint();
        Vector3 toObject = checkPoint - flashlight.transform.position;

        if (toObject.magnitude > flashlight.range)
            return false;

        float angle = Vector3.Angle(flashlight.transform.forward, toObject);
        return angle <= flashlight.spotAngle * 0.5f;
    }

    Vector3 GetCheckPoint()
    {
        if (objectRenderer != null)
            return objectRenderer.bounds.center;

        return transform.position;
    }
}