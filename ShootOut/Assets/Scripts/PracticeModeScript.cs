using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PracticeModeScript : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private GameObject attacker, defender, ball, preview;

    private Practice game;
    private InputActions actions;
    private Animator kickerAnimator, goalkeeperAnimator;

    private VisualElement root, cpuPanel, playerButtonPanel, cpuButtonPanel;
    private Button playerButton, cpuButton, toggleButton, pressedButton;
    private SliderInt playerSlider, cpuSlider;

    private Vector2 from, to;
    private float buttonRadius, boundRadius, lastMoveTime;

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
        game.Start();
    }

    void OnEnable()
    {
        RegisterEvents();
    }

    void OnDisable()
    {
        UnregisterEvents();
    }

    void Update()
    {
        if (!game.IsReady)
        {
            if (ball.GetComponent<Rigidbody>().GetPointVelocity(ball.transform.position).magnitude > 0f)
            {
                lastMoveTime = Time.unscaledTime;
            }
            else if (Time.unscaledTime - lastMoveTime > 3f || ball.transform.position.y < -3f)
            {
                game.Turn();
            }
        }
    }

    private void OngoingSwipe(InputAction.CallbackContext context)
    {
        if (pressedButton != null)
        {
            Vector2 pos = context.ReadValue<Vector2>();
            to.x = pos.x;
            to.y = Screen.height - pos.y;
            MoveButton();
            if (pressedButton == playerButton && Settings.practiceType == PracticeType.ATTACK)
            {
                Vector3 vector = AttackerVelocity(playerButton, playerSlider);
                ShowPreview(vector);
            }
            else if (pressedButton == cpuButton && Settings.practiceType == PracticeType.DEFENSE)
            {
                Vector3 vector = AttackerVelocity(cpuButton, cpuSlider);
                ShowPreview(vector);
            }
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
        if (pressedButton != null)
        {
            to = evt.position;
            MoveButton();
            HidePreview();
            if (game.IsReady) {
                if (!Settings.controllableCPU)
                {
                    cpuButton.style.translate = new Vector2(RandomValue(), RandomValue());
                    cpuSlider.value = Random.Range(cpuSlider.lowValue, cpuSlider.highValue + 1);
                }
                if (pressedButton == playerButton)
                {
                    Vector3 vector;
                    if (Settings.practiceType == PracticeType.ATTACK)
                    {
                        vector = StaticValues.attacker.Kick(AttackerVelocity(playerButton, playerSlider));
                        kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_X, vector.x);
                        kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Y, vector.y);
                        kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Z, vector.z);
                        vector = DefenderVelocity(cpuButton);
                        goalkeeperAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_X, vector.x);
                        goalkeeperAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Y, vector.y);
                        goalkeeperAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Z, vector.z);
                        kickerAnimator.SetTrigger(Constants.ANIMATOR_TRIGGER_SHOOT);
                        lastMoveTime = Time.unscaledTime;
                        game.IsReady = false;
                    }
                    else if (Settings.practiceType == PracticeType.DEFENSE)
                    {
                        vector = StaticValues.attacker.Kick(AttackerVelocity(cpuButton, cpuSlider));
                        kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_X, vector.x);
                        kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Y, vector.y);
                        kickerAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Z, vector.z);
                        vector = DefenderVelocity(playerButton);
                        goalkeeperAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_X, vector.x);
                        goalkeeperAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Y, vector.y);
                        goalkeeperAnimator.SetFloat(Constants.ANIMATOR_VELOCITY_Z, vector.z);
                        kickerAnimator.SetTrigger(Constants.ANIMATOR_TRIGGER_SHOOT);
                        lastMoveTime = Time.unscaledTime;
                        game.IsReady = false;
                    }
                }
            }
            pressedButton = null;
        }
    }

    private void OnSliderClick(ChangeEvent<int> evt)
    {
        if (Settings.practiceType == PracticeType.ATTACK)
        {
            ShowPreview(AttackerVelocity(playerButton, playerSlider));
        }
        else if (Settings.practiceType == PracticeType.DEFENSE)
        {
            ShowPreview(AttackerVelocity(cpuButton, cpuSlider));
        }
    }

    private void OnSliderReleased(PointerUpEvent evt)
    {
        HidePreview();
    }

    private void RegisterEvents()
    {
        actions.Gameplay.Swipe.performed += OngoingSwipe;
        actions.Gameplay.Enable();
        playerButton.RegisterCallback<GeometryChangedEvent>(UpdateRadius);
        playerButton.RegisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
        playerButton.RegisterCallback<PointerUpEvent>(OnButtonReleased);
        if (Settings.practiceType == PracticeType.ATTACK)
        {
            playerSlider.RegisterCallback<PointerUpEvent>(OnSliderReleased);
            playerSlider.RegisterValueChangedCallback(OnSliderClick);
        }
        if (Settings.controllableCPU)
        {
            toggleButton.clicked += OnToggleButtonClick;
            cpuButton.RegisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
            cpuButton.RegisterCallback<PointerUpEvent>(OnButtonReleased);
            if (Settings.practiceType == PracticeType.DEFENSE)
            {
                cpuSlider.RegisterCallback<PointerUpEvent>(OnSliderReleased);
                cpuSlider.RegisterValueChangedCallback(OnSliderClick);
            }
        }
    }

    private void UnregisterEvents()
    {
        actions.Gameplay.Swipe.performed -= OngoingSwipe;
        actions.Gameplay.Disable();
        playerButton.UnregisterCallback<GeometryChangedEvent>(UpdateRadius);
        playerButton.UnregisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
        playerButton.UnregisterCallback<PointerUpEvent>(OnButtonReleased);
        if (Settings.practiceType == PracticeType.ATTACK)
        {
            playerSlider.UnregisterCallback<PointerUpEvent>(OnSliderReleased);
            playerSlider.UnregisterValueChangedCallback(OnSliderClick);
        }
        if (Settings.controllableCPU)
        {
            toggleButton.clicked -= OnToggleButtonClick;
            cpuButton.UnregisterCallback<PointerDownEvent>(OnButtonPressed, TrickleDown.TrickleDown);
            cpuButton.UnregisterCallback<PointerUpEvent>(OnButtonReleased);
            if (Settings.practiceType == PracticeType.DEFENSE)
            {
                cpuSlider.UnregisterCallback<PointerUpEvent>(OnSliderReleased);
                cpuSlider.UnregisterValueChangedCallback(OnSliderClick);
            }
        }
    }

    private void MoveButton()
    {
        pressedButton.style.translate = Vector2.ClampMagnitude(to - from, boundRadius - buttonRadius);
    }

    public void ResetControls()
    {
        pressedButton = null;
        playerButton.style.translate = Vector2.zero;
    }

    private void OnToggleButtonClick()
    {
        bool newValue = !cpuPanel.visible;
        cpuPanel.SetEnabled(newValue);
        cpuPanel.visible = newValue;
    }

    public void ResetObjects()
    {
        attacker.transform.position = Constants.PENALTY_SPOT + (StaticValues.attacker.IsLeftFooted ? Constants.KICKER_OFFSET_RIGHT : Constants.KICKER_OFFSET_LEFT);
        kickerAnimator.ResetTrigger(Constants.ANIMATOR_TRIGGER_SHOOT);
        defender.transform.SetLocalPositionAndRotation(Constants.GOAL_LINE, Quaternion.LookRotation(Vector3.back));
        goalkeeperAnimator.ResetTrigger(Constants.ANIMATOR_TRIGGER_GOALKEEP);
        ball.transform.position = Constants.PENALTY_SPOT;
        PlayIdleAnimation();
    }

    public void PlayIdleAnimation()
    {
        kickerAnimator.Play(Constants.ANIMATOR_KICKER_IDLE);
        goalkeeperAnimator.SetTrigger(Constants.ANIMATOR_TRIGGER_IDLE);
    }

    private float RandomValue()
    {
        return (Random.value * 2 - 1) * (boundRadius - buttonRadius);
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

    private Vector3 DefenderVelocity(Button button)
    {
        return new()
        {
            x = button.style.translate.value.x.value * Constants.MULTIPLIER_X,
            y = button.style.translate.value.y.value * Constants.MULTIPLIER_Y,
            z = StaticValues.defender.Speed * -Constants.MULTIPLIER
        };
    }

    private void ShowPreview(Vector3 initialVelocity)
    {
        float g = Physics.gravity.y;
        float v = initialVelocity.z;
        float t = (Mathf.Sqrt(v * v + g * 22) - v) / g;
        Vector3 destination = (Constants.PENALTY_SPOT + initialVelocity * t + 0.5f * t * t * Physics.gravity) / ball.GetComponent<Rigidbody>().mass;
        if (destination.y > 0)
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
}
