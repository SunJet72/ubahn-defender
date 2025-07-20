using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;  // Required for TextMeshPro types

public class UbahnMapVisualizer : MonoBehaviour
{
    [Header("Prefabs & Parents (World‑Space)")]
    [Tooltip("Prefab must include a SpriteRenderer and a TextMeshPro component for the station label.")]
    public GameObject stationPrefab;
    public Transform stationsParent;

    [Tooltip("Prefab with a LineRenderer (Use World Space). Material shader should be Unlit/Color).")]
    public GameObject linePrefab;
    public Transform linesParent;

    [Header("Map Size (World Units)")]
    public float mapWidth = 10000f;
    public float mapHeight = 10000f;

    // Colors for lines
    private readonly Color green      = new Color(0.0f, 0.8f, 0.2f);
    private readonly Color lightGreen = new Color(0.3f, 1.0f, 0.5f);
    private readonly Color orange     = new Color(1f, 0.55f, 0f);
    private readonly Color blue       = Color.blue;
    private readonly Color yellow     = Color.yellow;
    private readonly Color red        = Color.red;
    private readonly Color white      = Color.white;
    private readonly Color purple     = new Color(0.6f, 0.2f, 1f);

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

        // clear old
        stationsParent.DestroyChildren();
        linesParent.DestroyChildren();

        // compute bounds & center
        var lons = map.Select(s => s.lon);
        var lats = map.Select(s => s.lat);
        double minLon = lons.Min(), maxLon = lons.Max();
        double minLat = lats.Min(), maxLat = lats.Max();
        double centerLon = (minLon + maxLon) / 2.0;
        double centerLat = (minLat + maxLat) / 2.0;

        // instantiate stations and set labels/colors
        var stationNodes = new Dictionary<Station, GameObject>();
        foreach (var station in map)
        {
            var go = Instantiate(stationPrefab, stationsParent);
            go.name = station.StationName;

            float dx = (float)(station.lon - centerLon);
            float dy = (float)(station.lat - centerLat);
            float x  = dx / (float)(maxLon - minLon) * mapWidth;
            float y  = dy / (float)(maxLat - minLat) * mapHeight;
            go.transform.localPosition = new Vector3(x, y, 0f);

            // set TextMeshPro label: show name and tier
            string displayName = $"{station.StationName} (T{station.StationTier})";
            var tmp3D = go.GetComponentInChildren<TextMeshPro>();
            if (tmp3D != null)
                tmp3D.text = displayName;
            else
            {
                var tmpUI = go.GetComponentInChildren<TextMeshProUGUI>();
                if (tmpUI != null)
                    tmpUI.text = displayName;
                else
                    Debug.LogWarning($"Station prefab '{go.name}' has no TextMeshPro or TextMeshProUGUI component.");
            }

            // ---- SPRITE COLOR BY TIER ----
            var sr = go.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                switch (station.StationTier)
                {
                    case 1:
                        sr.color = blue;
                        break;
                    case 2:
                        sr.color = orange;
                        break;
                    case 3:
                        sr.color = purple;
                        break;
                    default:
                        sr.color = white;
                        break;
                }
            }
            //-------------------------------

            stationNodes[station] = go;
        }

        // instantiate colored lines with correct tinting
        var drawn = new HashSet<(Station, Station)>();
        foreach (var s in map)
        {
            foreach (var nb in s.Neighbours)
            {
                var pair = s.GetHashCode() < nb.GetHashCode()
                    ? (s, nb)
                    : (nb, s);
                if (drawn.Contains(pair)) continue;
                drawn.Add(pair);

                var shared = s.uBahnLines.Intersect(nb.uBahnLines).ToList();
                if (shared.Count == 0) continue;

                // choose color
                Color col = white;
                if (shared.Contains(UBahnLine.U1) || shared.Contains(UBahnLine.U7))
                    col = green;
                else if (shared.Contains(UBahnLine.U4))
                    col = lightGreen;
                else if (shared.Contains(UBahnLine.U5))
                    col = orange;
                else if (shared.Contains(UBahnLine.U6))
                    col = blue;
                else if (shared.Contains(UBahnLine.U3))
                    col = yellow;
                else if (shared.Contains(UBahnLine.U2))
                    col = red;

                var lineGO = Instantiate(linePrefab, linesParent);
                lineGO.name = $"Line_{pair.Item1.StationName}_{pair.Item2.StationName}";
                var lr = lineGO.GetComponent<LineRenderer>();
                if (lr == null)
                {
                    Debug.LogError("LinePrefab missing LineRenderer!", lineGO);
                    continue;
                }

                // clone and tint the material itself
                var mat = new Material(lr.sharedMaterial);
                mat.color = col;
                lr.material = mat;

                // also set gradient in case shader uses start/end
                lr.startColor = col;
                lr.endColor   = col;

                lr.positionCount = 2;
                lr.SetPosition(0, stationNodes[pair.Item1].transform.position);
                lr.SetPosition(1, stationNodes[pair.Item2].transform.position);
            }
        }

        Debug.Log($"Map built: {stationNodes.Count} stations, {drawn.Count} edges");
    }
}

// Utility extension
public static class TransformExtensions
{
    public static void DestroyChildren(this Transform t)
    {
        for (int i = t.childCount - 1; i >= 0; --i)
            Object.DestroyImmediate(t.GetChild(i).gameObject);
    }
}
