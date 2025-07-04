using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public abstract class ScriptableActionBase : ScriptableObject
{
    public string actionName;

    public string description;
    public bool isPassive;


    public MonoScript ComponentSkript;

    public void SetUp(GameObject player, UnityEvent actionEvent)
    {
        Debug.Log("trying to configure stuff");
        Type ComponentType = ComponentSkript.GetClass();
        if (ComponentType == null)
        {
            Debug.LogError($"{name}: ComponentType is null.");   // `name` is the SO’s name
            return;
        }
        if (!typeof(MonoBehaviour).IsAssignableFrom(ComponentType))
        {
            Debug.LogError($"{ComponentType} isn’t a MonoBehaviour.");
            return;
        }
        if (!typeof(IActionable).IsAssignableFrom(ComponentType))
        {
            Debug.LogError($"{ComponentType} doesn’t implement IActionable.");
            return;
        }

        //player.TryGetComponent(ComponentType, out Component action);

        //if (action == null)
        //{
        Component action = player.AddComponent(ComponentType);
        //}

        ((IActionable)action).SetUp(player, this, actionEvent);
        return;
    }

}
