using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PracticeModeScript : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private GameObject attacker;
    [SerializeField] private GameObject defender;
    [SerializeField] private GameObject ball;

    private Practice game;
    private InputActions actions;
    private Animator kickerAnimator, goalkeeperAnimator;

    private VisualElement root, cpuPanel, playerButtonPanel, cpuButtonPanel;
    private Button playerButton, cpuButton, toggleButton, pressedButton;
    private SliderInt playerSlider, cpuSlider;

    private Vector2 from, to;
    private float buttonRadius, boundRadius;

    void Awake()
    {
        if (actions != null) return;
        actions = new InputActions();
        PracticeType type = Settings.practiceType;
        bool controllable = Settings.controllableCPU;
        game = new Practice(type, controllable, this);
        root = document.rootVisualElement;
        kickerAnimator = attacker.GetComponent<Animator>();
        goalkeeperAnimator = defender.GetComponent<Animator>();
        cpuPanel = root.Q<VisualElement>(Constants.PRACTICE_MODE_CPU_PANEL);
        playerButtonPanel = root.Q<TemplateContainer>(Constants.PRACTICE_MODE_PLAYER_BUTTON_PANEL).Q<VisualElement>(Constants.CONTROLS_BUTTON_BOUND);
        cpuButtonPanel = cpuPanel.Q<TemplateContainer>(Constants.PRACTICE_MODE_CPU_BUTTON_PANEL).Q<VisualElement>(Constants.CONTROLS_BUTTON_BOUND);
        playerButton = playerButtonPanel.Q<Button>(Constants.CONTROLS_DIRECTION_BUTTON);
        cpuButton = cpuButtonPanel.Q<Button>(Constants.CONTROLS_DIRECTION_BUTTON);
        toggleButton = root.Q<Button>(Constants.PRACTICE_MODE_TOGGLE_BUTTON);
        playerSlider = root.Q<TemplateContainer>(Constants.PRACTICE_MODE_PLAYER_SLIDER).Q<SliderInt>(Constants.CONTROLS_POWER_SLIDER);
        cpuSlider = root.Q<TemplateContainer>(Constants.PRACTICE_MODE_CPU_SLIDER).Q<SliderInt>(Constants.CONTROLS_POWER_SLIDER);
        if (type == PracticeType.ATTACK) cpuSlider.parent.visible = false;
        else if (type == PracticeType.DEFENSE) playerSlider.parent.visible = false;
        if (!controllable)
        {
            toggleButton.visible = false;
            toggleButton.SetEnabled(false);
            cpuPanel.SetEnabled(false);
        }
        game.Load();
    }

    void Start()
    {
        pressedButton = null;
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
        if (pressedButton != null)
        {
            Vector2 pos = context.ReadValue<Vector2>();
            to.x = pos.x;
            to.y = Screen.height - pos.y;
            MoveButton();
        }
    }

    private void UpdateRadius(GeometryChangedEvent evt)
    {
        boundRadius = playerButtonPanel.layout.width / 2.0f;
        buttonRadius = playerButton.layout.width / 2.0f;
        from = playerButton.layout.center;
        to = playerButton.layout.center;
    }

    private void OnButtonPressed(PointerDownEvent evt)
    {
        if (pressedButton == null)
        {
            if (playerButton == evt.target) pressedButton = playerButton;
            else if (cpuButton == evt.target) pressedButton = cpuButton;
            else return;
            evt.target.CaptureMouse();
            from = evt.position;
        }
    }

    private void OnButtonReleased(PointerUpEvent evt)
    {
        Vector3 vector;
        float x, y, z;
        if (pressedButton != null)
        {
            to = evt.position;
            MoveButton();
            if (pressedButton == playerButton)
            {
                if (Settings.practiceType == PracticeType.ATTACK)
                {
                    x = pressedButton.style.translate.value.x.value;
                    y = pressedButton.style.translate.value.y.value;
                    z = game.Attacker.Power;
                    vector = game.Attacker.Kick(new Vector3(x, y, z) * (playerSlider.value * 0.01f));
                    kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_X, vector.x);
                    kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Y, vector.y);
                    kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Z, vector.z);
                    kickerAnimator.SetTrigger(Constants.ANIMATOR_TRIGGER_SHOOT);
                }
                else if (Settings.practiceType == PracticeType.DEFENSE)
                {
                    x = cpuButton.style.translate.value.x.value;
                    y = cpuButton.style.translate.value.y.value;
                    z = game.Attacker.Power;
                    vector = game.Attacker.Kick(new Vector3(x, y, z) * (cpuSlider.value * 0.01f));
                    kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_X, vector.x);
                    kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Y, vector.y);
                    kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Z, vector.z);
                    kickerAnimator.SetTrigger(Constants.ANIMATOR_TRIGGER_SHOOT);
                }
            }
            pressedButton = null;
        }
    }

    private void RegisterEvents()
    {
        actions.Gameplay.Swipe.performed += OngoingSwipe;
        actions.Gameplay.Enable();
        if (toggleButton.visible) toggleButton.clicked += OnToggleButtonClick;
        playerButton.RegisterCallback<GeometryChangedEvent>(UpdateRadius);
        playerButton.RegisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
        playerButton.RegisterCallback<PointerUpEvent>(OnButtonReleased);
        if (Settings.controllableCPU)
        {
            cpuButton.RegisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
            cpuButton.RegisterCallback<PointerUpEvent>(OnButtonReleased);
        }
    }

    private void UnregisterEvents()
    {
        actions.Gameplay.Swipe.performed -= OngoingSwipe;
        actions.Gameplay.Disable();
        if (toggleButton.visible) toggleButton.clicked -= OnToggleButtonClick;
        playerButton.UnregisterCallback<GeometryChangedEvent>(UpdateRadius);
        playerButton.UnregisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
        playerButton.UnregisterCallback<PointerUpEvent>(OnButtonReleased);
        if (Settings.controllableCPU)
        {
            cpuButton.UnregisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
            cpuButton.UnregisterCallback<PointerUpEvent>(OnButtonReleased);
        }
    }

    private void MoveButton()
    {
        pressedButton.style.translate = Vector2.ClampMagnitude(to - from, boundRadius - buttonRadius);
    }

    public void ResetControls()
    {
        Button btn = pressedButton;
        pressedButton = null;
        btn.style.translate = Vector2.zero;
    }

    private void OnToggleButtonClick()
    {
        bool newValue = !cpuPanel.visible;
        cpuPanel.SetEnabled(newValue);
        cpuPanel.visible = newValue;
    }

    public void ResetObjects()
    {
        Rigidbody r = attacker.GetComponent<Rigidbody>();
        r.linearVelocity = Vector3.zero;
        r.angularVelocity = Vector3.zero;
        attacker.transform.position = Constants.PENALTY_SPOT + (game.Attacker.IsLeftFooted ? Constants.KICKER_OFFSET_RIGHT : Constants.KICKER_OFFSET_LEFT);
        attacker.GetComponent<Animator>().Play(Constants.ANIMATOR_KICKER_IDLE);
        r = defender.GetComponent<Rigidbody>();
        r.linearVelocity = Vector3.zero;
        r.angularVelocity = Vector3.zero;
        defender.transform.position = Constants.GOAL_LINE;
        //defenderAnimation.PlayIdleAnimation();
        r = ball.GetComponent<Rigidbody>();
        r.linearVelocity = Vector3.zero;
        r.angularVelocity = Vector3.zero;
        ball.transform.position = Constants.PENALTY_SPOT;
    }
}
