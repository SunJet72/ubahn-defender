using UnityEngine;

public class NoDriverVehicleController : VehicleBehaviourController
{
    public override void OnStartBehaviour()
    {
        // TODO: Set the transform to the bottom Grid.
    }

    public override void OnFixedUpdateBehave()
    {
        DoNothing();
    }

    public override void OnEndBehaviour()
    {
        Debug.LogError("The Vehicle has ended the Endstate: NoDriverState");
    }

    private void DoNothing() {}
}
