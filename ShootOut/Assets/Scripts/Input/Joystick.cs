using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    public Canvas canvas;
    public Image outerCircle;
    public Button innerCircle;

    private RectTransform buttonRect;
    private bool isPressed = false;

    private Vector2 startPos, endPos;
    private float boundRadius, buttonRadius;

    void OnEnable()
    {
        buttonRect = innerCircle.GetComponent<RectTransform>();
        boundRadius = outerCircle.rectTransform.rect.width * 0.5f;
        buttonRadius = buttonRect.rect.width * 0.5f;
        print($"{boundRadius} - {buttonRadius}");
    }

    void OnDisable()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPressed && eventData.pointerPress == innerCircle.gameObject)
        {
            isPressed = true;
            startPos = eventData.pressPosition;
            print("started pressing");
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (isPressed)
        {
            endPos = eventData.position;
            MoveButton();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPressed)
        {
            endPos = eventData.position;
            MoveButton();
            isPressed = false;
        }
    }

    private void MoveButton()
    {
        buttonRect.localPosition = Vector2.ClampMagnitude(endPos - startPos, boundRadius - buttonRadius);
    }
}
