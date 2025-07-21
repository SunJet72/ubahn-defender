using UnityEngine;

public class ChasingVehicleController : VehicleBehaviourController
{
    Transform targetTransform;
    Vector2 targetPosition;
    bool isChasingThePoint;
    PlayerCombatSystem player;
    [SerializeField] private ChasingVehicleControllerData data;
    public void SetTarget(PlayerCombatSystem player)
    {
        this.player = player;
        this.targetTransform = player.gameObject.transform;
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
            transform.Translate((targetPosition - (Vector2)transform.position).normalized * Controller.Speed * Runner.DeltaTime);
            if ((targetPosition - (Vector2)transform.position).magnitude < 0.1f)
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
        Controller.TellRangersToAttack(player);
    }

    public override void OnEndBehaviour()
    {
        targetTransform = null;
        targetPosition = Vector2.zero;
    }
}
