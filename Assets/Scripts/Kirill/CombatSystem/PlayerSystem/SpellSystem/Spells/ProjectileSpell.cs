using Fusion;
using UnityEngine;

public class ProjectileSpell : ActiveSpell
{
    [SerializeField] private ProjectileSpellData data;

    public override SpellData SpellData => data;
    
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    protected override void ExecuteRpc(NetworkObject playerNO, NetworkObject nStart, Vector2 end)
    {
        PlayerMock playerMock = playerNO.GetComponent<PlayerMock>();
        Transform start = nStart.transform;
        ProjectileSpellExecutor executor = playerMock.gameObject.AddComponent<ProjectileSpellExecutor>();
        if (data.targetType == TargetType.CURRENT_TARGET)
        {
            executor.Initialize(data, start, playerMock.GetCurrentTargetSelected());
        }
        else
        {
            executor.Initialize(data, start, end);
        }
    }
}
