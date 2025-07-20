using Fusion;
using UnityEngine;

public abstract class CombatBehaviourController : NetworkBehaviour
{
    private EnemyCombatBehaviourSystem controller;
    [SerializeField] public EnemyCombatBehaviourSystem Controller
    {
        get => controller;
        set
        {
            controller = value;
            Debug.Log(value);
        }
    }
    public override void Spawned()
    {
        Controller = gameObject.GetComponent<EnemyCombatBehaviourSystem>();
    }
    public abstract void OnStartBehaviour();
    public abstract void OnFixedUpdateBehave();
    public abstract void OnEndBehaviour();
}
