using TMPro;
using UnityEngine;

public class FaceCameraTMPro : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Automatically assign the main camera if not set
        }
    }

    void Update()
    {
        // Always make the TextMeshPro face the camera
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0); // To correct rotation (TextMeshPro faces wrong way by default)
    }
}
