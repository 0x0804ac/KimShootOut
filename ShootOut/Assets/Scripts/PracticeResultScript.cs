using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PracticeResultScript : MonoBehaviour
{
    public const string TITLE_TEXT = " 연습 결과";

    [SerializeField] private UIDocument document;
    [SerializeField] private PracticeModeScript script;

    private VisualElement root, mainPanel;
    private Label titleLabel;
    private Button quitButton;

    void OnEnable()
    {
        if (root == null) Init();
        quitButton.clicked += ReturnToMenu;
    }

    void OnDisable()
    {
        quitButton.clicked -= ReturnToMenu;
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(Constants.SCENE_MAIN_MENU);
    }

    public void Show()
    {
        mainPanel.Q<Label>(Constants.PRACTICE_MODE_ATTEMPTS_VALUE).text = $"{script.Game.Attempts}";
        mainPanel.Q<Label>(Constants.PRACTICE_MODE_GOALS_VALUE).text = $"{script.Game.Goals}";
        mainPanel.Q<Label>(Constants.PRACTICE_MODE_SAVES_VALUE).text = $"{script.Game.Saves}";
        root.visible = true;
    }

    private void Init()
    {
        root = document.rootVisualElement;
        mainPanel = root.Q<VisualElement>("main-panel");
        titleLabel = mainPanel.Q<Label>(Constants.TITLE_LABEL);
        quitButton = mainPanel.Q<Button>(Constants.QUIT_BUTTON);
        if (script.Game.Type == PracticeType.ATTACK)
        {
            titleLabel.text = Practice.ATTACK + TITLE_TEXT;
        }
        else if (script.Game.Type == PracticeType.DEFENSE)
        {
            titleLabel.text = Practice.DEFENSE + TITLE_TEXT;
        }
        root.visible = false;
    }
}
