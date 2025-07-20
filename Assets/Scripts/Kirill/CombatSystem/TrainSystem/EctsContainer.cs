using Fusion;
using TMPro;
using UnityEngine;

public class EctsContainer : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI boxesAmountIndicator;

    [Networked, OnChangedRender(nameof(OnBoxesAmountChanged))]
    public int BoxesAmount{ get; set; }
    [Networked]
    private int previousBoxesAmount { get; set; }


    public TrainSystem trainSystem;

    public override void Spawned()
    {
        OnBoxesAmountChanged();
    }

    private void OnBoxesAmountChanged()
    {
        boxesAmountIndicator.text = "" + BoxesAmount;
        trainSystem.BoxesAmountChanged(BoxesAmount - previousBoxesAmount);
        previousBoxesAmount = BoxesAmount;
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
