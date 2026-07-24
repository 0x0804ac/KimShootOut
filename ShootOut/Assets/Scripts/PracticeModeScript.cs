using UnityEngine;
using UnityEngine.UIElements;

public class PracticeModeScript : MonoBehaviour
{
    const string CPU_PANEL = "cpu-control-panel";
    const string BUTTON = "toggle-visibility";
    const string CPU_SLIDER = "cpu-power-slider";
    const string PLAYER_SLIDER = "player-power-slider";

    [SerializeField] private UIDocument document;
    [SerializeField] private GameObject attacker;
    [SerializeField] private GameObject defender;
    [SerializeField] private GameObject ball;

    private Practice game;
    private VisualElement root;
    private KickerAnimations attackerAnimation;
    private GoalkeeperAnimations defenderAnimation;

    void Start()
    {
        attackerAnimation = attacker.GetComponent<KickerAnimations>();
        defenderAnimation = defender.GetComponent<GoalkeeperAnimations>();
        root = document.rootVisualElement;
        PracticeType type = Settings.practiceType;
        bool controllable = Settings.controllableCPU;
        game = new Practice(type, controllable, this);
        if (type == PracticeType.ATTACK) root.Q<TemplateContainer>(CPU_SLIDER).visible = false;
        else if (type == PracticeType.DEFENSE) root.Q<TemplateContainer>(PLAYER_SLIDER).visible = false;
        if (!controllable)
        {
            Button btn = root.Q<Button>(BUTTON);
            btn.visible = false;
            btn.SetEnabled(false);
            root.Q<VisualElement>(CPU_PANEL).SetEnabled(false);
        }
        game.Load();
    }

    void OnEnable()
    {
        Button btn = root.Q<Button>(BUTTON);
        if (btn.visible) btn.clicked += OnToggleButtonClick;
    }

    void OnDisable()
    {
        Button btn = root.Q<Button>(BUTTON);
        if (btn.visible) btn.clicked -= OnToggleButtonClick;
    }

    private void OnToggleButtonClick()
    {
        root.Q<DropdownField>(CPU_PANEL).visible = !root.Q<DropdownField>(CPU_PANEL).visible;
    }

    public void ResetObjects()
    {
        Rigidbody r = attacker.GetComponent<Rigidbody>();
        r.linearVelocity = Vector3.zero;
        r.angularVelocity = Vector3.zero;
        attacker.transform.position = Constants.PENALTY_SPOT + (game.Attacker.IsLeftFooted ? Constants.KICKER_OFFSET_RIGHT : Constants.KICKER_OFFSET_LEFT);
        attackerAnimation.PlayIdleAnimation();
        r = defender.GetComponent<Rigidbody>();
        r.linearVelocity = Vector3.zero;
        r.angularVelocity = Vector3.zero;
        defender.transform.position = Constants.GOAL_LINE;
        defenderAnimation.PlayIdleAnimation();
        r = ball.GetComponent<Rigidbody>();
        r.linearVelocity = Vector3.zero;
        r.angularVelocity = Vector3.zero;
        ball.transform.position = Constants.PENALTY_SPOT;
    }
}
