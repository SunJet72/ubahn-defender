using System.Collections;
using Fusion;
using UnityEngine;

public class SplashSpellExecutor : SpellExecutor
{
    [SerializeField] private SplashSpellExecutorData data;
    public override SpellExecutorData SpellExecutorData => data;

    private Vector2 direction = Vector2.zero;

    public override void Initialize(UnitController caster, Transform castTransform, Vector2 endPosition)
    {
        base.Initialize(caster, castTransform, endPosition);
        direction = endPosition - (Vector2)castTransform.position;
    }

    protected override void Hit()
    {
        Debug.Log("I am using Splash Spell and try to deal damage with it");
        Collider2D[] hits = Physics2D.OverlapCircleAll(castTransform.position, data.radius);
        foreach (var hit in hits)
        {
            Vector3 toTarget = hit.transform.position - castTransform.position;
            toTarget.z = 0;

            float angle = Vector3.Angle(direction, toTarget.normalized);
            if (angle <= data.fov / 2f)
            {
                Debug.Log("I hit the enemy with splash: " + hit.gameObject);
                if (hit.TryGetComponent(out UnitController unit))
                {
                    Debug.LogWarning("!!! I am hurting enemy with splash");
                    OnHitExecution.Invoke(unit);
                }
            }
        }
    }
}
