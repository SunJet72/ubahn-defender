using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapCameraController : MonoBehaviour
{
    [Header("Pan Settings")]
    [Tooltip("How fast the camera follows your one‑finger drag.")]
    public float panSpeed = 1f;

    [Header("Pinch‑Zoom Settings")]
    [Tooltip("How sensitive the pinch gesture is (lower = slower zoom).")]
    public float pinchSmooth = 1.0f;
    [Tooltip("Clamp the smallest orthographic size.")]
    public float minOrthographicSize = 5f;
    [Tooltip("Clamp the largest orthographic size.")]
    public float maxOrthographicSize = 200f;

    private Camera cam;
    private Plane  mapPlane;
    private Vector3 lastPanWorldPos;
    private float   lastPinchDistance = 0f;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
            Debug.LogWarning("MapCameraController works best with an Orthographic camera.");
        mapPlane = new Plane(Vector3.forward, Vector3.zero);
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            TouchPan(Input.GetTouch(0));
            lastPinchDistance = 0f; // Reset pinch distance when not pinching
        }
        else if (Input.touchCount == 2)
        {
            TouchPinch(Input.GetTouch(0), Input.GetTouch(1));
        }
        else
        {
            lastPinchDistance = 0f;
        }
    }

    void TouchPan(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            lastPanWorldPos = ScreenToMap(touch.position);
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector3 currentWorld = ScreenToMap(touch.position);
            Vector3 delta = lastPanWorldPos - currentWorld;
            cam.transform.position += delta * panSpeed;
            lastPanWorldPos = ScreenToMap(touch.position);
        }
    }

    void TouchPinch(Touch t0, Touch t1)
    {
        Vector2 p0 = t0.position;
        Vector2 p1 = t1.position;
        float currDist = Vector2.Distance(p0, p1);

        // If new pinch, reset distance
        if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began || lastPinchDistance <= 0.01f)
        {
            lastPinchDistance = currDist;
            return;
        }

        // Use multiplicative zoom for smooth feel
        float ratio = Mathf.Pow(currDist / lastPinchDistance, pinchSmooth);
        lastPinchDistance = currDist;

        // Midpoint (for zoom center)
        Vector2 mid = (p0 + p1) * 0.5f;
        Vector3 worldBefore = ScreenToMap(mid);

        // Multiplicative zoom (like Google Maps)
        float newOrtho = Mathf.Clamp(cam.orthographicSize / ratio, minOrthographicSize, maxOrthographicSize);
        cam.orthographicSize = newOrtho;

        // Keep midpoint under your fingers
        Vector3 worldAfter = ScreenToMap(mid);
        cam.transform.position += (worldBefore - worldAfter);
    }

    /// <summary>
    /// Projects a screen-space point onto the Z=0 plane in world space.
    /// </summary>
    Vector3 ScreenToMap(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        if (mapPlane.Raycast(ray, out float enter))
            return ray.GetPoint(enter);
        return new Vector3(cam.transform.position.x, cam.transform.position.y, 0f);
    }
}
