using UnityEngine;

public class AirportCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Spawner"))
        {
            Destroy(other.gameObject);
        }
    }
}
