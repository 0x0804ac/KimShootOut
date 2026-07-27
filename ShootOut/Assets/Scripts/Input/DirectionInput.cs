using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DirectionInput : MonoBehaviour
{
    [SerializeField] private VisualElement buttonPanel;

    private InputActions actions;
    private Button button;

    private Vector2 from, to;
    private float buttonRadius, boundRadius;
    private bool isPressed;

    void Awake()
    {
        if (actions == null)
        {
            actions = new InputActions();
            button = buttonPanel.Q<Button>(Constants.CONTROLS_DIRECTION_BUTTON);
        }
    }

    void Start()
    {
        isPressed = false;
    }

    void OnEnable()
    {
        RegisterEvents();
    }

    void OnDisable()
    {
        UnregisterEvents();
    }

    private void OngoingSwipe(InputAction.CallbackContext context)
    {
        if (isPressed)
        {
            Vector2 pos = context.ReadValue<Vector2>();
            to.x = pos.x;
            to.y = Screen.height - pos.y;
            MoveButton();
        }
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        boundRadius = button.parent.layout.width / 2.0f;
        buttonRadius = button.layout.width / 2.0f;
        from = button.layout.center;
        to = button.layout.center;
    }

    private void OnButtonPressed(PointerDownEvent evt)
    {
        if (!isPressed)
        {
            button.CaptureMouse();
            from = evt.position;
            isPressed = true;
        }
    }

    private void OnButtonReleased(PointerUpEvent evt)
    {
        if (isPressed)
        {
            to = evt.position;
            MoveButton();
            isPressed = false;
        }
    }

    private void RegisterEvents()
    {
        actions.Gameplay.Swipe.performed += OngoingSwipe;
        actions.Gameplay.Enable();
        button.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        button.RegisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
        button.RegisterCallback<PointerUpEvent>(OnButtonReleased);
    }

    private void UnregisterEvents()
    {
        actions.Gameplay.Swipe.performed -= OngoingSwipe;
        actions.Gameplay.Disable();
        button.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        button.UnregisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
        button.UnregisterCallback<PointerUpEvent>(OnButtonReleased);
    }

    private void MoveButton()
    {
        button.style.translate = Vector2.ClampMagnitude(to - from, boundRadius - buttonRadius);
    }

    public void ResetControls()
    {
        isPressed = false;
        button.style.translate = Vector2.zero;
    }
    /*
    public Transform bound;
    public Transform circle;
    public GameScript gameScript;

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
    */
}
