using System.Collections;
using Fusion;
using UnityEngine;

public class CircleSpellExecutor : NetworkBehaviour
{
    private CircleSpellData data;
    private Transform castTransform;
    private PlayerCombatSystem player;

    [Networked]
    private TickTimer spellTimer { get; set; }
    public void Initialize(CircleSpellData spellData, Transform castTransform, PlayerCombatSystem player)
    {
        data = spellData;
        this.player = player;
        this.castTransform = castTransform;
        spellTimer = TickTimer.CreateFromSeconds(Runner, data.executionDelay);
    }
    private int i = 0;

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsServer) return;
        if (!spellTimer.Expired(Runner)) return;
        
        float interval = data.executionTime / data.executionAmount;

        if (i < data.executionAmount)
        {
            DealDamage();
            spellTimer = TickTimer.CreateFromSeconds(Runner, interval);
            i++;
        }
        else
        {
            Runner.Despawn(Object);
        }
    }

    private void DealDamage()
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
                unit.Hurt(CalculateDamage(data.damageProExecution), player);
            }
        }
    }

    private float CalculateDamage(float damage)
    {
        return damage * ((100f + player.Strength) / 100f);
    }
}
