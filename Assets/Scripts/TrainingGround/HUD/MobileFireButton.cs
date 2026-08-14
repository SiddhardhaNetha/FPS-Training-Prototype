using UnityEngine;
using UnityEngine.EventSystems;

public class MobileFireButton : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    public bool IsPressed { get; private set; }

    private Vector2 lastPosition;
    private Vector2 lookDelta;

    private int activePointerId = -1;

    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;

        activePointerId = eventData.pointerId;

        lastPosition = eventData.position;
        lookDelta = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsPressed)
            return;

        if (eventData.pointerId != activePointerId)
            return;

        lookDelta =
            eventData.position - lastPosition;

        lastPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != activePointerId)
            return;

        IsPressed = false;

        activePointerId = -1;

        lookDelta = Vector2.zero;
    }

    public Vector2 GetLookDelta()
    {
        Vector2 delta = lookDelta;

        lookDelta = Vector2.zero;

        return delta;
    }

    private void OnDisable()
    {
        IsPressed = false;

        activePointerId = -1;

        lookDelta = Vector2.zero;
    }
}