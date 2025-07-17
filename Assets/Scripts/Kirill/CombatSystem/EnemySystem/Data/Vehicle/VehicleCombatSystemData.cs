using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleData_NAME_System", menuName = "Combat/Vehicle Combat System Data")]
public class VehicleCombatSystemData : UnitData
{
    public bool isVehicleToRangers; // if yes, it chases, and has rangers inside, if not, it abordages, and has warriors and scoundrels inside
    public int passangersAmount;
    public List<GameObject> _passengersList; // listSize == amount of vehicle passangers, which is defined. The first one is a vehicle rider.
}
