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
        ectsContainer.OnDieEvent += OnHauntedContainerFullyStolen;
        if (data.canBeCancelledByAttack)
        {
            Controller.OnHurtEvent += OnChangeAggro;
        }

        timeSpent = 0;
    }

    public override void OnFixedUpdateBehave()
    {
        timeSpent += Runner.DeltaTime;
        if (timeSpent >= data.stealTime)
        {
            BoxWasStolen();
        }
    }

    private void BoxWasStolen()
    {
        Controller.BoxWasStolen(ectsContainer);
    }

    private void OnHauntedContainerFullyStolen(EctsContainer container)
    {
        if (ectsContainer != container)
        {
            Debug.LogWarning("Wrong Container. Probably forgot to unsubscribe or smth else happened wrong");
            return;
        }
        Controller.ContainerWasEmptied(container);
    }

    private void OnChangeAggro(UnitController attacker, UnitController target) // Target should be us
    {
        if (attacker != null)
            Controller.ChangeAggro(attacker);
        if (data.canBeCancelledByAttack)
        {
            Controller.OnHurtEvent -= OnChangeAggro;
        }
    }
    
    public override void OnEndBehaviour()
    {
        timeSpent = 0;
        target = null;
        ectsContainer = null;
    }
}
