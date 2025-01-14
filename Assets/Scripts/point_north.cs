using UnityEngine;

public class PointNorth : MonoBehaviour
{

    public float leanAngle = 19f; // Angle to lean the compass forward
    void Update()
    {

        // Make the object face the global north direction (positive Z-axis)
        transform.rotation = Quaternion.LookRotation(Vector3.left);

        transform.rotation *= Quaternion.Euler(0, 0, leanAngle);
    }
}