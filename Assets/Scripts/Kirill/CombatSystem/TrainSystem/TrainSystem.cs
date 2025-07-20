using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class TrainSystem : NetworkBehaviour
{
    [SerializeField] private int playerAmount; // Has to be determined by Server

    [SerializeField] private const int BOXES_PRO_PLAYER = 2;
    [SerializeField] private const float LENGTH_PRO_CONTAINER = 3;
    [SerializeField] private const float START_TRAIN_LENGTH = 5;
    [SerializeField] private GameObject ectsContainerPrefab;

    [Networked]
    private int totalBoxesAmount { get; set; }
    [Networked]
    private int maxBoxesAmount { get; set; }

    private float trainLength;

    [Networked]
    private int containersAmount { get; set; }
    private EctsContainer[] containers;

    private System.Random rand;

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            rand = new System.Random();
            DetermineTrainParameters();
        }
    }

    public void Setup()
    {
        if (Runner.IsServer)
        {
            DetermineAndPlaceContainers();
        }
    }

    private void DetermineTrainParameters() // just watch description on the miro board
    {
        containersAmount = (playerAmount - 1) / 3 + 1;
        trainLength = LENGTH_PRO_CONTAINER * containersAmount + START_TRAIN_LENGTH;
    }

    private void DetermineAndPlaceContainers() // Some math principles used, dont think too much on that
    {
        float _distanceBetweenContainers = trainLength / (containersAmount + 1);
        containers = new EctsContainer[containersAmount];

        for (int i = 0; i < containersAmount; i++)
        {
            NetworkObject containerGO = Runner.Spawn(ectsContainerPrefab, onBeforeSpawned: (runner, spawned) =>
            {
                spawned.transform.parent = transform;
                spawned.transform.position = transform.position - new Vector3(0, trainLength / 2)
                 + new Vector3(0, _distanceBetweenContainers * (i + 1));
                containers[i] = spawned.GetComponent<EctsContainer>();
                containers[i].BoxesAmount = BOXES_PRO_PLAYER * 3;
                maxBoxesAmount += containers[i].BoxesAmount;
                containers[i].trainSystem = this;
            });
        }

        for (int i = 0; i < 2 - ((playerAmount - 1) % 3); i++)
        {
            int id = rand.Next(containersAmount);
            containers[id].BoxesAmount -= BOXES_PRO_PLAYER;
            maxBoxesAmount -= BOXES_PRO_PLAYER;
        }
    }

    public EctsContainer ReturnNearestContainer(Transform enemyTransform)
    {
        float distance = float.MaxValue;
        EctsContainer containerToReturn = null;
        for (int i = 0; i < containersAmount; i++)
        {
            if (containers[i] == null)
                continue;
            if ((containers[i].transform.position - enemyTransform.position).magnitude < distance)
            {
                distance = (containers[i].transform.position - enemyTransform.position).magnitude;
                containerToReturn = containers[i];
            }
        }
        return containerToReturn;
    }

    public void BoxesAmountChanged(int deltaAmount)
    {
        totalBoxesAmount += deltaAmount;
        UIEvents.HealthChanged(totalBoxesAmount, maxBoxesAmount);
    }
}
