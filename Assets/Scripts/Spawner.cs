using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public InputAction dropAction;  // Input Action for dropping the gift

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
        GameObject obj = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }

}
