using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EctsContainer : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI boxesAmountIndicator;

    [Networked, OnChangedRender(nameof(OnBoxesAmountChanged))]
    public int BoxesAmount{ get; set; }

    public override void Spawned()
    {
        OnBoxesAmountChanged();
    }

    private void OnBoxesAmountChanged()
    {
        boxesAmountIndicator.text = "" + BoxesAmount;
    }
    public void Steal()
    {
        BoxesAmount--;
        if (BoxesAmount <= 0)
        {
            Debug.Log("I lost all boxes");
            Runner.Despawn(Object);
        }
    }
}
