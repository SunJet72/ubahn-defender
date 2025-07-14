using System.Collections.Generic;
using UnityEngine;

public class GameCombatManager : MonoBehaviour
{
    [SerializeField] private TrainSystem trainSystem;
    [SerializeField] private PlayerMock playerMock; // Mock!
    [SerializeField] private GameCombatUIManager ui;

    [SerializeField] private PlayerCombatSystem playerCombatSystem;
    [SerializeField] private ScriptableWeapon scriptableWeaponBuffer;
    [SerializeField] private ScriptableArmor scriptableArmorBuffer;
    [SerializeField] private List<ScriptableConsumable> consumables;

    void Start()
    {
        trainSystem.Setup();
        playerCombatSystem.Init(scriptableArmorBuffer, scriptableWeaponBuffer, consumables);
    }

    public PlayerMock GetNearestPlayer(Transform vehicleTransform) // Mock!
    {
        return playerMock;
    }

    public EctsContainer GetNearestContainer(Transform enemyTransform)
    {
        return trainSystem.ReturnNearestContainer(enemyTransform);
    }
    public Transform GetApplicableAbordagePoint(Transform vehicleTransform) // Mock!
    {
        return trainSystem.transform;
    }

    public void IncrementEnemyScore()
    {

    }

    public void SetSpells(Spell spellArmor, Spell spellWeapon)
    {
        ui.SetSpells(spellArmor, spellWeapon);
    }
}
