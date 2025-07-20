using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class GameCombatManager : NetworkBehaviour
{
    [SerializeField] private TrainSystem trainSystem;
    [SerializeField] private GameCombatUIManager ui;

    [SerializeField] private PlayerCombatSystem playerCombatSystem;

    [SerializeField] private int secondsToDestroy;

    [Networked]
    private TickTimer selfDestroyTimer { get; set; }

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            trainSystem.Setup();
        }
    }

    public PlayerCombatSystem GetNearestPlayer(Transform vehicleTransform) // Mock!
    {
        List<NetworkObject> players = NetworkManager.Instance.GetPlayerObjects();

        if (players.Count <= 0) return null;

        PlayerCombatSystem nearestPlayer = null;
        float nearestDistance = float.MaxValue;

        foreach (var playerNO in players)
        {
            float distance = (vehicleTransform.position - playerNO.transform.position).magnitude;
            if (nearestDistance > distance)
            {
                nearestDistance = distance;
                nearestPlayer = playerNO.GetComponent<PlayerCombatSystem>();
            }
        }
        return nearestPlayer;
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

    public void SetSpells(PlayerCombatSystem player, Spell spellArmor, Spell spellWeapon)
    {
        ui.SetSpells(player, spellArmor, spellWeapon);
    }

    public override void FixedUpdateNetwork()
    {
        if (!selfDestroyTimer.IsRunning) selfDestroyTimer = TickTimer.CreateFromSeconds(Runner, secondsToDestroy);
        if (selfDestroyTimer.Expired(Runner)) EndGameRpc();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public async void EndGameRpc()
    {
        if (!Runner.IsServer) return;
        float ratio = (float)TrainSystem.Instance.TotalBoxesAmount / (float)TrainSystem.Instance.MaxBoxesAmount;
        //Send to db end game

        await Runner.Shutdown(destroyGameObject: false);
    }
}
