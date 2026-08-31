using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PracticeModeScript : MonoBehaviour
{
    [SerializeField] private ScriptManager manager;
    [SerializeField] private AttackScript attackScript;
    [SerializeField] private DefenseScript defenseScript;
    [SerializeField] private PracticeResultScript result;
    [SerializeField] private UIDocument document;
    [SerializeField] private GameObject attacker, defender, ball, goal, preview;

    private Practice game;
    private Animator kickerAnimator, goalkeeperAnimator;

    private VisualElement root, mainPanel, buttonPanel, cpuPanel, playerPanel;
    private Button toggleButton, confirmButton, quitButton;
    private float lastMoveTime;

    public ScriptManager Manager => manager;
    public Practice Game => game;

    void Awake()
    {
        PracticeType type = Settings.practiceType;
        bool controllable = Settings.controllableCPU;
        game = new Practice(type, controllable, this);
        root = document.rootVisualElement;
        kickerAnimator = attacker.GetComponent<Animator>();
        goalkeeperAnimator = defender.GetComponent<Animator>();
        mainPanel = root.Q<VisualElement>(Constants.MAIN_PANEL);
        buttonPanel = mainPanel.Q<VisualElement>(Constants.BUTTON_PANEL);
        cpuPanel = mainPanel.Q<VisualElement>(Constants.CPU_PANEL).Q<VisualElement>(Constants.PRACTICE_MODE_CPU_PANEL);
        playerPanel = mainPanel.Q<VisualElement>(Constants.PLAYER_PANEL).Q<VisualElement>(Constants.PRACTICE_MODE_PLAYER_PANEL);
        toggleButton = buttonPanel.Q<Button>(Constants.PRACTICE_MODE_TOGGLE_BUTTON);
        confirmButton = buttonPanel.Q<Button>(Constants.CONFIRM_BUTTON);
        quitButton = buttonPanel.Q<Button>(Constants.QUIT_BUTTON);
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
        if (game.Type == PracticeType.ATTACK)
        {
            playerPanel.Add(attackScript.mainPanel);
            cpuPanel.Add(defenseScript.mainPanel);
        }
        else if (game.Type == PracticeType.DEFENSE)
        {
            playerPanel.Add(defenseScript.mainPanel);
            cpuPanel.Add(attackScript.mainPanel);
        }
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

    void FixedUpdate()
    {
        float currentTime = Time.fixedTime;
        if (!game.IsReady)
        {
            if (ball.transform.position.y < -3f) game.Turn();
            else if (ball.GetComponent<Rigidbody>().GetPointVelocity(ball.transform.position).magnitude > 0f)
            {
                lastMoveTime = currentTime;
            }
            else if (currentTime - lastMoveTime > 3f) game.Turn();
        }
    }

    private void RegisterEvents()
    {
        confirmButton.clicked += OnConfirmButtonClick;
        quitButton.clicked += OnQuitButtonClick;
        if (Settings.controllableCPU)
        {
            toggleButton.clicked += OnToggleButtonClick;
        }
    }

    private void UnregisterEvents()
    {
        confirmButton.clicked -= OnConfirmButtonClick;
        quitButton.clicked -= OnQuitButtonClick;
        if (Settings.controllableCPU)
        {
            toggleButton.clicked -= OnToggleButtonClick;
        }
    }

    private void OnToggleButtonClick()
    {
        bool newValue = !cpuPanel.visible;
        cpuPanel.SetEnabled(newValue);
        cpuPanel.visible = newValue;
    }

    private void OnConfirmButtonClick()
    {
        if (game.IsReady)
        {
            HidePreview();
            kickerAnimator.SetBool(Constants.ANIMATOR_TRIGGER_SHOOT, true);
            lastMoveTime = Time.fixedTime;
            game.IsReady = false;
        }
    }

    private void OnQuitButtonClick()
    {
        if (game.IsReady)
        {
            if (game.Attempts > 0) ShowResults();
            else SceneManager.LoadScene(Constants.SCENE_MAIN_MENU);
        }
    }

    public void ResetObjects()
    {
        attacker.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        attacker.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        attacker.transform.position = Constants.PENALTY_SPOT + (StaticValues.attacker.IsLeftFooted ? Constants.KICKER_OFFSET_RIGHT : Constants.KICKER_OFFSET_LEFT);
        kickerAnimator.ResetTrigger(Constants.ANIMATOR_TRIGGER_SHOOT);
        defender.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        defender.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        defender.transform.SetLocalPositionAndRotation(Constants.GOAL_LINE, Quaternion.LookRotation(Vector3.back));
        goalkeeperAnimator.SetBool(Constants.ANIMATOR_TRIGGER_GOALKEEP, false);
        ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.position = Constants.PENALTY_SPOT;
        PlayIdleAnimation();
    }

    public void PlayIdleAnimation()
    {
        kickerAnimator.Play(Constants.ANIMATOR_KICKER_IDLE);
        goalkeeperAnimator.SetTrigger(Constants.ANIMATOR_TRIGGER_IDLE);
    }

    public void ShowResults()
    {
        document.gameObject.SetActive(false);
        result.Show();
    }

    public void ResetControls()
    {
        bool controllable = Settings.controllableCPU;
        switch (game.Type)
        {
            case PracticeType.ATTACK:
                attackScript.ResetControls();
                if (!controllable) defenseScript.RandomizeControls();
                break;
            case PracticeType.DEFENSE:
                defenseScript.ResetControls();
                if (!controllable) attackScript.RandomizeControls();
                break;
        }
    }

    private void HidePreview()
    {
        preview.SetActive(false);
    }
}
