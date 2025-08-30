using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isReady = false;


    public Station nextStation;
    public string nickname { get; private set; }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartGame()
    {

    }

    public void StopGame()
    {

    }

    public void GetReady()
    {
        isReady = true;
        //.... mock shit
        PlayerInventory inventory = PlayerInventory.instance;
        Debug.Log("Starting train from " + WorldMapController.instance.currentStation.StationName + " to " + nextStation.StationName);
        SpacetimeDBController.instance.StartTrain(WorldMapController.instance.currentStation, nextStation);
        NetworkManager.Instance.JoinGame(inventory.GetCurrentWeapon(), inventory.GetCurrentArmor(), PlayerInventory.instance.GetPlayerCombatSystemData(),new List<ScriptableConsumable>());
    }
    public void GetUnready()
    {
        isReady = false;
        //...
    }

    public void LogIn(string nickname)
    {
        Debug.Log("Logging in");
        this.nickname = nickname; 
        GetComponent<SpacetimeDBController>().initDB(nickname);
        WorldMapController.instance.StartChecking();
        SceneManager.LoadScene(1);
    }
}
