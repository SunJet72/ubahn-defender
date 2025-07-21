using System;
using Fusion;
using TMPro;
using UnityEngine;

public class EctsContainer : NetworkBehaviour
{
    public event Action<EctsContainer> OnDieEvent;
    [SerializeField] private TextMeshProUGUI boxesAmountIndicator;

    [Networked, OnChangedRender(nameof(OnBoxesAmountChanged))]
    public int BoxesAmount{ get; set; }
    [Networked]
    private int previousBoxesAmount { get; set; }

    public override void Spawned()
    {
        OnBoxesAmountChanged();
    }

    private void OnBoxesAmountChanged()
    {
        boxesAmountIndicator.text = "" + BoxesAmount;
        TrainSystem.Instance.BoxesAmountChanged(BoxesAmount - previousBoxesAmount);
        previousBoxesAmount = BoxesAmount;
    }
    public void Steal()
    {
        BoxesAmount--;
        if (BoxesAmount <= 0)
        {
            Debug.Log("I lost all boxes");
            OnDieEvent?.Invoke(this);
            Runner.Despawn(Object);
        }
    }
}
