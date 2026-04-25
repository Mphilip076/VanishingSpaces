using UnityEngine;

public class RotateItem : InteractableItem
{
    [Header("Rotate to")]
    public int x;
    public int y;
    public int z;
    public AudioSource rotateAudioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactMessage = "Press E to open door";
        interactRange = 4f;
        interactKey = KeyCode.E;
    }

    public override void OnInteract(){
        gameObject.transform.rotation = Quaternion.Euler(x, y, z);
        if(rotateAudioSource != null)
            rotateAudioSource.Play();

        canInteract = false;
    }
}
