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

    public static NetworkManager Instance { get; private set; }

    private NetworkRunner _myrunner;
    [SerializeField]
    private NetworkPrefabRef _playerPrefab;
    private Transform parentTransform;

    // [Networked]
    // private NetworkDictionary<PlayerRef, NetworkObject> _spawnedPlayers { get; } = new();

    private Color[] colorPool = { Color.blue, Color.yellow, Color.magenta };

    [SerializeField] private ScriptableWeapon scriptableWeaponBuffer;
    [SerializeField] private ScriptableArmor scriptableArmorBuffer;
    [SerializeField] private List<ScriptableConsumable> consumables;

    async void StartGame(GameMode mode)
    {
        Instance = this;

        _myrunner = gameObject.AddComponent<NetworkRunner>();
        _myrunner.ProvideInput = true;

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

        await _myrunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
    {
        Debug.Log("Player Joined!");
        if (runner.IsServer)
        {
            parentTransform = GameObject.FindGameObjectsWithTag("Train").First().transform;
            int size = runner.ActivePlayers.Count();
            NetworkObject playerObj = runner.Spawn(_playerPrefab, new Vector3(0, 9 - (size * 3), 0), Quaternion.identity, playerRef,
            onBeforeSpawned: (runner, spawned) =>
            {
                Color color = colorPool[size % colorPool.Length];
                PlayerMock player = spawned.GetComponent<PlayerMock>();
                player.SpriteColor = color;
                spawned.transform.SetParent(parentTransform, false);
                spawned.transform.localScale = Vector3.one * 2;

                PlayerCombatSystem playerCombatSystem = spawned.GetComponent<PlayerCombatSystem>();
                playerCombatSystem.Init(scriptableArmorBuffer, scriptableWeaponBuffer, consumables);
            });

            // _spawnedPlayers.Add(playerRef, playerObj);
            runner.SetPlayerObject(playerRef, playerObj);
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        NetworkObject obj = runner.GetPlayerObject(player);
        if (obj != null)
        {
            runner.Despawn(obj);
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
        if (_myrunner == null)
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

    public PlayerMock GetCurrentPlayer()
    {
        // return _spawnedPlayers.Where(p => p.Key.Equals(Runner.LocalPlayer)).First().Value.GetComponent<PlayerMock>();
        return Runner.GetPlayerObject(Runner.LocalPlayer).GetComponent<PlayerMock>();
    }

    public ActiveSpell[] GetCurrentPlayerActiveSpells()
    {
        // return _spawnedPlayers.Where(p => p.Key.Equals(Runner.LocalPlayer)).First().Value.GetComponentsInChildren<ActiveSpell>();
        return Runner.GetPlayerObject(Runner.LocalPlayer).GetComponentsInChildren<ActiveSpell>();
    }

    public List<NetworkObject> GetPlayerObjects()
    {
        return Runner.ActivePlayers.Select(p => Runner.GetPlayerObject(p)).ToList();
    }

    public bool AnyPlayerSpawned()
    {
        // return _spawnedPlayers.Count > 0;
        return Runner.ActivePlayers.Count() > 0;
    }
}
