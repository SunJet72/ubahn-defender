using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WorldMapController : MonoBehaviour
{
    public static WorldMapController instance;

    public HashSet<Station> map = new HashSet<Station>();
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        var allStationObjects = Resources.LoadAll<StationObj>("Stations");

        // Initialising map object
        HashSet<Station> tempMap = new HashSet<Station>();
        foreach (StationObj obj in allStationObjects)
        {
            tempMap.Add(new Station(obj));
        }

        while (tempMap.Count > 0)
        {
            Station station = tempMap.First();
            List<Station> neighbours = tempMap.Where(st => allStationObjects.FirstOrDefault(obj => obj.id == station.Id).neigbours.Select(obj => obj.id).Contains(st.Id)).ToList();
            station.Neighbours.AddRange(neighbours);
            foreach (Station neighbour in neighbours)
            {
                neighbour.Neighbours.Add(station);
            }
            map.Add(station);
            tempMap.Remove(station);
        }

        LoadMapData();

        // foreach (Station station in map)
        // {
        //     Debug.Log(station);
        // }

    }

    private async void LoadMapData()
    {
        //Load data from server
        // Warning mock shit ahead
        List<int> ids = map.Select(station => station.Id).ToList();
        Dictionary<int, float> stationScores = new Dictionary<int, float>();

        foreach (int id in ids)
        {
            stationScores.Add(id, Random.Range(0, 1000));
        }
        //--------------------------------------------------

        foreach (Station station in map)
        {
            float stationValue;
            if (stationScores.TryGetValue(station.Id, out stationValue))
            {
                station.Wealth = stationValue;
            }
            else
            {
                Debug.LogError("Wrong Id in loded map data");
            }
        }

    }

    

}
