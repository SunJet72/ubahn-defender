using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptablleRoute", menuName = "Scriptable Objects/ScriptablleRoute")]
public class ScriptablleRoute : ScriptableObject
{
    public string routeName;
    public List<StationObj> route = new List<StationObj>();
}
