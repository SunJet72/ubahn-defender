using Fusion;
using UnityEngine;

public class GameCombatManager : NetworkBehaviour
{
    [SerializeField] private TrainSystem trainSystem;

    // [SerializeField] private PlayerMock playerMock; // Mock!

    public override void Spawned()
    {
        trainSystem.Setup();
    }

    public PlayerMock GetNearestPlayer(Transform vehicleTransform) // Mock!
    {
        return NetworkManager.Instance.GetPlayers()[0].GetComponent<PlayerMock>();
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
}
