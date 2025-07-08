using UnityEngine;
using UnityEngine.Events;

public interface IActionable
{
    public void SetUp(GameObject player, ScriptableActionBase action, UnityEvent ActionEvent);
    public void Action();
}
