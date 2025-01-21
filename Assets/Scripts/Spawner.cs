using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public InputAction dropAction;  // Input Action for dropping the gift
    public float throwForce;
    private bool canDrop = true;    // A flag to prevent multiple drops during the cooldown
    public float timeBetweenEachDrop;
    public AudioSource dropSound;

    private void OnEnable()
    {
        dropAction.Enable();
        dropAction.performed += OnDrop;
    }

    private void OnDisable()
    {
        dropAction.performed -= OnDrop;
        dropAction.Disable();
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        if (canDrop)
        {
            dropSound.Play();
            canDrop = false;  // Prevent dropping during cooldown
            GameObject parentObject = transform.parent.gameObject;
            GameObject obj = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            // Apply a force or velocity to throw the object forward
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //Vector3 forwardDirection = transform.forward; // Forward direction of the parent object
                //Vector3 upwardDirection = transform.up;      // Upward direction
                //Vector3 combinedDirection = forwardDirection + upwardDirection;
                //rb.AddForce(combinedDirection.normalized * throwForce, ForceMode.VelocityChange);

                //Vector3 forwardDirection = transform.forward; // Forward direction of the parent object
                //Vector3 upwardDirection = transform.up;      // Upward direction
                //Vector3 combinedDirection = forwardDirection + upwardDirection;
                //rb.AddForce(forwardDirection.normalized * throwForce, ForceMode.VelocityChange);
            }
            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(timeBetweenEachDrop);  // Wait for 5 seconds
        canDrop = true;  // Allow dropping again
    }
}
