using UnityEngine;

[CreateAssetMenu(fileName = "TimeReloadData", menuName = "Spells/Reload/Time Reload Data")]
public class BloodPointsReloadData : ScriptableObject
{
    public int bloodPointsCost;
    public float minimalReloadTime;
}
