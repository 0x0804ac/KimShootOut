using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class AttackScript : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private GameObject attacker, goal, ball, preview;

    private InputActions input;
    private Animator animator;
    private VisualElement root, mainPanel, buttonPanel;
    private Button directionButton;
    private SliderInt powerSlider;
    private Vector2 from, to;
    private float buttonRadius, boundRadius, lastClickTime;
    private bool isSwiping, isSliding;

    void Awake()
    {
        if (input != null) return;
        input = new InputActions();
        animator = attacker.GetComponent<Animator>();
        root = document.rootVisualElement;
        mainPanel = root.Q<VisualElement>(Constants.MAIN_PANEL);
        buttonPanel = mainPanel.Q<TemplateContainer>(Constants.DIRECTION_CONTAINER).Q<Button>(Constants.CONTROLS_BUTTON_BOUND);
        directionButton = buttonPanel.Q<Button>(Constants.CONTROLS_DIRECTION_BUTTON);
        powerSlider = mainPanel.Q<TemplateContainer>(Constants.POWER_CONTAINER).Q<SliderInt>(Constants.CONTROLS_POWER_SLIDER);
        from = new Vector2();
        to = new Vector2();
        isSwiping = false;
        isSliding = false;
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
        preview.SetActive(isSwiping || isSliding || currentTime - lastClickTime < 1f);
    }

    private void RegisterEvents()
    {
        input.Gameplay.Swipe.performed += OnSwipe;
        input.Gameplay.Enable();
        directionButton.RegisterCallback<GeometryChangedEvent>(UpdateRadius);
        directionButton.RegisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
        directionButton.RegisterCallback<PointerUpEvent>(OnButtonReleased);
        powerSlider.RegisterCallback<PointerDownEvent>(OnSliderPressed, TrickleDown.TrickleDown);
        powerSlider.RegisterCallback<PointerUpEvent>(OnSliderReleased);
        powerSlider.RegisterValueChangedCallback(OnSliderClick);
    }

    private void UnregisterEvents()
    {
        input.Gameplay.Swipe.performed -= OnSwipe;
        input.Gameplay.Disable();
        directionButton.UnregisterCallback<GeometryChangedEvent>(UpdateRadius);
        directionButton.UnregisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
        directionButton.UnregisterCallback<PointerUpEvent>(OnButtonReleased);
        powerSlider.UnregisterCallback<PointerDownEvent>(OnSliderPressed, TrickleDown.TrickleDown);
        powerSlider.UnregisterCallback<PointerUpEvent>(OnSliderReleased);
        powerSlider.UnregisterValueChangedCallback(OnSliderClick);
    }

    private void UpdateRadius(GeometryChangedEvent evt)
    {
        boundRadius = buttonPanel.layout.width / 2.0f;
        buttonRadius = directionButton.layout.width / 2.0f;
        from = directionButton.layout.center;
        to = directionButton.layout.center;
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
            ShowPreview(AttackerVelocity(directionButton, powerSlider));
        }
    }

    private void OnButtonReleased(PointerUpEvent evt)
    {
        if (isSwiping)
        {
            to = evt.position;
            MoveButton();
            HidePreview();
            Vector3 vector = StaticValues.attacker.Kick(AttackerVelocity(directionButton, powerSlider));
            animator.SetFloat(Constants.ANIMATOR_VELOCITY_X, vector.x);
            animator.SetFloat(Constants.ANIMATOR_VELOCITY_Y, vector.y);
            animator.SetFloat(Constants.ANIMATOR_VELOCITY_Z, vector.z);
            isSwiping = false;
        }
    }

    private void OnSliderClick(ChangeEvent<int> evt)
    {
        ShowPreview(AttackerVelocity(directionButton, powerSlider));
    }

    private void OnSliderPressed(PointerDownEvent evt)
    {
        if (!isSliding)
        {
            lastClickTime = Time.fixedTime;
            evt.target.CaptureMouse();
            isSliding = true;
        }
    }

    private void OnSliderReleased(PointerUpEvent evt)
    {
        if (isSliding)
        {
            HidePreview();
            isSliding = false;
        }
    }

    private void MoveButton()
    {
        directionButton.style.translate = Vector2.ClampMagnitude(to - from, boundRadius - buttonRadius);
    }

    private void ShowPreview(Vector3 initialVelocity)
    {
        lastClickTime = Time.fixedTime;
        float g = Physics.gravity.y;
        float v = initialVelocity.z;
        float t = (Mathf.Sqrt(v * v + g * 22) - v) / g;
        if (float.IsNaN(t))
        {
            HidePreview();
            return;
        }
        Vector3 destination = (Constants.PENALTY_SPOT + initialVelocity * t + 0.5f * t * t * Physics.gravity) / ball.GetComponent<Rigidbody>().mass;
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

    private Vector3 AttackerVelocity(Button button, SliderInt slider)
    {
        Vector3 v = new()
        {
            x = button.style.translate.value.x.value * Constants.MULTIPLIER_X,
            y = (button.style.translate.value.y.value + buttonRadius - boundRadius) * Constants.MULTIPLIER_Y,
            z = StaticValues.attacker.Power * Constants.MULTIPLIER_Z
        };
        return v * (slider.value * Constants.MULTIPLIER);
    }
}
