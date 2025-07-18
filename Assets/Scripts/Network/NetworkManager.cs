using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : SimulationBehaviour, INetworkRunnerCallbacks
{
    #region Empty Callbacks
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    #endregion

    private NetworkRunner _runner;
    [SerializeField]
    private NetworkPrefabRef _playerPrefab;
    private Transform parentTransform;

    private Dictionary<PlayerRef, NetworkObject> _spawnedPlayers = new Dictionary<PlayerRef, NetworkObject>();

    private Color[] colorPool = { Color.blue, Color.yellow, Color.magenta };

    async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // string sceneName = "SampleScene";
        // await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // Scene level = SceneManager.GetSceneByName(sceneName);
        // Debug.Log("Scene index: " + level.buildIndex);
        SceneRef scene = SceneRef.FromIndex(1);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        
    }


    
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player Joined!");
        if (runner.IsServer)
        {
            parentTransform = GameObject.FindGameObjectsWithTag("Train").First().transform;
            int size = _spawnedPlayers.Count;
            NetworkObject playerObj = runner.Spawn(_playerPrefab, new Vector3(0, 9 - (size * 3), 0), Quaternion.identity, player,
            onBeforeSpawned: (runner, spawned) =>
            {
                Color color = colorPool[size % colorPool.Length];
                Player player = spawned.GetComponent<Player>();
                player.SpriteColor = color;
                spawned.transform.SetParent(parentTransform, false);
                spawned.transform.localScale = Vector3.one * 2;
            });

            _spawnedPlayers.Add(player, playerObj);
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    { 
        if (_spawnedPlayers.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedPlayers.Remove(player);
        }
    }

    private bool _xPressed;
    private bool _cPressed;

    private void Update()
    {
        _xPressed = _xPressed || Input.GetKey(KeyCode.X);
        _cPressed = _cPressed || Input.GetKey(KeyCode.C);
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        data.Buttons.Set(NetworkInputData.X, _xPressed);
        data.Buttons.Set(NetworkInputData.C, _cPressed);

        _xPressed = false;
        _cPressed = false;

        input.Set(data);
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

}
