using UnityEngine;

public class CircleSpell : ActiveSpell
{
    [SerializeField] private CircleSpellData data;
    protected override void Execute(PlayerMock playerMock, Transform start, Vector2 end) // end is not used
    {
        CircleSpellExecutor executor = playerMock.gameObject.AddComponent<CircleSpellExecutor>();
        executor.Initialize(data, start);
    }
}
