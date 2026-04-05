using UnityEngine;

public class ChandelierSwing : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().AddTorque(Vector3.forward * 3f, ForceMode.Impulse);
    }
}