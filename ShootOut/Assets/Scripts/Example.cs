using UnityEngine;
using UnityEngine.InputSystem;

public class Example : MonoBehaviour
{
    readonly int minScale = 1, maxScale = 40;

    public bool printToConsole = false;
    InputAction moveAction, scrollAction;
    int scale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions["Move"];
        scrollAction = InputSystem.actions["ScrollWheel"];
        if (printToConsole) print("GAME START");
        scale = (int) (transform.localScale[0] * 10F);
    }

    // Update is called once per frame
    void Update()
    {
        var moveValue = moveAction.ReadValue<Vector2>();
        if (moveValue != null)
        {
            var x = moveValue.x;
            var y = moveValue.y;

            transform.Translate(new Vector3(x, y, 0) * Time.deltaTime);
            if (printToConsole)
            {
                if (x > 0) print("RIGHT");
                else if (x < 0) print("LEFT");
                if (y > 0) print("UP");
                else if (y < 0) print("DOWN");
            }
        }
        var scrollValue = scrollAction.ReadValue<Vector2>();
        if (scrollValue != null && scrollValue != Vector2.zero)
        {
            int newScale = scale + (int) scrollValue.y;
            if (newScale >= minScale && newScale <= maxScale)
            {
                scale = newScale;
                UpdateScale();
                if (printToConsole)
                {
                    if (scrollValue.y > 0) print("SCALE UP");
                    else if (scrollValue.y < 0) print("SCALE DOWN");
                }
            }
        }
    }

    private void UpdateScale()
    {
        float s = scale / 10F;
        transform.localScale = new Vector3(s, s, s);
    }
}
