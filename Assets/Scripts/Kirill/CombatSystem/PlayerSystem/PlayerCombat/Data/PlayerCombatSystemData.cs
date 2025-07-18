using UnityEngine;
public enum PlayerClass
{
    WARRIOR,
    RANGER,
    INGENIEUR
}
[CreateAssetMenu(fileName = "PlayerCombatSystemData", menuName = "Scriptable Objects/PlayerCombatSystemData")]
public class PlayerCombatSystemData : UnitData
{
    public PlayerClass playerClass;
}
