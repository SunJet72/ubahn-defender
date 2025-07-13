using UnityEngine;

[CreateAssetMenu(fileName = "VehicleData_NAME_Chase", menuName = "Combat/Vehicle/Chase Data")]
public class ChasingVehicleControllerData : ScriptableObject
{
    public float lowToleranceDistance;
    public float highToleranceDistance;
}
