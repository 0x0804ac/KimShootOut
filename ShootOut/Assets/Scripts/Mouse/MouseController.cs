using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MouseController
{
    [SerializeField] private Camera gameCamera;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private RectTransform aimImage;
    [SerializeField] private BoxCollider goalAimArea;

    private Vector2 lastHitPoint = Vector2.zero;

    // Update is called once per frame
    public void Update()
    {
        bool hasValidAim = false;

        if (Mouse.current == null)
        {
            hasValidAim = false;
            return;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = gameCamera.ScreenPointToRay(mousePosition);

        hasValidAim = goalAimArea.Raycast(
            ray,
            out RaycastHit hit,
            1000f
            );

        if (hasValidAim)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mousePosition,
            null,
            out Vector2 localPosition))
            {
                aimImage.anchoredPosition = localPosition;
                lastHitPoint = localPosition;
            }
        }

        World2ScreenProjection();
    }

    public Vector2 GetLastHitPoint()
    {
        return lastHitPoint; 
    }

    private Vector2 World2ScreenProjection()
    {


        return Vector2.zero;
    }
}
