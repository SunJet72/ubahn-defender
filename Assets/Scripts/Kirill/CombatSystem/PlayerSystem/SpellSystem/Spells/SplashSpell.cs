using UnityEngine;

[CreateAssetMenu(fileName = "SplashSpell", menuName = "Scriptable Objects/SplashSpell")]
public class SplashSpell : ActiveSpell
{
    [SerializeField] private SplashSpellData data;
    protected override void Execute(PlayerMock playerMock, Transform start, Vector2 end)
    {
        SplashSpellExecutor executor = playerMock.gameObject.AddComponent<SplashSpellExecutor>();
        executor.Initialize(data, start, end);
    }
}
