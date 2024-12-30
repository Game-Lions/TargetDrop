using UnityEngine;

public class ObjectUpDownMovement : MonoBehaviour
{
    public float speed = 1.0f;    // Speed of the up and down movement
    public float amplitude = 2.0f; // Height of the up and down movement
    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        float y = startingPosition.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startingPosition.x, y, startingPosition.z);
    }
}
