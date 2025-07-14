using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using UnityEngine;



public class Station
{
    public int Id { get; }
    public string StationName { get; }
    public string Description { get; }
    public float Wealth { get; set; }
    public List<Station> Neighbours { get; }

    public Station(StationObj obj)
    {
        Neighbours = new List<Station>();
        Id = obj.id;
        StationName = obj.stationName;
        Description = obj.description;
    }

    public float SendGoods()
    {
        return Wealth / 10;
    }

    public void RecieveGoods(float recievedGoods)
    {
        Wealth += recievedGoods;
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append("Station: ").Append(StationName).Append("\n    Description: ").Append(Description).Append("\n    Wealth: ").Append(Wealth).Append("\n    Neighbours: \n");
        foreach (Station station in Neighbours)
        {
            str.Append("        ").Append(station.StationName).Append("\n").Append("\n");
        }
        return str.ToString();
    }

}
