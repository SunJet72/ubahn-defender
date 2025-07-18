
using Fusion;
using UnityEngine;

public class CircleSpell : ActiveSpell
{
    [SerializeField] private CircleSpellData data;

    public override SpellData SpellData => data;

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    protected override void ExecuteRpc(NetworkObject playerNO, NetworkObject nStart, Vector2 end) // end is not used
    {
        PlayerMock playerMock = playerNO.GetComponent<PlayerMock>();
        Transform start = nStart.transform;
        CircleSpellExecutor executor = playerMock.gameObject.AddComponent<CircleSpellExecutor>();
        executor.Initialize(data, start);
    }
}
