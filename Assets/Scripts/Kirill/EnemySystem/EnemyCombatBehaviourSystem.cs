using UnityEngine;

public class EnemyCombatBehaviourSystem : MonoBehaviour
{
    public bool useAttackingMeleeController;
    public bool useAttackingRangedController;
    public bool useEscapingController;
    public bool useHauntingController;
    public bool useStealingController;

    public AttackingMeleeBehaviourController attackingMeleeBehaviourController;
    public AttackingRangedBehaviourController attackingRangedBehaviourController;
    public EscapingBehaviourController escapingBehaviourController;
    public HauntingBehaviourController hauntingBehaviourController;
    public StealingBehaviourController stealingBehaviourController;

    [SerializeField] private PlayerMock playerMock;
    [SerializeField] private GameCombatManager gameCombatManager;

    private CombatBehaviourController curController;

    void Start()
    {
        //ChangeCurrentBehaviour(escapingBehaviourController);


        /*attackingMeleeBehaviourController.SetTarget(playerMock);
        ChangeCurrentBehaviour(attackingMeleeBehaviourController);*/

        ChangeCurrentBehaviour(hauntingBehaviourController);

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

    void FixedUpdate()
    {
        if (curController == null)
        {
            Debug.LogError("I dont have needed behaviour controller");
            return;
        }
        curController.OnFixedUpdateBehave();
    }



    //---// AttackingMeleeBehaviourController //---//
    public void MeleeLoseTarget()
    {
        ChangeCurrentBehaviour(hauntingBehaviourController);
    }

    //---// AttackingRangedBehaviourController //---//
    public void RangedLoseTarget()
    {
        ChangeCurrentBehaviour(escapingBehaviourController);
    }


    //---// HauntingBehaviourController //---//
    public EctsContainer GetContainerToHaunt()
    {
        return gameCombatManager.GetNearestContainer(transform);
    }
    public void StartStealing(EctsContainer container)
    {
        stealingBehaviourController.SetTarget(container);
        ChangeCurrentBehaviour(stealingBehaviourController);
    }


    //---// StealingBehaviourController //---//
    public void BoxWasStolen(EctsContainer container)
    {
        container.Steal();
        Debug.Log("I increment enemy score for stealing box with ects");
        ChangeCurrentBehaviour(escapingBehaviourController);
    }
}
