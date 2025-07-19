using System.Collections;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isReady = false;
    [SerializeField] private float locationRefreshTime;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
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
        //....
    }

    public void GetUnready()
    {
        isReady = false;
        //...
    }
}
