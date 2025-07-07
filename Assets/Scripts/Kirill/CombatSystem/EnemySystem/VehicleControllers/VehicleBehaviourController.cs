using UnityEngine;

public abstract class VehicleBehaviourController : MonoBehaviour
{
    private VehicleCombatBehaviourSystem controller;
    [SerializeField]
    public VehicleCombatBehaviourSystem Controller
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
        Controller = gameObject.GetComponent<VehicleCombatBehaviourSystem>();
    }
    public abstract void OnStartBehaviour();
    public abstract void OnFixedUpdateBehave();
    public abstract void OnEndBehaviour();
}
