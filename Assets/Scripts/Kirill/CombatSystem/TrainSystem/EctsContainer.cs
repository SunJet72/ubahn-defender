using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EctsContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI boxesAmountIndicator;
    private int boxesAmount;
    public int BoxesAmount
    {
        get { return boxesAmount; }
        set
        {
            boxesAmount = value;
            boxesAmountIndicator.text = "" + boxesAmount;
        }
    }

    public void Steal()
    {
        BoxesAmount--;
        if (BoxesAmount <= 0)
        {
            Debug.Log("I lost all boxes");
            Destroy(gameObject);
        }
    }
}
