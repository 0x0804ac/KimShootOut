using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
    public Transform bound;
    public Transform circle;

    private readonly float minimumLength = 10.0f;

    private InputActions actions;

    private Vector2 startPos;
    private Vector2 endPos;

    void Start()
    {
        startPos = new Vector2(circle.position.x, circle.position.z);
        endPos = new Vector2(circle.position.x, circle.position.z);
        actions = new InputActions();
        actions.Gameplay.Touch.started += OnTouchBegin;
        actions.Gameplay.Touch.canceled += OnTouchEnd;
        actions.Gameplay.Enable();
    }

    void Update()
    {

    }

    private void OnTouchBegin(InputAction.CallbackContext context)
    {
        startPos = actions.Gameplay.Swipe.ReadValue<Vector2>();
    }

    private void OnTouchEnd(InputAction.CallbackContext context)
    {
        endPos = actions.Gameplay.Swipe.ReadValue<Vector2>();
        Vector2 swipeVector = endPos - startPos;
        if (swipeVector.magnitude >= minimumLength)
        {
            Debug.Log(swipeVector);
            swipeVector = Vector2.ClampMagnitude(swipeVector, bound.localScale[0]);
            circle.position = new Vector3(swipeVector.x / 2.0f, circle.position.y, swipeVector.y / 2.0f);
        }
    }
}
