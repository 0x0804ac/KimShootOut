using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MouseController
{
    [SerializeField] private Camera gameCamera;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private RectTransform aimImage;
    [SerializeField] private BoxCollider goalPostCol;

    RangeVariable<Vector2> screenRangeVar;
    Vector2 lastAimPosition = Vector2.zero;

    public void Init()
    {
        screenRangeVar = WorldToScreenProjection();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector2 aimPosition = ClampToGoalPost(mousePosition, screenRangeVar);
        lastAimPosition = aimPosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            aimPosition,
            null,
            out Vector2 localPosition))
        {
            aimImage.anchoredPosition = localPosition;
        }
    }

    private RangeVariable<Vector2> WorldToScreenProjection()
    {
        Vector3 center = goalPostCol.center;
        Vector3 halfSize = goalPostCol.size * 0.5f;

        Vector3 bottomLeft = center - halfSize;
        Vector3 topRight = center + halfSize;

        bottomLeft = goalPostCol.transform.TransformPoint(bottomLeft);
        topRight = goalPostCol.transform.TransformPoint(topRight);

        Vector2 screenMin = gameCamera.WorldToScreenPoint(bottomLeft);
        Vector2 screenMax = gameCamera.WorldToScreenPoint(topRight);

        RangeVariable<Vector2> screenRangeVar = new RangeVariable<Vector2>(screenMin, screenMax);

        return screenRangeVar;
    }

    private Vector2 ClampToGoalPost(Vector2 mousePosition, RangeVariable<Vector2> rangeVar)
    {
        return new Vector2(
            Mathf.Clamp(mousePosition.x, rangeVar.min.x, rangeVar.max.x),
            Mathf.Clamp(mousePosition.y, rangeVar.min.y, rangeVar.max.y) 
            );
    }

    public Vector2 GetLastAimPosition()
    {
        return lastAimPosition;
    }
}
