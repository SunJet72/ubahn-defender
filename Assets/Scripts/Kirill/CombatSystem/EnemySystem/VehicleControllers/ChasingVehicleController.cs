using UnityEngine;

public class ChasingVehicleController : VehicleBehaviourController
{
    Transform targetTransform;
    Vector2 targetPosition;
    bool isChasingThePoint;
    [SerializeField] private ChasingVehicleControllerData data;
    public void SetTarget(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
        DeterminePosition();
    }
    public override void OnStartBehaviour()
    {
        DeterminePosition();
        isChasingThePoint = true;
    }

    private void DeterminePosition()
    {
        // TODO: Refactor here, so that it takes into consideration, on which side of the train it is
        targetPosition = (Vector2)targetTransform.position + new Vector2(data.lowToleranceDistance, 0);
    }

    public override void OnFixedUpdateBehave()
    {
        if (isChasingThePoint)
        {
            transform.Translate((targetPosition - (Vector2)transform.position).normalized * data.vehicleSpeed * Time.fixedDeltaTime);
            if ((targetPosition - (Vector2)transform.position).magnitude < 0.01f)
            {
                if (((Vector2)targetTransform.position - (Vector2)transform.position).magnitude > data.highToleranceDistance)
                {
                    DeterminePosition();
                }
                isChasingThePoint = false;
                TellRangersToAttack(true);
            }
        }
        else
        {
            if (((Vector2)targetTransform.position - (Vector2)transform.position).magnitude > data.highToleranceDistance)
            {
                TellRangersToAttack(false);
                DeterminePosition();
                isChasingThePoint = true;
            }
        }
    }
    private void TellRangersToAttack(bool isAttacking)
    {
        // TODO: All the rangers in the vehicle have to change their behavior.
    }

    public override void OnEndBehaviour()
    {
        targetTransform = null;
        targetPosition = Vector2.zero;
    }
}
