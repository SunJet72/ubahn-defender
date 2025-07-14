using UnityEngine;

public abstract class CombatBehaviourController : MonoBehaviour
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
    void Awake()
    {
        Controller = gameObject.GetComponent<EnemyCombatBehaviourSystem>();
    }
    public abstract void OnStartBehaviour();
    public abstract void OnFixedUpdateBehave();
    public abstract void OnEndBehaviour();
}
