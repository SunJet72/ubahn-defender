using Fusion;
using UnityEngine;

public class EffectorSpell : ActiveSpell
{
    [SerializeField] private EffectorSpellData data;
    public override SpellData SpellData => data;

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    protected override void ExecuteRpc(NetworkObject playerNO, NetworkObject nStart, Vector2 end)
    {
        PlayerCombatSystem player = playerNO.GetComponent<PlayerCombatSystem>();
        Transform start = nStart.transform;
        if (data.type == EffectorSpellType.SELF)
        {
            foreach (StatusEffect statusEffect in data.statusEffects)
            {
                statusEffect.ApplyEffect(player, start);
            }
        }
        else if (data.type == EffectorSpellType.TARGET)
        {
            foreach (StatusEffect statusEffect in data.statusEffects)
            {
                var target = player.GetCurrentTargetSelected();
                if (target == null)
                    return;
                statusEffect.ApplyEffect(player, target.transform);
            }
        }
    }
}
