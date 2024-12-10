using UnityEngine;
using UnityEngine.InputSystem;

public class Drop : MonoBehaviour
{
    [SerializeField]
    InputAction action = new InputAction(type: InputActionType.Button, expectedControlType: "Button");
    public GameObject poopPrefab; // Assign your prefab here in the Inspector
    private bool isPressed = false;

    void Awake()
    {
        action.Enable();
    }

    void OnDestroy()
    {
        action.Disable();
    }

    void Update()
    {
        bool buttonState = action.ReadValue<float>() > 0;

        if (buttonState && !isPressed)
        {
            PerformAction();
        }

        isPressed = buttonState;
    }

    void PerformAction()
    {
        Debug.Log("Button pressed!");
        if (GetComponent<Drop_erea>().isInsideArea)
        {
            Instantiate(poopPrefab, transform.position, Quaternion.identity);
        }
    }
}
