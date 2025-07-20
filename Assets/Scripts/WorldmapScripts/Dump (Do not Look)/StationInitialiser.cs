// StationGenerator.cs  – drop anywhere inside Assets/
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StationGenerator : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("Drag the connections.json TextAsset (put it in a Resources or any Assets/ folder)")]
    public TextAsset connectionsJson;

    [Header("Output (Editor only)")]
    [Tooltip("Relative to Assets/, e.g. \"ScriptableObjects/Stations\"")]
    public string saveFolder = "ScriptableObjects/Stations";

    private void Start()
    {
#if UNITY_EDITOR
        GenerateAndSave();
#else
        Debug.LogWarning($"{nameof(StationGenerator)} only runs inside the Unity Editor.");
#endif
    }

#if UNITY_EDITOR
    [ContextMenu("Generate & Save Stations (Editor)")]
    private void GenerateAndSave()
    {
        if (connectionsJson == null)
        {
            Debug.LogError("❌  No JSON file assigned!");
            return;
        }

        // ensure folder exists
        string fullFolder = Path.Combine("Assets", saveFolder);
        if (!AssetDatabase.IsValidFolder(fullFolder))
        {
            Directory.CreateDirectory(fullFolder);
            AssetDatabase.Refresh();
        }

        // -------- 1 ► parse JSON --------
        Root root = JsonUtility.FromJson<Root>(connectionsJson.text);

        // first pass – create / update every StationObj
        var map = new Dictionary<string, StationObj>(StringComparer.OrdinalIgnoreCase);

        foreach (StationData s in root.data)
        {
            string assetName = $"{Sanitise(s.stationName)}.asset";
            string assetPath = Path.Combine(fullFolder, assetName);

            StationObj so = AssetDatabase.LoadAssetAtPath<StationObj>(assetPath);
            if (so == null)
            {
                so = ScriptableObject.CreateInstance<StationObj>();
                AssetDatabase.CreateAsset(so, assetPath);
            }

            so.id          = s.stationGlobalID;      // use the official ID
            so.stationName = s.stationName;
            so.description = $"Neighbours: {string.Join(", ", s.connections)}";
            so.lat         = s.lat;
            so.lon         = s.@long;                // @long escapes keyword ‘long’

            map[s.stationName] = so;
            EditorUtility.SetDirty(so);
        }

        // second pass – wire neighbour references
        foreach (StationData s in root.data)
        {
            StationObj so = map[s.stationName];
            so.neigbours.Clear();

            foreach (string nbName in s.connections)
            {
                if (map.TryGetValue(nbName, out StationObj nb))
                    so.neigbours.Add(nb);
                else
                    Debug.LogWarning($"Neighbour “{nbName}” referenced by “{s.stationName}” not found.");
            }

            EditorUtility.SetDirty(so);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"✅  Generated/updated {map.Count} stations in “{fullFolder}”.");
    }
#endif

    // ---------- helpers ----------
    private static string Sanitise(string raw)
    {
        foreach (char c in Path.GetInvalidFileNameChars()) raw = raw.Replace(c, '_');
        return raw.Replace(' ', '_');
    }

    // ---------- DTOs for JsonUtility ----------
    [Serializable] private class Root          { public List<StationData> data; }
    [Serializable] private class StationData
    {
        public string stationName;
        public string stationGlobalID;
        public double  lat;
        public double  @long;         // ‘long’ is a keyword; prefix with @
        public List<string> connections;
    }
}
