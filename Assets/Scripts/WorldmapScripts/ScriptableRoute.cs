using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptablleRoute", menuName = "Scriptable Objects/ScriptablleRoute")]
public class ScriptableRoute : ScriptableObject
{
    public string routeName;
    public List<StationObj> routeObj = new List<StationObj>();
    public StationObj currStation;
}
