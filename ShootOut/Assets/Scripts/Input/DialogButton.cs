using UnityEngine;
using UnityEngine.UIElements;

public class DialogButton : MonoBehaviour
{
    [SerializeField] private UIDocument document;

    private Button button;

    public Button Btn => button;

    void OnEnable()
    {
        button = document.rootVisualElement.Q<VisualElement>(Constants.MAIN_PANEL).Q<VisualElement>(Constants.TOP_PANEL).Q<Button>();
        button.clicked += OnButtonClick;
    }

    void OnDisable()
    {
        button.clicked -= OnButtonClick;
    }

    private void OnButtonClick()
    {
        print(button.name);
    }
}
