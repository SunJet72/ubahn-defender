using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class VehicleCombatBehaviourSystem : UnitController//, IAfterSpawned
{
    [SerializeField] private VehicleCombatSystemData data;
    public override UnitData UnitData => data;

    public bool useAbordagingController;
    public bool useChasingController;
    public bool useEscapingController;
    public bool useNoDriverController;

    public AbordagingVehicleController abordagingVehicleController;
    public ChasingVehicleController chasingVehicleController;
    public EscapingVehicleController escapingVehicleController;
    public NoDriverVehicleController noDriverVehicleController;
    private GameCombatManager gameCombatManager;
    [SerializeField] private List<Transform> passengerSeats; // has to be equall to passengersAmount in data

    [SerializeField] private BoxCollider2D colliderToDisableHardCoded;

    private VehicleBehaviourController curController;
    private EnemyCombatBehaviourSystem[] enemies;

    public void Initialize()
    {
        Debug.LogWarning("Vehicle Was initialized");
        base.Init();

        gameCombatManager = GameObject.Find("GameCombatManager").GetComponent<GameCombatManager>();
        if (data._passengersList == null || data.passangersAmount != data._passengersList.Count || passengerSeats == null || passengerSeats.Count != data.passangersAmount)
        {
            Debug.Log("Vehicle Passengers were wrong defined. Please take a look at SO and passengerSeats. They have to be equal to passengersAmount in data");
            ChangeCurrentBehaviour(noDriverVehicleController);
            return;
        }

        enemies = new EnemyCombatBehaviourSystem[data.passangersAmount];

        if (Runner.IsServer)
        {
            for (int i = 0; i < data.passangersAmount; i++)
            {
                NetworkObject go = Runner.Spawn(data._passengersList[i], onBeforeSpawned: (runner, spawned) => {
                    spawned.transform.SetParent(passengerSeats[i], false);
                    spawned.transform.localPosition = Vector3.zero;
                });
                enemies[i] = go.GetComponent<EnemyCombatBehaviourSystem>();
            }
        }

        InitBehaviour();
    }

    private void ChangeCurrentBehaviour(VehicleBehaviourController controller)
    {
        if (curController != null)
        {
            curController.OnEndBehaviour();
        }
        curController = controller;
        controller.OnStartBehaviour();
    }

    private void InitBehaviour()
    {
        if (data.isVehicleToRangers)
        {
            PlayerCombatSystem playerMock = gameCombatManager.GetNearestPlayer(transform);
            if (playerMock == null) return;
            chasingVehicleController.SetTarget(playerMock);
            ChangeCurrentBehaviour(chasingVehicleController);
        }
        else
        {
            Transform abordagePoint = gameCombatManager.GetApplicableAbordagePoint(transform);
            abordagingVehicleController.SetTarget(abordagePoint);
            ChangeCurrentBehaviour(abordagingVehicleController);
        }
    }
    private TickTimer timer;
    private bool started = false;
    public override void FixedUpdateNetwork()
    {
        // if (!timer.IsRunning) timer = TickTimer.CreateFromSeconds(Runner, 7);

        // if (!timer.Expired(Runner)) return;

        if (!started)
        {
            Initialize();
            started = true;
        }

        if (curController == null)
        {
            InitBehaviour();
        }
        curController.OnFixedUpdateBehave();
    }

    //---// AbordagingController //---//
    public void Abordage(Vector2 abordagingPoint)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && (enemies[i].EnemyType == EnemyType.MELEE || enemies[i].EnemyType == EnemyType.SCOUNDREL))
            {
                enemies[i].gameObject.transform.SetParent(null);
                enemies[i].VehicleEndedTheAbordageProcess();
                enemies[i] = null;
            }
        }
        ChangeCurrentBehaviour(escapingVehicleController);
    }

    //---// ChasingController //---//
    public void TellRangersToAttack(PlayerCombatSystem player)
    {
        Debug.Log("I am trying to tell the ranger, so that they could attack");
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && enemies[i].EnemyType == EnemyType.RANGED)
            {
                enemies[i].VehicleToldTheRangerToAttack(player);
            }
        }
    }

    public void TellRangersToStopAttack()
    {
        Debug.Log("I am trying to tell the ranger, so that they could attack");
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && enemies[i].EnemyType == EnemyType.RANGED)
            {
                enemies[i].RangedLoseTarget();
            }
        }
    }

    protected override void Die()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                Hit(enemy, 999999);
            }
        }
        Destroy(colliderToDisableHardCoded);
        TriggerDeathEvent();
        Runner.Despawn(Object);
    }
}
