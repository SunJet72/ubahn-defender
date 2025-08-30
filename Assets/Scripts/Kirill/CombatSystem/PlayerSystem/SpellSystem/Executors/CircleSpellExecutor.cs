using System.Collections;
using Fusion;
using UnityEngine;

public class CircleSpellExecutor : SpellExecutor
{
    [SerializeField] private CircleSpellExecutorData data;

    public override SpellExecutorData SpellExecutorData => data;

    protected override void Hit()
    {
        Debug.Log("I am using Circle Spell and try to cast effects with it");
        Collider2D[] hits = Physics2D.OverlapCircleAll(castTransform.position, data.radius);

        foreach (Collider2D hit in hits)
        {
            Debug.Log("I hit an enemy with circle: " + hit.gameObject);
            if (hit.TryGetComponent(out UnitController unit))
            {
                Debug.Log("I am casting effect on an unit");
                OnHitExecution.Invoke(unit);
            }
        }
    }
}
