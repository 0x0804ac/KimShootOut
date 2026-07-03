using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ButtonScript : MonoBehaviour
{
    public UIDocument document;
    public ScriptManager manager;

    private InputActions actions;
    private Button button;
    private SliderInt slider;

    private Vector2 from, to, vec;
    private float buttonRadius, boundRadius;
    private bool isPressed;

    void Start()
    {
        isPressed = false;
    }

    void OnEnable()
    {
        Init();
        button.clicked += OnButtonClicked;
        actions.Gameplay.Swipe.performed += OngoingSwipe;
        actions.Gameplay.Enable();
    }

    void OnDisable()
    {
        button.clicked -= OnButtonClicked;
        actions.Gameplay.Swipe.performed -= OngoingSwipe;
        actions.Gameplay.Disable();
        button.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        button.UnregisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
        button.UnregisterCallback<PointerUpEvent>(OnButtonReleased);
    }

    void Update()
    {
        
    }

    private void OnButtonClicked()
    {
        print("Button clicked!");
    }

    private void OngoingSwipe(InputAction.CallbackContext context)
    {
        if (isPressed)
        {
            Vector2 pos = context.ReadValue<Vector2>();
            pos.y = Screen.height - pos.y;
            to = pos;
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
            //button.CaptureMouse();
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
            if (manager.gameScript.IsReady())
            {
                if (manager.gameScript.Turn % 2 == 0)
                {
                    manager.gameScript.Shoot(new Vector2(vec.x, -vec.y) / (boundRadius - buttonRadius), slider.value * 0.01f);
                }
                else
                {
                    manager.gameScript.Goalkeep(new Vector2(vec.x, -vec.y));
                }
            }
            isPressed = false;
        }
    }

    private void Init()
    {
        if (actions == null)
        {
            actions = new InputActions();
            button = document.rootVisualElement.Q<Button>("direction-button");
            button.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            button.RegisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
            button.RegisterCallback<PointerUpEvent>(OnButtonReleased);
            slider = document.rootVisualElement.Q<SliderInt>("power-slider");
        }
    }

    private void MoveButton()
    {
        vec = Vector2.ClampMagnitude(to - from, boundRadius - buttonRadius);
        button.style.translate = vec;
    }

    public void ResetControls()
    {
        isPressed = false;
        button.style.translate = Vector2.zero;
        slider.value = (slider.lowValue + slider.highValue) / 2;
        slider.visible = manager.gameScript.Turn % 2 == 0;
    }
    /*
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropManipulator : PointerManipulator
{
    private Vector2 _startPosition;
    private Vector3 _startPointerPosition;
    private bool _isDragging;
    private VisualElement _rootContainer;

    public DragAndDropManipulator(VisualElement rootContainer)
    {
        _rootContainer = rootContainer;
        _isDragging = false;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        // Cache initial coordinate and pointer position
        _startPosition = new Vector2(target.layout.x, target.layout.y);
        _startPointerPosition = evt.position;

        // Capture pointer to ensure tracking if dragging rapidly
        target.CapturePointer(evt.pointerId);
        _isDragging = true;
        evt.StopPropagation();
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!_isDragging || !target.HasPointerCapture(evt.pointerId)) 
            return;

        // Calculate layout delta from starting pointer click
        Vector3 delta = evt.position - _startPointerPosition;

        // Update target layout styling to move it dynamically
        target.style.left = _startPosition.x + delta.x;
        target.style.top = _startPosition.y + delta.y;
        
        // Ensure style position type is absolute for precise movement
        target.style.position = Position.Absolute;
        
        evt.StopPropagation();
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (!_isDragging) 
            return;

        // Release pointer capture and reset state
        target.ReleasePointer(evt.pointerId);
        _isDragging = false;
        evt.StopPropagation();

        // Optional: Execute drop zone layout query logic here
        ResolveDrop(evt.position);
    }

    private void ResolveDrop(Vector2 pointerPosition)
    {
        // Simple over-the-top layout verification for a drop zone
        // e.g., Find slots or resolve standard gameplay drops
    }
}

using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIController : MonoBehaviour
{
    private UIDocument _uiDocument;

    void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;

        // Query your target button (Replace "MyButton" with your button's name/ID)
        Button dragButton = root.Q<Button>("MyButton");

        if (dragButton != null)
        {
            // Instantiate and inject the root element container boundary
            dragButton.AddManipulator(new DragAndDropManipulator(root));
        }
    }
}
     */
}
