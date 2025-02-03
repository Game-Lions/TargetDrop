using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // The object the camera follows
    private float initialYRotation;  // The object's initial Y rotation
    //private Vector3 initialposition;  // The object's initial Y rotation

    void Start()
    {
        // Store the target's initial Y rotation
        initialYRotation = target.rotation.eulerAngles.y;
        //initialposition = transform.position;
    }

    void Update()
    {
        // Keep the camera's position relative to the target
        //transform.position = new Vector3(initialposition.x, target.transform.position.y, initialposition.z);

        // Apply only the initial Y rotation to the camera (ignore changes to X and Z)
        transform.rotation = Quaternion.Euler(28.9f, initialYRotation, 0f);
    }
}
