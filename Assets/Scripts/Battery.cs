using UnityEngine;

public class Battery : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Make sure it shows
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp()
    {
        // Make it disappear
        this.gameObject.SetActive(false);
    }

    // Placeholder: Pick it up on a collision
    private void OnCollisionEnter(Collision collision)
    {
        PickUp();
    }
}
