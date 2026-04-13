using UnityEngine;

public class MainMenuCameraMove : MonoBehaviour
{
    public Transform endPoint; 
    public float moveSpeed = 3f;
    public float rotateSpeed = 2f;

    public MainMenu mainMenu;      

    private bool isMoving = false;

    void Update()
    {
        if (!isMoving && Input.GetKeyDown(KeyCode.Return))
        {
            StartMove();
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                endPoint.position,
                moveSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                endPoint.rotation,
                rotateSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, endPoint.position) < 0.05f)
            {
                isMoving = false;
                mainMenu.StartGame();
            }
        }
    }

    public void StartMove()
    {
        isMoving = true;
    }
}