using System;
using System.Collections;
using TMPro;
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
    public float boost;

    [SerializeField]
    GameObject plane;

    public float maxRotationUD;
    public float rotationSpeedUD;
    private float currentRotationUD = 0;

    public float maxRotationRL;
    public float rotationSpeedRL;
    private float currentRotationRL = 0;

    public float rotationSpeedSpin;

    public float maxBoost;
    public float BoostSpeed;
    private float currentBoost = 0;

    bool spinMore = false;

    Vector3 Starting_position;
    Quaternion Starting_rotation;

    //float targetRotation = 1;

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
        Starting_rotation = plane.transform.rotation;
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
                //if (currentBoost < maxBoost)
                //{
                //    float boostAnimate = BoostSpeed * Time.deltaTime;
                //    //plane.transform.position = new Vector3(plane.transform.position.x, plane.transform.position.y, plane.transform.position.z * boostAnimate);
                //    plane.transform.Translate(Vector3.forward * boostAnimate);
                //    currentBoost += boostAnimate;
                //}
                rb.AddForce(transform.forward * speed * boost);
            }
            else
            {
                //if (currentBoost > 0)
                //{
                //    float boostAnimate = BoostSpeed * Time.deltaTime;
                //    plane.transform.Translate(Vector3.back * boostAnimate);
                //    currentBoost -= boostAnimate;
                //}
                rb.AddForce(transform.forward * speed);
            }

            // Move player up and down
            if (movement.y != 0)
            {
                if (movement.y > 0)
                {
                    if (currentRotationUD < maxRotationUD)
                    {
                        float rotation = rotationSpeedUD * Time.deltaTime;
                        plane.transform.Rotate(Vector3.right, rotation, Space.Self);
                        currentRotationUD += rotation;
                    }
                }
                else
                {
                    if (currentRotationUD > -maxRotationUD)
                    {
                        float rotation = rotationSpeedUD * Time.deltaTime;
                        plane.transform.Rotate(Vector3.left, rotation, Space.Self);
                        currentRotationUD -= rotation;
                    }
                }
                rb.AddTorque(transform.right * turnspeed * movement.y);
            }
            else
            {
                if (currentRotationUD > Starting_rotation.x + 0.5)
                {
                    float rotation = rotationSpeedUD * Time.deltaTime;
                    plane.transform.Rotate(Vector3.left, rotation, Space.Self);
                    currentRotationUD -= rotation;
                }
                else if (currentRotationUD < Starting_rotation.x - 0.5)
                {
                    float rotation = rotationSpeedUD * Time.deltaTime;
                    plane.transform.Rotate(Vector3.right, rotation, Space.Self);
                    currentRotationUD += rotation;
                }
            }

            // Move player left and right
            if (movement.x != 0)
            {
                if (shiftHeld)
                {
                    rb.AddTorque(transform.forward * turnspeed * -movement.x);  // Spin the plane
                }
                else
                {
                    if (movement.x > 0)
                    {
                        if (currentRotationRL < maxRotationRL)
                        {
                            float rotation = rotationSpeedRL * Time.deltaTime;
                            plane.transform.Rotate(Vector3.up, rotation, Space.Self);
                            currentRotationRL += rotation;
                            spinMore = false;
                        }
                    }
                    else
                    {
                        if (currentRotationRL > -maxRotationRL)
                        {
                            float rotation = rotationSpeedRL * Time.deltaTime;
                            plane.transform.Rotate(Vector3.down, rotation, Space.Self);
                            currentRotationRL -= rotation;
                            spinMore = false;
                        }
                    }
                    rb.AddTorque(transform.up * turnspeed * movement.x);      // Turn the plane
                }
            }
            else
            {
                spinMore = true;
                if (currentRotationRL > Starting_rotation.y + 0.5)
                {
                    float rotation = rotationSpeedRL * Time.deltaTime;
                    plane.transform.Rotate(Vector3.down, rotation, Space.Self);
                    currentRotationRL -= rotation;
                    spinMore = false;
                }
                else if (currentRotationRL < Starting_rotation.y - 0.5)
                {
                    float rotation = rotationSpeedRL * Time.deltaTime;
                    plane.transform.Rotate(Vector3.up, rotation, Space.Self);
                    currentRotationRL += rotation;
                    spinMore = false;
                }
            }

            if (movement.y == 0 && movement.x == 0)
            {
                float localZRotation = plane.transform.localEulerAngles.z;
                if (localZRotation > 180)
                    localZRotation -= 360;
                float localyRotation = plane.transform.localEulerAngles.y;
                if (localyRotation > 180)
                    localyRotation -= 360;

                if (localZRotation > Starting_rotation.z + 0.5)
                {
                    float rotation = rotationSpeedSpin * Time.deltaTime;
                    plane.transform.Rotate(Vector3.back, rotation, Space.Self);
                }
                else if (localZRotation < Starting_rotation.z - 0.5)
                {
                    float rotation = rotationSpeedSpin * Time.deltaTime;
                    plane.transform.Rotate(Vector3.forward, rotation, Space.Self);
                }

                if (localyRotation > Starting_rotation.y + 0.5 && spinMore)
                {
                    float rotation = rotationSpeedRL * Time.deltaTime;
                    plane.transform.Rotate(Vector3.down, rotation, Space.Self);
                }
                else if (localyRotation < Starting_rotation.y - 0.5 && spinMore)
                {
                    float rotation = rotationSpeedRL * Time.deltaTime;
                    plane.transform.Rotate(Vector3.up, rotation, Space.Self);
                }
            }
        }
    }
    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

}