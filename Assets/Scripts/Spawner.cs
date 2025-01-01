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
        GameObject parentObject = transform.parent.gameObject;
        GameObject obj = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        obj.transform.Rotate(new Vector3(-90, parentObject.transform.rotation.y, parentObject.transform.rotation.z), Space.Self);
    }
}
