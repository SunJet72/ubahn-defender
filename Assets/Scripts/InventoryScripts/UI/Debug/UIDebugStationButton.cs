using UnityEngine;
using UnityEngine.UI;

public class UIDebugStationButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] bool isArriving;
    void Start()
    {
        if (isArriving)
        {
            GetComponent<Button>().onClick.AddListener(WorldMapController.instance.SetTrueTemp);
        }
        else
        {
            GetComponent<Button>().onClick.AddListener(WorldMapController.instance.SetFalseTemp);
        }
    }
}
