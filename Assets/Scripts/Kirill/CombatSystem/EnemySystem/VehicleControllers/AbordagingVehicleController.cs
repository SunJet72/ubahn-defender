using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class AbordagingVehicleController : VehicleBehaviourController
{
    [SerializeField] private AbordagingVehicleControllerData data;
    private Vector2 abordagingPoint;

    private bool isPreparingAbordaging = false;
    private float curTimeforAbordagingLeft;
    public void SetTarget(Transform abordagePointTransform) // Position of the point on the train edge, where abordage has to be happened.
    {
        abordagingPoint = abordagePointTransform.position;
    }
    public override void OnEndBehaviour()
    {

    }

    public override void OnFixedUpdateBehave()
    {
        if (!isPreparingAbordaging)
        {
            Move();
            float distance = (abordagingPoint - (Vector2)transform.position).magnitude;
            if (distance < 0.05f)
            {
                isPreparingAbordaging = true;
            }
        }
        else
        {
            curTimeforAbordagingLeft -= Time.fixedDeltaTime;
            if (curTimeforAbordagingLeft <= 0)
            {
                Abordage();
            }
        }

    }

    private void Move()
    {
        transform.Translate((abordagingPoint - (Vector2)transform.position).normalized * Controller.Speed * Time.fixedDeltaTime);
    }

    private void Abordage()
    {
        Debug.Log(Controller);
        Controller.Abordage(abordagingPoint);
    }

    public override void OnStartBehaviour()
    {
        if (abordagingPoint == null)
        {
            Debug.LogError("I didnt become a point to abordage on");
            abordagingPoint = Vector2.zero;
            return;
        }
        curTimeforAbordagingLeft = data.timeForAbordage;
    }
}
