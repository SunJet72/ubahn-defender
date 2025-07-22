using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class TrainSystem : NetworkBehaviour
{
    public static TrainSystem Instance { get; private set; }
    [SerializeField] private int playerAmount; // Has to be determined by Server

    [SerializeField] private const int BOXES_PRO_PLAYER = 2;
    [SerializeField] private const float LENGTH_PRO_CONTAINER = 13.75f;
    [SerializeField] private const float START_TRAIN_LENGTH = 13f;

    [SerializeField] private GameObject _sequencePrefab; // Length = 13.75
    [SerializeField] private GameObject _trainHeadPrefab; // Length = 14

    [Networked]
    public int TotalBoxesAmount { get; private set; }
    [Networked]
    public int MaxBoxesAmount { get; set; }

    private float trainLength;

    [Networked]
    private int containersAmount { get; set; }
    private EctsContainer[] containers;
    private TrainPart[] sequences;

    private System.Random rand;

    public override void Spawned()
    {
        Instance = this;
        Debug.Log("Train spawned");
        if (Runner.IsServer)
        {
            Debug.Log("Runner is server");
            //rand = new System.Random();
            //DetermineTrainParameters();
        }
    }

    public void Setup()
    {
        if (Runner.IsServer)
        {
            Debug.Log("I am setuping train");
            rand = new System.Random();
            DetermineTrainParameters();
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
        containers = new EctsContainer[containersAmount];
        sequences = new TrainPart[containersAmount];

        for (int i = 0; i < containersAmount; i++)
        {
            NetworkObject sequenceGO = Runner.Spawn(_sequencePrefab, onBeforeSpawned: (runner, spawned) =>
            {
                spawned.transform.parent = transform;
                spawned.transform.localPosition = new Vector2(0, -START_TRAIN_LENGTH - LENGTH_PRO_CONTAINER * i);
                sequences[i] = spawned.GetComponent<TrainPart>();
                containers[i] = sequences[i].GetEctsContainer();
                containers[i].BoxesAmount = BOXES_PRO_PLAYER * 3;
                MaxBoxesAmount += containers[i].BoxesAmount;
            });
            /*NetworkObject containerGO = Runner.Spawn(ectsContainerPrefab, onBeforeSpawned: (runner, spawned) =>
            {
                spawned.transform.parent = transform;
                spawned.transform.position = transform.position - new Vector3(0, trainLength / 2)
                 + new Vector3(0, _distanceBetweenContainers * (i + 1));
                containers[i] = spawned.GetComponent<EctsContainer>();
                containers[i].BoxesAmount = BOXES_PRO_PLAYER * 3;
                MaxBoxesAmount += containers[i].BoxesAmount;
            });*/
        }

        for (int i = 0; i < 2 - ((playerAmount - 1) % 3); i++)
        {
            int id = rand.Next(containersAmount);
            containers[id].BoxesAmount -= BOXES_PRO_PLAYER;
            MaxBoxesAmount -= BOXES_PRO_PLAYER;
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
        TotalBoxesAmount += deltaAmount;
        UIEvents.HealthChanged(TotalBoxesAmount, MaxBoxesAmount);
    }

    public Transform ReturnAnyAbordagePoint(Transform enemyTransform)
    {
        //return transform;
        //TODO: randomly choose am abordage point. Dont forget sides.
        int id = Random.Range(0, containersAmount);
        if (enemyTransform.position.x > transform.position.x)
            return sequences[id].GetRandomAbordagePoints(false);
        else
            return sequences[id].GetRandomAbordagePoints(true);
    }
}
