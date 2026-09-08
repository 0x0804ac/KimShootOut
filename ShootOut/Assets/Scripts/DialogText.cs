using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogText : MonoBehaviour
{
    [SerializeField] private UIDocument document;

    public string title;
    [Multiline] public string text;
    [Min(0.01f)] public float delay;

    private VisualElement textPanel;
    private Label titleLabel, textLabel;
    private bool isTyping;

    void OnEnable()
    {
        VisualElement panel = document.rootVisualElement.Q<VisualElement>(Constants.MAIN_PANEL);
        titleLabel = panel.Q<VisualElement>("top-panel").Q<VisualElement>("title-panel").Q<Label>(Constants.TITLE_LABEL);
        textPanel = panel.Q<VisualElement>("dialog-panel");
        textLabel = textPanel.Q<Label>("text-label");
        isTyping = false;
        textPanel.RegisterCallback<ClickEvent>(SkipAnimation, TrickleDown.TrickleDown);
    }

    void OnDisable()
    {
        textPanel.UnregisterCallback<ClickEvent>(SkipAnimation, TrickleDown.TrickleDown);
    }

    void Start()
    {
        if (string.IsNullOrEmpty(title)) title = "튜토리얼";
        if (string.IsNullOrEmpty(text)) text = @"테스트1
테스트2
테스트3";
        StartCoroutine(FillText());
    }

    IEnumerator FillText()
    {
        titleLabel.text = title;
        textLabel.text = string.Empty;
        isTyping = true;
        foreach (char letter in text)
        {
            textLabel.text += letter;
            yield return new WaitForSeconds(delay);
        }
        isTyping = false;
    }

    private void SkipAnimation(ClickEvent evt)
    {
        if (isTyping)
        {
            StopAllCoroutines();
            textLabel.text = text;
            isTyping = false;
        }
    }
}
