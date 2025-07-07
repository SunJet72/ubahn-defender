using UnityEngine;

public class VehicleCombatBehaviourSystem : MonoBehaviour
{
    [SerializeField] private VehicleCombatSystemData data;

    public bool useAbordagingController;
    public bool useChasingController;
    public bool useEscapingController;
    public bool useNoDriverController;

    public AbordagingVehicleController abordagingVehicleController;
    public ChasingVehicleController chasingVehicleController;
    public EscapingVehicleController escapingVehicleController;
    public NoDriverVehicleController noDriverVehicleController;

    [SerializeField] private Transform abordagePoint;

    private VehicleBehaviourController curController;

    void Start()
    {
        abordagingVehicleController.SetTarget(abordagePoint.position);
        ChangeCurrentBehaviour(abordagingVehicleController);
    }

    public void OnSpawn()
    {

    }

    private void ChangeCurrentBehaviour(VehicleBehaviourController controller)
    {
        if (curController != null)
        {
            curController.OnEndBehaviour();
        }
        curController = controller;
        controller.OnStartBehaviour();
    }

    void FixedUpdate()
    {
        if (curController == null)
        {
            Debug.LogError("I dont have needed behaviour controller");
            return;
        }
        curController.OnFixedUpdateBehave();
    }

    //---// AbordaginController //---//
    public void Abordage(Vector2 abordagingPoint)
    {
        // TODO: Set All enemies out

        //
        ChangeCurrentBehaviour(escapingVehicleController);
    }
}
