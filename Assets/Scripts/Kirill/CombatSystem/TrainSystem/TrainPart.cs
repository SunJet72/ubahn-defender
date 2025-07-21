using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Networking;

public class TrainPart : NetworkBehaviour
{
    [SerializeField] private List<Transform> abordagePoints;
    [SerializeField] private EctsContainer container;

    public Transform GetRandomAbordagePoints(bool isLeft) // ! Build so, that all left points have gerade id, and right ungerade.
    {
        int id = Random.Range(0, abordagePoints.Count / 2);
        if (isLeft)
        {
            return abordagePoints[2 * id];
        }
        else
        {
            return abordagePoints[2 * id + 1];
        }
    }

    public EctsContainer GetEctsContainer()
    {
        return container;
    }
}
