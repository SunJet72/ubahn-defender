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
        
    }

    public override void OnEndBehaviour()
    {
        ectsContainer.OnDieEvent -= OnHauntedContainerFullyStolen;
        ectsContainer = null;
        target = null;
    }

}
