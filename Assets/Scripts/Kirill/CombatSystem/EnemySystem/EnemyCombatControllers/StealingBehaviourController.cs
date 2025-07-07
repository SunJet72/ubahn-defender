using UnityEngine;

public class StealingBehaviourController : CombatBehaviourController
{
    private EctsContainer ectsContainer;
    private Transform target;
    [SerializeField] private EnemyStealData data;
    private float timeSpent;
    public void SetTarget(EctsContainer container)
    {
        ectsContainer = container;
        target = container.gameObject.transform;
    }

    public override void OnStartBehaviour()
    {
        timeSpent = 0;
    }

    public override void OnFixedUpdateBehave()
    {
        timeSpent += Time.fixedDeltaTime;
        if (timeSpent >= data.stealTime)
        {
            BoxWasStolen();
        }
    }

    private void BoxWasStolen()
    {
        Controller.BoxWasStolen(ectsContainer);
    }
    
        public override void OnEndBehaviour()
    {
        timeSpent = 0;
        target = null;
        ectsContainer = null;
    }
}
