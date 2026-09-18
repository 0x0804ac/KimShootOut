using UnityEngine;
using UnityEngine.UIElements;

public class DialogButton : MonoBehaviour
{
    [SerializeField] private TutorialModeScript script;
    [SerializeField] private UIDocument document;

    private VisualElement panel;
    private Button button;

    public Button Btn => button;

    void OnEnable()
    {
        VisualElement mainPanel = document.rootVisualElement.Q<VisualElement>(Constants.MAIN_PANEL);
        panel = mainPanel.Q<VisualElement>(Constants.CONTROL_PANEL).Q<VisualElement>(Constants.PLAYER_CONTROL_PANEL);
        button = mainPanel.Q<VisualElement>(Constants.TOP_PANEL).Q<Button>(Constants.CONFIRM_BUTTON);
        button.clicked += OnButtonClick;
    }

    void OnDisable()
    {
        button.clicked -= OnButtonClick;
    }

    private void OnButtonClick()
    {
        switch (script.Game.Phase)
        {
            case TutorialType.ATTACK:
                script.ShowAttackUI();
                break;
            case TutorialType.DEFENSE:
                script.ShowDefenseUI();
                break;
            case TutorialType.ITEM:
                //script.ShowItemUI();
                break;
            default:
                return;
        }
        document.rootVisualElement.visible = false;
    }

    public void SetEnabled(bool flag)
    {
        document.gameObject.SetActive(flag);
    }
}
