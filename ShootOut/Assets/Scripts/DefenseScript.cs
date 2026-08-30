using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DefenseScript : MonoBehaviour
{
    [SerializeField] private GameObject defender, goal, preview;
    [SerializeField] private bool showPreview;

    public TemplateContainer root;

    private InputActions input;
    private Animator animator;
    private VisualElement mainPanel, buttonPanel;
    private Button directionButton;
    private Vector2 from, to;
    private float buttonRadius, boundRadius, lastClickTime;
    private bool isSwiping;

    void Awake()
    {
        if (input != null) return;
        input = new InputActions();
        animator = defender.GetComponent<Animator>();
        mainPanel = root.Q<VisualElement>(Constants.MAIN_PANEL);
        buttonPanel = mainPanel.Q<TemplateContainer>(Constants.DIRECTION_CONTAINER).Q<Button>(Constants.CONTROLS_BUTTON_BOUND);
        directionButton = buttonPanel.Q<Button>(Constants.CONTROLS_DIRECTION_BUTTON);
        from = new Vector2();
        to = new Vector2();
        isSwiping = false;
    }

    void OnEnable()
    {
        RegisterEvents();
    }

    void OnDisable()
    {
        UnregisterEvents();
    }

    void FixedUpdate()
    {
        float currentTime = Time.fixedTime;
        preview.SetActive(isSwiping || currentTime - lastClickTime < 1f);
    }

    private void RegisterEvents()
    {
        input.Gameplay.Swipe.performed += OnSwipe;
        input.Gameplay.Enable();
        directionButton.RegisterCallback<GeometryChangedEvent>(UpdateRadius);
        directionButton.RegisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
        directionButton.RegisterCallback<PointerUpEvent>(OnButtonReleased);
    }

    private void UnregisterEvents()
    {
        input.Gameplay.Swipe.performed -= OnSwipe;
        input.Gameplay.Disable();
        directionButton.UnregisterCallback<GeometryChangedEvent>(UpdateRadius);
        directionButton.UnregisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
        directionButton.UnregisterCallback<PointerUpEvent>(OnButtonReleased);
    }

    private void OnButtonPressed(PointerDownEvent evt)
    {
        if (!isSwiping && directionButton == evt.target)
        {
            evt.target.CaptureMouse();
            from = evt.position;
            isSwiping = true;
        }
    }

    private void OnSwipe(InputAction.CallbackContext context)
    {
        if (isSwiping)
        {
            Vector2 pos = context.ReadValue<Vector2>();
            to.x = pos.x;
            to.y = Screen.height - pos.y;
            MoveButton();
            if (showPreview) ShowPreview(DefenderVelocity(directionButton));
        }
    }

    private void OnButtonReleased(PointerUpEvent evt)
    {
        if (isSwiping)
        {
            to = evt.position;
            MoveButton();
            HidePreview();
            Vector3 vector = DefenderVelocity(directionButton);
            animator.SetFloat(Constants.ANIMATOR_VELOCITY_X, vector.x);
            animator.SetFloat(Constants.ANIMATOR_VELOCITY_Y, vector.y);
            animator.SetFloat(Constants.ANIMATOR_VELOCITY_Z, vector.z);
            isSwiping = false;
        }
    }

    private void UpdateRadius(GeometryChangedEvent evt)
    {
        boundRadius = buttonPanel.layout.width / 2.0f;
        buttonRadius = directionButton.layout.width / 2.0f;
        from = directionButton.layout.center;
        to = directionButton.layout.center;
    }

    private void MoveButton()
    {
        directionButton.style.translate = Vector2.ClampMagnitude(to - from, boundRadius - buttonRadius);
    }

    private void ShowPreview(Vector3 initialVelocity)
    {
        lastClickTime = Time.fixedTime;
        float t = 1.0f;
        if (float.IsNaN(t))
        {
            HidePreview();
            return;
        }
        Vector3 destination = (Constants.GOAL_LINE + initialVelocity * t + 0.5f * t * t * Physics.gravity) / defender.GetComponent<Rigidbody>().mass;
        if (goal.GetComponent<BoxCollider>().bounds.Contains(destination + Vector3.forward))
        {
            preview.transform.position = destination;
            preview.SetActive(true);
        }
        else HidePreview();
    }

    private void HidePreview()
    {
        preview.SetActive(false);
    }

    public void ResetControls()
    {
        isSwiping = false;
        directionButton.style.translate = Vector2.zero;
    }

    public void RandomizeControls()
    {
        directionButton.style.translate = new Vector2(RandomValue(), RandomValue());
    }

    private Vector3 DefenderVelocity(Button button)
    {
        return new()
        {
            x = button.style.translate.value.x.value * Constants.MULTIPLIER_X,
            y = button.style.translate.value.y.value * Constants.MULTIPLIER_Y,
            z = StaticValues.defender.Speed * -Constants.MULTIPLIER
        };
    }

    private float RandomValue()
    {
        return (Random.value * 2 - 1) * (boundRadius - buttonRadius);
    }
}
