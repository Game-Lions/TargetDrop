using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    InputAction move = new InputAction(type: InputActionType.Value, expectedControlType: nameof(Vector2));
    private Rigidbody2D rb;
    public float speed = 1;
    public float turnspeed = 0.5f;
    private Camera cam;
    private void OnEnable()
    {
        move.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.up * speed);
        Vector2 movement = move.ReadValue<Vector2>();
        rb.AddTorque(-turnspeed * movement.x);
        // Keep camera rotation fixed
        //cam.transform.rotation = Quaternion.Euler(50f, 0f, 0f);
    }
}
