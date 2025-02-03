using UnityEngine;
public class SpinObject : MonoBehaviour
{
    public Rigidbody rb;  // Reference to the object's Rigidbody
    public Vector3 spinSpeed;  // The spin speed for each axis

    void Start()
    {
        // Get the Rigidbody component if not assigned
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Apply angular velocity to spin the object
        rb.angularVelocity = spinSpeed;
    }
}
