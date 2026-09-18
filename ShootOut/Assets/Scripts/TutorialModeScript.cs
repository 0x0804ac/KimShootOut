using UnityEngine;
using UnityEngine.UIElements;

public class TutorialModeScript : MonoBehaviour
{
    [SerializeField] private ScriptManager manager;
    [SerializeField] private AttackScript attackScript;
    [SerializeField] private DefenseScript defenseScript;
    [SerializeField] private DialogText textScript;
    [SerializeField] private DialogButton buttonScript;
    [SerializeField] private UIDocument document;
    [SerializeField] private GameObject attacker, defender, ball, goal, preview;

    private Tutorial game;
    private Animator kickerAnimator, goalkeeperAnimator;

    private VisualElement playerPanel;

    public ScriptManager Manager => manager;
    public Tutorial Game => game;

    void Awake()
    {
        game = new Tutorial(this);
        kickerAnimator = attacker.GetComponent<Animator>();
        goalkeeperAnimator = defender.GetComponent<Animator>();
        game.Load();
    }

    void OnEnable()
    {
        playerPanel = document.rootVisualElement.Q<VisualElement>(Constants.MAIN_PANEL).Q<VisualElement>(Constants.CONTROL_PANEL).Q<VisualElement>(Constants.PLAYER_CONTROL_PANEL);
    }

    public void SetDialogTitle(string title)
    {
        textScript.Title = title;
    }

    public void SetDialogText(string text)
    {
        textScript.Text = text;
    }

    public void ShowAttackUI()
    {
        attackScript.AddPanel(playerPanel);
        document.gameObject.SetActive(true);
    }

    public void ShowDefenseUI()
    {
        defenseScript.AddPanel(playerPanel);
        document.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        playerPanel.Clear();
        document.gameObject.SetActive(false);
    }

    public void SetDialogEnabled(bool flag)
    {
        buttonScript.SetEnabled(flag);
    }
}
