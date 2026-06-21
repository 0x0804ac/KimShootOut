using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ButtonScript : MonoBehaviour
{
    private InputActions actions;
    private UIDocument document;
    private Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        document = GetComponent<UIDocument>();
        button = document.rootVisualElement.Q<Button>("DirectionButton");
        if (button != null) button.clicked += OnButtonClicked;
        actions = new InputActions();
        actions.Gameplay.Swipe.performed += OngoingSwipe;
        actions.Gameplay.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnButtonClicked()
    {
        Debug.Log("Button clicked!");
    }

    private void OngoingSwipe(InputAction.CallbackContext context)
    {
        Vector2 swipeVector = context.ReadValue<Vector2>();
    }
}
