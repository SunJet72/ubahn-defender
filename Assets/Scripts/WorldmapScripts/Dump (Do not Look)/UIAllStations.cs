using UnityEngine;
using TMPro;
using System.Text;

public class UIAllStations : MonoBehaviour
{
    TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        StringBuilder str = new StringBuilder();
        str.Append("All Stations: \n");
        foreach (Station station in WorldMapController.instance.map)
        {
            str.Append(station);
        }
        text.text = str.ToString();
    }
}
