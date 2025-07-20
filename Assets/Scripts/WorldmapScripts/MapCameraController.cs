using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class MapCameraController : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeedTouch = 0.01f;    // pinch sensitivity
    public float zoomSpeedMouse = 5f;       // scroll‐wheel sensitivity
    public float minOrthoSize = 10f;        // clamp zoom in
    public float maxOrthoSize = 200f;       // clamp zoom out

    [Header("Pan Settings")]
    public float panSpeedTouch = 1f;        // drag sensitivity
    public float panSpeedMouse = 1f;        // right‐mouse‐drag sensitivity

    private Camera cam;
    private Vector3 lastMousePos;
    private Plane mapPlane;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            Debug.LogWarning("MapCameraController: switching camera to orthographic.");
            cam.orthographic = true;
        }

        // Our pan/zoom will operate on the Z=0 plane
        mapPlane = new Plane(Vector3.forward, Vector3.zero);
    }

    void Update()
    {
        // Prevent input when over UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        HandleMousePanZoom();
        HandleTouchPanZoom();
    }

    //───────────────────────────────────────────────────────────
    // Mouse controls (Editor / Desktop)
    //───────────────────────────────────────────────────────────
    void HandleMousePanZoom()
    {
        // --- Zoom with scroll wheel ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            float newSize = cam.orthographicSize - scroll * zoomSpeedMouse;
            cam.orthographicSize = Mathf.Clamp(newSize, minOrthoSize, maxOrthoSize);
        }

        // --- Pan with right mouse button ---
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            PanCamera(delta * panSpeedMouse);
            lastMousePos = Input.mousePosition;
        }
    }

    //───────────────────────────────────────────────────────────
    // Touch controls (Mobile)
    //───────────────────────────────────────────────────────────
    void HandleTouchPanZoom()
    {
        int count = Input.touchCount;
        if (count == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                // convert deltaPosition from screen‐space to world‐space pan
                PanCamera(-t.deltaPosition * panSpeedTouch);
            }
        }
        else if (count == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            // Prior positions of each touch
            Vector2 prev0 = t0.position - t0.deltaPosition;
            Vector2 prev1 = t1.position - t1.deltaPosition;

            float prevDist = Vector2.Distance(prev0, prev1);
            float currDist = Vector2.Distance(t0.position, t1.position);
            float delta = currDist - prevDist;

            // Zoom camera
            float newSize = cam.orthographicSize - delta * zoomSpeedTouch;
            cam.orthographicSize = Mathf.Clamp(newSize, minOrthoSize, maxOrthoSize);

            // Optional: keep pinch center fixed under fingers
            Vector2 midScreen = (t0.position + t1.position) * 0.5f;
            KeepScreenPointLocked(midScreen);
        }
    }

    //───────────────────────────────────────────────────────────
    // Helper: pans camera by a screen‐space delta
    //───────────────────────────────────────────────────────────
    void PanCamera(Vector2 screenDelta)
    {
        // Cast a ray through the camera to the map plane at two screen points
        Ray r1 = cam.ScreenPointToRay(Vector2.zero);
        Ray r2 = cam.ScreenPointToRay(screenDelta);

        // Actually, we want to know how far moving by screenDelta moves us in world:
        // map both (0,0) and (screenDelta) into world at z=0, then subtract.
        if (mapPlane.Raycast(r1, out float enter1) && mapPlane.Raycast(r2, out float enter2))
        {
            Vector3 world1 = r1.GetPoint(enter1);
            Vector3 world2 = r2.GetPoint(enter2);
            Vector3 move = world1 - world2;    // note inversion: dragging finger right moves world left
            cam.transform.position += move;
        }
    }

    //───────────────────────────────────────────────────────────
    // Helper: after zooming, keep a given screen point fixed in world‐space
    //───────────────────────────────────────────────────────────
    void KeepScreenPointLocked(Vector2 screenPoint)
    {
        // Raycast before zoom
        Ray rayBefore = cam.ScreenPointToRay(screenPoint);
        if (!mapPlane.Raycast(rayBefore, out float enterBefore)) return;
        Vector3 worldBefore = rayBefore.GetPoint(enterBefore);

        // After changing orthographicSize, ray through same screen point
        Ray rayAfter = cam.ScreenPointToRay(screenPoint);
        if (!mapPlane.Raycast(rayAfter, out float enterAfter)) return;
        Vector3 worldAfter = rayAfter.GetPoint(enterAfter);

        // Move camera so that worldAfter == worldBefore
        Vector3 correction = worldBefore - worldAfter;
        cam.transform.position += correction;
    }
}
