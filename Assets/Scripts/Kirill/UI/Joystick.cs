using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform background; // Joystick background
    public RectTransform handle;     // Joystick knob

    [Range(0, 2f)]
    public float handleLimit = 1f;   // How far the handle can move (in radius)

    private Vector2 inputVector;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Convert touch position to local coordinates
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            Debug.Log(background.sizeDelta);
            localPoint /= background.sizeDelta / 2f; // Normalize to [-1, 1]

            inputVector = localPoint.magnitude > 1.0f ? localPoint.normalized : localPoint;

            // Move handle
            handle.anchoredPosition = inputVector * (background.sizeDelta.x / 2f) * handleLimit;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    // Public getters for input
    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;
    public Vector2 Direction => new Vector2(Horizontal, Vertical);
}
