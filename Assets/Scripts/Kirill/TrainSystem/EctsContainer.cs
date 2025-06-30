using UnityEngine;
using UnityEngine.AI;

public class EctsContainer : MonoBehaviour
{
    [SerializeField] private int boxesAmount;

    public void Steal()
    {
        boxesAmount--;
        if (boxesAmount <= 0)
        {
            Debug.Log("I lost all boxes");
            Destroy(gameObject);
        }
    }
}
