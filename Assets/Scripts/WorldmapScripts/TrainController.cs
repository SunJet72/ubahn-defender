using UnityEngine;

public class TrainController : MonoBehaviour
{
    public TrainState State { get; }
    public enum TrainState
    {
        None,
        Moving,
        Waiting
    }

}
