using System.Data.Common;
using UnityEngine;

public class HauntingBehaviourController : CombatBehaviourController
{
    private EctsContainer ectsContainer;
    private Transform target;
    [SerializeField] private EnemyHauntData data;

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

    }

    public override void OnFixedUpdateBehave()
    {
        float distance = (transform.position - target.position).magnitude;
        if (distance > data.distanceBeforeSteal)
        {
            Chaise();
        }
        else
        {
            StartStealing();
        }
    }

    private void Chaise()
    {
        transform.Translate((target.position - transform.position).normalized * Controller.Speed * Runner.DeltaTime);
    }

    private void StartStealing()
    {
        Controller.StartStealing(ectsContainer);
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
    }

    public override void OnEndBehaviour()
    {
        ectsContainer.OnDieEvent -= OnHauntedContainerFullyStolen;
        if (data.canBeCancelledByAttack)
        {
            Controller.OnHurtEvent -= OnChangeAggro;
        }

        ectsContainer = null;
        target = null;
    }

}
