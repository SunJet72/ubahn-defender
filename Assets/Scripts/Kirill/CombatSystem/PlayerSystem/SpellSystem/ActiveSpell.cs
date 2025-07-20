using Fusion;
using UnityEngine;

public abstract class ActiveSpell : Spell
{
    public Reload Reload { get => reload; }
    [SerializeField] private Reload reload;
    public void Activate(NetworkObject playerNO, NetworkObject nStart, Vector2 end)
    {
        if (!reload.IsReady())
            return;
        ExecuteRpc(playerNO, nStart, end);
        reload.SpellWasUsed();
    }

    protected abstract void ExecuteRpc(NetworkObject playerNO, NetworkObject nStart, Vector2 end);

}
