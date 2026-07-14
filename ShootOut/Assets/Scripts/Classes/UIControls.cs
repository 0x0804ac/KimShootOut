using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIControls : MonoBehaviour
{
    protected const string LEFT = "offScreenLeft";
    protected const string RIGHT = "offScreenRight";

    protected UIDocument document;
    protected VisualElement root;
    protected bool isHidden;

    void Awake()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        Init();
    }

    void OnEnable()
    {
        RegisterEvents();
        root.RegisterCallback<TransitionEndEvent>(OnTransitionEnd);
    }

    void OnDisable()
    {
        UnregisterEvents();
        root.UnregisterCallback<TransitionEndEvent>(OnTransitionEnd);
    }

    protected void RegisterClickEvent(string buttonName, System.Action action)
    {
        Button button = root.Q<Button>(buttonName);
        if (button != null) button.clicked += action;
    }

    protected void UnregisterClickEvent(string buttonName, System.Action action)
    {
        Button button = root.Q<Button>(buttonName);
        if (button != null) button.clicked -= action;
    }

    protected abstract void Init();

    protected abstract void RegisterEvents();

    protected abstract void UnregisterEvents();

    private void OnTransitionEnd(TransitionEndEvent evt)
    {
        if (!isHidden) RegisterEvents();
    }

    public void MoveToLeft()
    {
        if (isHidden) return;
        UnregisterEvents();
        isHidden = true;
        root.EnableInClassList(LEFT, true);
    }

    public void MoveToCenter()
    {
        if (!isHidden) return;
        isHidden = false;
        if (root.ClassListContains(LEFT)) root.EnableInClassList(LEFT, false);
        else if (root.ClassListContains(RIGHT)) root.EnableInClassList(RIGHT, false);
    }

    public void MoveToRight()
    {
        if (isHidden) return;
        UnregisterEvents();
        isHidden = true;
        root.EnableInClassList(RIGHT, true);
    }
}
