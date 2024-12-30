using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    InputAction move = new InputAction(type: InputActionType.Value, expectedControlType: nameof(Vector2));
    private Rigidbody rb;
    public float speed = 1;
    public float turnspeed = 0.5f;
    public bool ActiveControllers;
    private bool shiftHeld;
    private bool sHeld;

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
        rb = GetComponent<Rigidbody>();
        ActiveControllers = true;
    }

    void FixedUpdate()
    {
        if (ActiveControllers)
        {
            // Check if shift is held down
            shiftHeld = Keyboard.current.shiftKey.isPressed;

            // Check if S is held down
            sHeld = Keyboard.current.sKey.isPressed;

            Vector2 movement = move.ReadValue<Vector2>();
            // Move player forward
            if (sHeld)
            {
                
                rb.AddForce(transform.forward * speed * 3);
            }
            else
            {
                rb.AddForce(transform.forward * speed);
            }
            

            // Move player up
            rb.AddTorque(transform.right * turnspeed * movement.y);

            if (shiftHeld)
            {
                rb.AddTorque(transform.forward * turnspeed * -movement.x);  // Spin the plane
            }
            else
            {
                rb.AddTorque(transform.up * turnspeed * movement.x);      // Turn the plane
            }
        }
    }
    public void setSpeed(float speed)
    {
        this.speed = speed;
    }
}