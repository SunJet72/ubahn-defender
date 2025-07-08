using UnityEngine;

public class RidingBehaviourController : CombatBehaviourController
{
    public override void OnStartBehaviour()
    {
        DoNothing();
    }

    public override void OnFixedUpdateBehave()
    {
        DoNothing();
    }

    public override void OnEndBehaviour()
    {
        DoNothing();
    }

    private void DoNothing() {}
}
