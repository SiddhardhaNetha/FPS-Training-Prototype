using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileLookArea : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    private Vector2 lastPosition;
    private bool touching;

    private Vector2 lookDelta;

    private Graphic graphic;

    private void Awake()
    {
        graphic = GetComponent<Graphic>();
    }

    private void Start()
    {
        // On PC, this area should NOT block mouse/UI detection.
        // On mobile, it remains a normal raycast target.
        if (!Application.isMobilePlatform)
        {
            if (graphic != null)
            {
                graphic.raycastTarget = false;
            }
        }
    }

    // =========================================================
    // POINTER DOWN
    // =========================================================

    public void OnPointerDown(PointerEventData eventData)
    {
        // Mobile only
        if (!Application.isMobilePlatform)
            return;

        touching = true;

        lastPosition = eventData.position;

        lookDelta = Vector2.zero;
    }

    // =========================================================
    // DRAG
    // =========================================================

    public void OnDrag(PointerEventData eventData)
    {
        if (!Application.isMobilePlatform)
            return;

        if (!touching)
            return;

        lookDelta =
            eventData.position - lastPosition;

        lastPosition = eventData.position;
    }

    // =========================================================
    // POINTER UP
    // =========================================================

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!Application.isMobilePlatform)
            return;

        touching = false;

        lookDelta = Vector2.zero;
    }

    // =========================================================
    // GET LOOK DELTA
    // =========================================================

    public Vector2 GetLookDelta()
    {
        Vector2 delta = lookDelta;

        lookDelta = Vector2.zero;

        return delta;
    }

    // =========================================================
    // DISABLE
    // =========================================================

    private void OnDisable()
    {
        touching = false;

        lookDelta = Vector2.zero;
    }
}