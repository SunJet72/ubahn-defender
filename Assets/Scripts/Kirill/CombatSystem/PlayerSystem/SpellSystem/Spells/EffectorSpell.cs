using Fusion;
using UnityEngine;

public class EffectorSpell : ActiveSpell
{
    [SerializeField] private EffectorSpellData data;
    public override SpellData SpellData => data;

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    protected override void ExecuteRpc(NetworkObject playerNO, NetworkObject nStart, Vector2 end)
    {
        PlayerMock playerMock = playerNO.GetComponent<PlayerMock>();
        Transform start = nStart.transform;
        if (data.type == EffectorSpellType.SELF)
        {
            foreach (StatusEffect statusEffect in data.statusEffects)
            {
                statusEffect.ApplyEfect(start);
            }
        }
        // TODO: if apply effects on others, like areas, etc.
    }
}
