using UnityEngine;

public class GameCombatManager : MonoBehaviour
{
    [SerializeField] private TrainSystem trainSystem;

    [SerializeField] private PlayerMock playerMock; // Mock!

    void Start()
    {
        trainSystem.Setup();
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
}
