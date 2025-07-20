using Fusion;
using UnityEngine;
public class EnemyCombatBehaviourSystem : UnitController, IAfterSpawned
{
    public override UnitData UnitData => data;
    [SerializeField] private EnemyCombatSystemData data;
    public bool useRidingController;
    public bool useAttackingMeleeController;
    public bool useAttackingRangedController;
    public bool useEscapingController;
    public bool useHauntingController;
    public bool useStealingController;

    public RidingBehaviourController ridingBehaviourController;
    public AttackingMeleeBehaviourController attackingMeleeBehaviourController;
    public AttackingRangedBehaviourController attackingRangedBehaviourController;
    public EscapingBehaviourController escapingBehaviourController;
    public HauntingBehaviourController hauntingBehaviourController;
    public StealingBehaviourController stealingBehaviourController;

    [HideInInspector] public EnemyType EnemyType { get => data.enemyType; }

    [SerializeField] private PlayerMock playerMock;
    private GameCombatManager gameCombatManager;

    private CombatBehaviourController curController;

    public void AfterSpawned()
    {
        base.Init();

        gameCombatManager = GameObject.Find("GameCombatManager").GetComponent<GameCombatManager>();
        ChangeCurrentBehaviour(ridingBehaviourController);

        //ChangeCurrentBehaviour(escapingBehaviourController);


        /*attackingMeleeBehaviourController.SetTarget(playerMock);
        ChangeCurrentBehaviour(attackingMeleeBehaviourController);*/

        //ChangeCurrentBehaviour(hauntingBehaviourController);

        /*attackingRangedBehaviourController.SetTarget(playerMock);
        ChangeCurrentBehaviour(attackingRangedBehaviourController);*/
    }

    private void ChangeCurrentBehaviour(CombatBehaviourController controller)
    {
        if (curController != null)
        {
            curController.OnEndBehaviour();
        }
        curController = controller;
        controller.OnStartBehaviour();
    }

    public override void FixedUpdateNetwork()
    {
        if (curController == null)
        {
            // Debug.LogError("I dont have needed behaviour controller");
            return;
        }
        curController.OnFixedUpdateBehave();
    }


    //---//RidingBehaviourController //---//
    public void VehicleToldTheRangerToAttack(PlayerCombatSystem target) // Applicable only to Rangers
    {
        SetAttackingRangedBehaviour(target);
    }

    public void VehicleEndedTheAbordageProcess() // Applicable only to Melee and Scoundrels
    {
        SetHauntingBehavior();
    }


    //---// AttackingMeleeBehaviourController //---//
    public void MeleeLoseTarget()
    {
        SetHauntingBehavior();
    }

    private void SetAttackingMeleeBehaviour()
    {
        PlayerCombatSystem player = gameCombatManager.GetNearestPlayer(transform);
        if (player == null)
        {
            SetHauntingBehavior();
        }
        else
        {
            attackingMeleeBehaviourController.SetTarget(player);
            ChangeCurrentBehaviour(attackingRangedBehaviourController);
        }
    }

    private void SetAttackingMeleeBehaviour(PlayerCombatSystem target)
    {
        Debug.Log("new target: " + target);
        attackingMeleeBehaviourController.SetTarget(target);
        ChangeCurrentBehaviour(attackingMeleeBehaviourController);
    }

    //---// AttackingRangedBehaviourController //---//
    public void RangedLoseTarget()
    {
        ChangeCurrentBehaviour(ridingBehaviourController);
    }

    private void SetAttackingRangedBehaviour(PlayerCombatSystem target)
    {
        if (target == null)
        {
            ChangeCurrentBehaviour(ridingBehaviourController);
        }
        else
        {
            attackingRangedBehaviourController.SetTarget(target);
            ChangeCurrentBehaviour(attackingRangedBehaviourController);
        }
    }


    //---// HauntingBehaviourController //---//
    public void StartStealing(EctsContainer container)
    {
        stealingBehaviourController.SetTarget(container);
        ChangeCurrentBehaviour(stealingBehaviourController);
    }

    public void ContainerWasEmptied(EctsContainer oldContainer)
    {
        SetHauntingBehavior();
    }

    public void ChangeAggro(UnitController attacker)
    {
        Debug.Log("I am " + this);
        Debug.Log("Changing aggro on: " + (attacker as PlayerCombatSystem));
        SetAttackingMeleeBehaviour(attacker as PlayerCombatSystem);
    }

    private void SetHauntingBehavior()
    {
        EctsContainer container = gameCombatManager.GetNearestContainer(transform);
        if (container == null)
            ChangeCurrentBehaviour(escapingBehaviourController);
        else
        {
            hauntingBehaviourController.SetTarget(container);
            ChangeCurrentBehaviour(hauntingBehaviourController);
        }
    }

    //---// StealingBehaviourController //---//
    public void BoxWasStolen(EctsContainer container)
    {
        container.Steal();
        Debug.Log("I increment enemy score for stealing box with ects");
        ChangeCurrentBehaviour(escapingBehaviourController);
    }

    protected override void Die()
    {
        TriggerDeathEvent();
        Runner.Despawn(Object);
    }

    public override void Hurt(float damage, float penetration, UnitController attacker)
    {
        base.Hurt(damage, penetration, attacker);
        if (attacker != null)
            transform.Translate((transform.position - attacker.transform.position) * 0.45f);
    }
}
