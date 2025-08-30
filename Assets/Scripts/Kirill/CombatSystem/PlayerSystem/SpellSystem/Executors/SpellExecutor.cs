using System;
using Fusion;
using UnityEngine;

public abstract class SpellExecutor : NetworkBehaviour
{
    public abstract SpellExecutorData SpellExecutorData { get; }
    public Action OnStartExecution;
    public Action OnEndExecution;
    public Action<UnitController> OnHitExecution;
    
    [Networked]
    private TickTimer spellTimer { get; set; }

    protected UnitController caster;
    protected Transform castTransform;
    protected Vector2 endPosition;
    public virtual void Initialize(UnitController caster, Transform castTransform, Vector2 endPosition)
    {
        this.caster = caster;
        this.castTransform = castTransform;
        this.endPosition = endPosition;
        spellTimer = TickTimer.CreateFromSeconds(Runner, SpellExecutorData.executionDelay);
    }
    private int i = 0;

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsServer) return;
        if (!spellTimer.Expired(Runner)) return;
        
        float interval = SpellExecutorData.executionTime / SpellExecutorData.executionAmount;

        if (i < SpellExecutorData.executionAmount)
        {
            if (i == 0)
                StartExecution();
            Hit();
            spellTimer = TickTimer.CreateFromSeconds(Runner, interval);
            i++;
        }
        else
        {
            EndExecution();
        }
    }

    protected abstract void Hit(); // basically applies effects. Not neccessarily deals damage

    /*private void DealDamage()
    {
        Debug.Log("I am using Circle Spell and try to deal damage with it");
        Collider2D[] hits = Physics2D.OverlapCircleAll(castTransform.position, data.radius);

        foreach (Collider2D hit in hits)
        {
            Debug.Log("I hit an enemy with circle: " + hit.gameObject);
            if (hit.TryGetComponent(out UnitController unit))
            {
                if (unit is PlayerCombatSystem)
                    return;
                Debug.Log("I am hitting an enemy");
                player.Hit(unit, data.damageProExecution);
            }
        }
    }*/
    private void StartExecution()
    {
        OnStartExecution?.Invoke();
    }
    private void EndExecution()
    {
        OnEndExecution?.Invoke();
        Runner.Despawn(Object);
    }
}
