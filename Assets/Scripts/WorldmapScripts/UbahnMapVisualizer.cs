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

}
