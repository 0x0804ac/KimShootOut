using UnityEngine;
using UnityEngine.UIElements;

public class BackButton : MonoBehaviour
{
    [SerializeField] private UIControls previousMenu, thisMenu;

    private UIDocument document;
    private Button button;

    void Awake()
    {
        document = GetComponent<UIDocument>();
        button = document.rootVisualElement.Q<Button>("back-button");
    }

    void OnEnable()
    {
        if (button != null) button.clicked += OnBackButtonPressed;
    }

    void OnDisable()
    {
        if (button != null) button.clicked -= OnBackButtonPressed;
    }

    private void OnBackButtonPressed()
    {
        thisMenu.MoveToRight();
        previousMenu.MoveToCenter();
    }
}
