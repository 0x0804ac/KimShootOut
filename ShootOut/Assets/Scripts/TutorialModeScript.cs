using UnityEngine;
using UnityEngine.SceneManagement;
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

    public ScriptManager Manager => manager;
    public Tutorial Game => game;

    void Awake()
    {
        game = new Tutorial();
        kickerAnimator = attacker.GetComponent<Animator>();
        goalkeeperAnimator = defender.GetComponent<Animator>();
        game.Load();
    }    
}
