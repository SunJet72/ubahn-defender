using System.Collections;
using Fusion;
using UnityEngine;

public class SplashSpellExecutor : NetworkBehaviour
{
    private SplashSpellData data;
    private Transform castTransform;
    private Vector2 direction;
    private PlayerCombatSystem player;

    [Networked]
    private TickTimer spellTimer { get; set; }

    public void Initialize(SplashSpellData spellData, PlayerCombatSystem player, Transform castTransform, Vector2 castedPoint)
    {
        data = spellData;
        this.castTransform = castTransform;
        direction = castedPoint - (Vector2)castTransform.position;
        this.player = player;
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
        else Runner.Despawn(Object);
    }

    private void DealDamage()
    {
        Debug.Log("I am using Splash Spell and try to deal damage with it");
        Collider2D[] hits = Physics2D.OverlapCircleAll(castTransform.position, data.radius);
        foreach (var hit in hits)
        {
            Vector3 toTarget = (hit.transform.position - castTransform.position);
            toTarget.z = 0;

            float angle = Vector3.Angle(direction, toTarget.normalized);
            if (angle <= data.fov / 2f)
            {
                Debug.Log("I hit the enemy with splash: " + hit.gameObject);
                if (hit.TryGetComponent(out UnitController unit))
                {
                    if (unit is PlayerCombatSystem)
                        return;
                    Debug.LogWarning("!!! I am hurting enemy with splash");
                    unit.Hurt(CalculateDamage(data.damageProExecution), player.ArmorPenetration, player);
                }
            }
        }
    }

    private float CalculateDamage(float damage)
    {
        return damage * ((100f + player.Strength) / 100f);
    }
}
