using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainController
{
    public TrainState State { get; private set;}
    public Station LastVisitedStation { get; private set; }

    public int goods;


    public List<Station> route = new List<Station>();

    public TrainController(List<Station> trainRoute, Station currStation)
    {
        route = trainRoute;
        LastVisitedStation = currStation;
    }

    public void StartMoving()
    {
        if (NextStation() == null)
        {
            Debug.LogError("Trying to move from last station");
            return;
        }
        State = TrainState.Moving;
        goods = LastVisitedStation.SendGoods();
    }

    public void ArrriveToNextStation()
    {
        if (State != TrainState.Moving)
        {
            Debug.LogError("You have to start moving, then only arriving, moron.");
            return;
        }
        LastVisitedStation.RecieveGoods(goods);
        LastVisitedStation = NextStation();
        LastVisitedStation.RecieveGoods(goods);
        State = TrainState.Waiting;
    }

    public Station NextStation()
    {
        int lastIndex = route.FindIndex(st => st ==LastVisitedStation);
        if (lastIndex != route.Count-1) {
            return route[lastIndex + 1];
        }
        return null;
    }

    public enum TrainState
    {
        None,
        Moving,
        Waiting
    }

}
