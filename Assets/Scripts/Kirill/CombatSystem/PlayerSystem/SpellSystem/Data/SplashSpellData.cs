using UnityEngine;

[CreateAssetMenu(fileName = "SplashSpellData", menuName = "Spells/SplashSpellData")]
public class SplashSpellData : SpellData
{
    [Header("Splash Spell Info")]
    public float radius;
    public float fov;

    public float executionTime;
    public float executionDelay;
    public int executionAmount;
    public float damageProExecution;
}
