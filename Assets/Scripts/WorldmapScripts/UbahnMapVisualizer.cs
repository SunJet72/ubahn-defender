using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UbahnMapVisualizer : MonoBehaviour
{
    [Header("Prefabs & Parents (World‐Space)")]
    public GameObject stationPrefab;
    public Transform stationsParent;
    public GameObject linePrefab;
    public Transform linesParent;

    [Header("Map Size (World Units)")]
    public float mapWidth = 10000f;
    public float mapHeight = 10000f;

    [Header("Zoom Settings")]
    public float minZoom = 0.5f;
    public float maxZoom = 2f;
    public float pinchZoomSpeed = 0.005f;

    private Camera cam;
    private float lastPinchDistance;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null) Debug.LogError("UbahnMapVisualizer: No camera tagged MainCamera in scene!");
    }

    void Start()
    {
        BuildMap();
    }

    void BuildMap()
    {
        var map = WorldMapController.instance.map;
        if (map == null || map.Count == 0)
        {
            Debug.LogError("UbahnMapVisualizer: no stations in WorldMapController.instance.map");
            return;
        }

        // Compute lon/lat bounds & center
        var allLons = map.Select(s => s.lon);
        var allLats = map.Select(s => s.lat);
        double minLon = allLons.Min(), maxLon = allLons.Max();
        double minLat = allLats.Min(), maxLat = allLats.Max();
        double centerLon = (minLon + maxLon) / 2.0;
        double centerLat = (minLat + maxLat) / 2.0;

        // Instantiate stations
        var stationNodes = new Dictionary<Station, GameObject>();
        foreach (var station in map)
        {
            var go = Instantiate(stationPrefab, stationsParent);
            go.name = station.StationName;

            float dx = (float)(station.lon - centerLon);
            float dy = (float)(station.lat - centerLat);
            float x = dx / (float)(maxLon - minLon) * mapWidth;
            float y = dy / (float)(maxLat - minLat) * mapHeight;

            go.transform.localPosition = new Vector3(x, y, 0f);
            stationNodes[station] = go;
        }

        // Instantiate lines
        var drawn = new HashSet<(Station, Station)>();
        foreach (var s in map)
        {
            foreach (var nb in s.Neighbours)
            {
                var pair = s.GetHashCode() < nb.GetHashCode() ? (s, nb) : (nb, s);
                if (drawn.Contains(pair)) continue;
                drawn.Add(pair);
                if (!s.uBahnLines.Intersect(nb.uBahnLines).Any()) continue;

                var lineGO = Instantiate(linePrefab, linesParent);
                lineGO.name = $"Line_{pair.Item1.StationName}_{pair.Item2.StationName}";
                var lr = lineGO.GetComponent<LineRenderer>();
                if (!lr)
                {
                    Debug.LogError("LinePrefab missing LineRenderer!", lineGO);
                    continue;
                }
                lr.positionCount = 2;
                lr.SetPosition(0, stationNodes[pair.Item1].transform.position);
                lr.SetPosition(1, stationNodes[pair.Item2].transform.position);
            }
        }

        Debug.Log($"Map built: {stationNodes.Count} stations, {drawn.Count} edges");
    }

    void Update()
    {
        // Only handle touches if not over UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(0))
            return;

        if (Input.touchCount == 1)
            PanTouch(Input.GetTouch(0));
        else if (Input.touchCount == 2)
            PinchZoom(Input.GetTouch(0), Input.GetTouch(1));
    }

   private Plane mapPlane = new Plane(Vector3.forward, Vector3.zero);

private void PanTouch(Touch touch)
{
    if (touch.phase != TouchPhase.Moved) return;

    // Cast a ray from the previous touch position onto the Z=0 plane
    var prevRay = cam.ScreenPointToRay(touch.position - touch.deltaPosition);
    var curRay  = cam.ScreenPointToRay(touch.position);

    if (mapPlane.Raycast(prevRay, out float enterPrev) &&
        mapPlane.Raycast(curRay,  out float enterCur))
    {
        Vector3 prevWorld = prevRay.GetPoint(enterPrev);
        Vector3 curWorld  =  curRay.GetPoint(enterCur);

        // Move MapRoot opposite to finger movement
        Vector3 delta = curWorld - prevWorld;
        transform.position -= delta;
    }
}

private void PinchZoom(Touch t0, Touch t1)
{
    // Get current distance between touches
    float curDist = Vector2.Distance(t0.position, t1.position);

    if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
    {
        lastPinchDistance = curDist;
        return;
    }

    float delta = curDist - lastPinchDistance;
    lastPinchDistance = curDist;

    // We’ll zoom the MapRoot by scaling it, clamped
    float newScale = Mathf.Clamp(transform.localScale.x + delta * pinchZoomSpeed, minZoom, maxZoom);

    // To keep the pivot under the fingers stable, find the world point under their midpoint
    Vector2 mid = (t0.position + t1.position) * 0.5f;
    var midRay = cam.ScreenPointToRay(mid);
    if (!mapPlane.Raycast(midRay, out float enter)) return;

    Vector3 worldBefore = midRay.GetPoint(enter);

    // Apply the scale
    transform.localScale = Vector3.one * newScale;

    // After scaling, re‐project the same screen midpoint onto the map plane
    var midRayAfter = cam.ScreenPointToRay(mid);
    if (mapPlane.Raycast(midRayAfter, out float enterAfter))
    {
        Vector3 worldAfter = midRayAfter.GetPoint(enterAfter);
        // Offset so that worldBefore stays under the fingers
        transform.position -= (worldAfter - worldBefore);
    }
}

}
