using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class GameCombatManager : NetworkBehaviour
{
    [SerializeField] private TrainSystem trainSystem;
    [SerializeField] private GameCombatUIManager ui;

    [SerializeField] private PlayerCombatSystem playerCombatSystem;

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            trainSystem.Setup();
        }
    }

    public PlayerMock GetNearestPlayer(Transform vehicleTransform) // Mock!
    {
        List<NetworkObject> players = NetworkManager.Instance.GetPlayerObjects();
        if (players.Count <= 0) return null;
        return players[0].GetComponent<PlayerMock>();
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
