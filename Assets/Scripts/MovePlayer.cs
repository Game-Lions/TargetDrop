using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    InputAction move = new InputAction(type: InputActionType.Value, expectedControlType: nameof(Vector2));
    [SerializeField]
    InputAction speedAndSpin = new InputAction(type: InputActionType.Value, expectedControlType: nameof(Vector2));
    private Rigidbody rb;
    public float speed = 1;
    public float turnspeed = 0.5f;
    public bool ActiveControllers;
    private bool shiftHeld;
    private bool sHeld;
    public float boost;
    public float maxSpeed;
    public float accelerationSpeed;
    public RawImage Speedometer;
    //private float SpeedometerSize;

    [SerializeField]
    GameObject plane;

    public float maxRotationUD;
    //public float rotationSpeedUD;
    private float rotationSpeedUD1 = 5;
    private float rotationSpeedUD2 = 5;
    private float rotationSpeedUD3 = 5;
    private float rotationSpeedUD4 = 5;
    private float currentRotationUD = 0;

    public float maxRotationRL;
    public float rotationSpeedRL;
    private float rotationSpeedRL1;
    private float rotationSpeedRL2;
    private float rotationSpeedRL3;
    private float rotationSpeedRL4;
    private float currentRotationRL = 0;

    public float graphicAcceleration;
    public float graphicStartingSpeed;

    public float rotationSpeedSpin;

    public float maxBoost;
    public float BoostSpeed;
    //private float currentBoost = 0;
    public float boostAnimate;

    bool spinMore = false;

    Vector3 Starting_position;
    Quaternion Starting_rotation;

    //float targetRotation = 1;

    private void OnEnable()
    {
        move.Enable();
        speedAndSpin.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        speedAndSpin.Disable();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ActiveControllers = true;
        Starting_rotation = plane.transform.rotation;
    }

    void FixedUpdate()
    {
        float pointerPosition = (-Speedometer.rectTransform.sizeDelta.y / 2f) - 3f + ((speed / (maxSpeed + 0.8f)) * Speedometer.rectTransform.sizeDelta.y);
        Speedometer.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, pointerPosition);
        if (ActiveControllers)
        {
            // Check if shift is held down
            shiftHeld = Keyboard.current.shiftKey.isPressed;

            // Check if S is held down
            //sHeld = Keyboard.current.sKey.isPressed;

            Vector2 movement = move.ReadValue<Vector2>();
            Vector2 Speedandspin = speedAndSpin.ReadValue<Vector2>();

            // Move player forward
            //if (Speedandspin.y != 0)
            //{
            //if (currentBoost < maxBoost)
            //{
            //    boostAnimate = BoostSpeed * Time.deltaTime;
            //    //plane.transform.position = new Vector3(plane.transform.position.x, plane.transform.position.y, plane.transform.position.z * boostAnimate);
            //    plane.transform.Translate(Vector3.forward * boostAnimate);
            //    currentBoost += boostAnimate;
            //}
            //float pointerPosition = (-Speedometer.rectTransform.sizeDelta.y / 2f) - 3f + ((speed / (maxSpeed+0.8f)) * Speedometer.rectTransform.sizeDelta.y);
            //float divide = Speedometer.rectTransform.sizeDelta.y * accelerationSpeed;
            if (Speedandspin.y > 0 && speed <= maxSpeed)
            {
                speed += accelerationSpeed;
                //Speedometer.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, pointerPosition);
                //SpeedometerPointer.transform.GetChild(0).rectTransform.position += new Vector3(0, 0.5f, 0);
                //Speedometer.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 0.5f);
            }
            else if (Speedandspin.y < 0 && speed > 1f)
            {
                speed -= accelerationSpeed;
                //SpeedometerPointer.rectTransform.position += new Vector3(0, -0.5f, 0);
                //Speedometer.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -0.5f);
                //Speedometer.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, pointerPosition);
            }
            rb.AddForce(transform.forward * speed);
            //}
            //else
            //{
            //    //if (currentBoost > 0)
            //    //{
            //    //    boostAnimate = BoostSpeed * Time.deltaTime;
            //    //    plane.transform.Translate(Vector3.back * boostAnimate);
            //    //    currentBoost -= boostAnimate;
            //    //}
            //    rb.AddForce(transform.forward * speed);
            //}

            // Move player up and down
            if (movement.y != 0)
            {
                rotationSpeedUD3 = graphicStartingSpeed;
                rotationSpeedUD4 = graphicStartingSpeed;
                if (movement.y > 0)
                {
                    if (currentRotationUD < maxRotationUD)
                    {
                        // Gradually increase the rotation speed (acceleration)
                        rotationSpeedUD1 += graphicAcceleration * Time.deltaTime;/////

                        float rotation = rotationSpeedUD1 * Time.deltaTime;
                        plane.transform.Rotate(Vector3.right, rotation, Space.Self);
                        currentRotationUD += rotation;
                    }
                }
                else
                {

                    if (currentRotationUD > -maxRotationUD)
                    {
                        rotationSpeedUD2 += graphicAcceleration * Time.deltaTime;
                        float rotation = rotationSpeedUD2 * Time.deltaTime;
                        plane.transform.Rotate(Vector3.left, rotation, Space.Self);
                        currentRotationUD -= rotation;
                    }
                }
                rb.AddTorque(transform.right * turnspeed * movement.y);
            }
            else
            {
                rotationSpeedUD1 = graphicStartingSpeed;
                rotationSpeedUD2 = graphicStartingSpeed;
                if (currentRotationUD > Starting_rotation.x + 1)
                {
                    rotationSpeedUD3 += graphicAcceleration * Time.deltaTime;/////
                    float rotation = rotationSpeedUD3 * Time.deltaTime;
                    plane.transform.Rotate(Vector3.left, rotation, Space.Self);
                    currentRotationUD -= rotation;
                }
                else if (currentRotationUD < Starting_rotation.x - 1)
                {
                    rotationSpeedUD4 += graphicAcceleration * Time.deltaTime;/////
                    float rotation = rotationSpeedUD4 * Time.deltaTime;
                    plane.transform.Rotate(Vector3.right, rotation, Space.Self);
                    currentRotationUD += rotation;
                }
            }

            // Move player left and right
            if (movement.x != 0)
            {
                rotationSpeedRL3 = graphicStartingSpeed;
                rotationSpeedRL4 = graphicStartingSpeed;
                //if (shiftHeld)
                //{
                //    rb.AddTorque(transform.forward * turnspeed * -movement.x);  // Spin the plane
                //    spinMore = true;
                //}
                //else
                //{
                if (movement.x > 0)
                {
                    if (currentRotationRL < maxRotationRL)
                    {
                        rotationSpeedRL1 += graphicAcceleration * Time.deltaTime;/////
                        float rotation = rotationSpeedRL1 * Time.deltaTime;
                        plane.transform.Rotate(Vector3.up, rotation, Space.Self);
                        currentRotationRL += rotation;
                        spinMore = false;
                    }
                }
                else
                {
                    if (currentRotationRL > -maxRotationRL)
                    {
                        rotationSpeedRL2 += graphicAcceleration * Time.deltaTime;/////
                        float rotation = rotationSpeedRL2 * Time.deltaTime;
                        plane.transform.Rotate(Vector3.down, rotation, Space.Self);
                        currentRotationRL -= rotation;
                        spinMore = false;
                    }
                }
                rb.AddTorque(transform.up * turnspeed * movement.x);      // Turn the plane
                //}
            }
            else
            {
                rotationSpeedRL1 = graphicStartingSpeed;
                rotationSpeedRL2 = graphicStartingSpeed;
                spinMore = true;
                if (currentRotationRL > Starting_rotation.y + 1)
                {
                    rotationSpeedRL3 += graphicAcceleration * Time.deltaTime;/////
                    float rotation = rotationSpeedRL3 * Time.deltaTime;
                    plane.transform.Rotate(Vector3.down, rotation, Space.Self);
                    currentRotationRL -= rotation;
                    spinMore = false;
                }
                else if (currentRotationRL < Starting_rotation.y - 1)
                {
                    rotationSpeedRL4 += graphicAcceleration * Time.deltaTime;/////
                    float rotation = rotationSpeedRL4 * Time.deltaTime;
                    plane.transform.Rotate(Vector3.up, rotation, Space.Self);
                    currentRotationRL += rotation;
                    spinMore = false;
                }
            }

            // Bank plane
            if (Speedandspin.x != 0)
            {
                rb.AddTorque(transform.forward * turnspeed * -Speedandspin.x);  // bank the plane
            }

            if (movement.y == 0 && movement.x == 0)
            {
                float localZRotation = plane.transform.localEulerAngles.z;
                if (localZRotation > 180)
                    localZRotation -= 360;
                float localyRotation = plane.transform.localEulerAngles.y;
                if (localyRotation > 180)
                    localyRotation -= 360;

                if (localZRotation > Starting_rotation.z + 1)
                {
                    float rotation = rotationSpeedSpin * Time.deltaTime;
                    plane.transform.Rotate(Vector3.back, rotation, Space.Self);
                }
                else if (localZRotation < Starting_rotation.z - 1)
                {
                    float rotation = rotationSpeedSpin * Time.deltaTime;
                    plane.transform.Rotate(Vector3.forward, rotation, Space.Self);
                }

                if (localyRotation > Starting_rotation.y + 1 && spinMore)
                {
                    float rotation = rotationSpeedRL * Time.deltaTime;
                    plane.transform.Rotate(Vector3.down, rotation, Space.Self);
                }
                else if (localyRotation < Starting_rotation.y - 1 && spinMore)
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

    public void respawn()
    {
        // Reset velocity and angular velocity
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(-117.11f, -0.08f, -193.58f);
        transform.rotation = Quaternion.Euler(0, 208.1f, 0);
    }

}