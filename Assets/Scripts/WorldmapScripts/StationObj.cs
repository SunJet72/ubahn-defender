using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Station", menuName = "Scriptable Objects/Station")]

public class StationObj: ScriptableObject
{
    public int id = -1;
    public string stationName = "";
    public string description = "";
    public List<StationObj> neigbours = new List<StationObj>();

}
