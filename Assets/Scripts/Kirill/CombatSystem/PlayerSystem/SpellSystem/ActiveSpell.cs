using UnityEngine;

public abstract class ActiveSpell : Spell
{
    public Reload Reload { get => reload; }
    [SerializeField] private Reload reload;
    public void Activate(PlayerMock playerMock, Transform start, Vector2 end)
    {
        if (!reload.IsReady())
            return;
        Execute(playerMock, start, end);
        reload.SpellWasUsed();
    }

    protected abstract void Execute(PlayerMock playerMock, Transform start, Vector2 end);

}
