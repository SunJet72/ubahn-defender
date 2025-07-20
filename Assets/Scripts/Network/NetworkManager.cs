using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Fusion.Sockets;
using SpacetimeDB.Types;
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
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    #endregion

    public static NetworkManager Instance { get; private set; }

    private NetworkRunner _myrunner;
    [SerializeField]
    private NetworkPrefabRef _playerPrefab;

    [SerializeField]    
    private int inventorySceneIndex;
    [SerializeField]
    private int gameSceneIndex;

    private Transform parentTransform;

    // [Networked]
    // private NetworkDictionary<PlayerRef, NetworkObject> _spawnedPlayers { get; } = new();

    private Color[] colorPool = { Color.blue, Color.yellow, Color.magenta };

    private ScriptableWeapon scriptableWeapon;
    private ScriptableArmor scriptableArmor;
    private PlayerCombatSystemData playerCombatSystemData;
    private List<ScriptableConsumable> consumables;

    private PlayerController playerController;

    private void Awake()
    {
        Instance = this;
    }

    async void StartGame(GameMode mode, string roomName = "TestRoomKirill")
    {

        _myrunner = gameObject.AddComponent<NetworkRunner>();
        _myrunner.ProvideInput = true;

        // string sceneName = "SampleScene";
        // await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // Scene level = SceneManager.GetSceneByName(sceneName);
        // Debug.Log("Scene index: " + level.buildIndex);
        SceneRef scene = SceneRef.FromIndex(gameSceneIndex);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await _myrunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
    {
        Debug.Log("Player Joined!");
        if (runner.IsServer)
        {
            parentTransform = TrainSystem.Instance.transform;
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
                playerCombatSystem.Init(playerCombatSystemData, scriptableArmor, scriptableWeapon, consumables);

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

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData data = new NetworkInputData();

        if (playerController == null)
        {
            NetworkObject playerObj = runner.GetPlayerObject(runner.LocalPlayer);
            if (playerObj == null) return;
            playerController = playerObj.GetComponent<PlayerController>();
            // Debug.LogWarning("NetworkManager.OnInput(): Player has no controller.");
            // return;
        }
        data.moveInput = playerController.MoveInput;

        input.Set(data);
    }

    public void JoinGame(ScriptableWeapon scriptableWeapon, ScriptableArmor scriptableArmor,
        PlayerCombatSystemData playerCombatSystemData, List<ScriptableConsumable> consumables, string roomName = "TestRoom")
    {
        this.scriptableWeapon = scriptableWeapon;
        this.scriptableArmor = scriptableArmor;
        this.playerCombatSystemData = playerCombatSystemData;
        this.consumables = consumables;

        StartGame(GameMode.AutoHostOrClient, roomName);
    }
    // private void OnGUI()
    // {
    //     if (_myrunner == null)
    //     {
    //         if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
    //         {
    //             StartGame(GameMode.Host);
    //         }
    //         if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
    //         {
    //             StartGame(GameMode.Client);
    //         }
    //     }
    // }

    public PlayerCombatSystem GetCurrentPlayer()
    {
        // return _spawnedPlayers.Where(p => p.Key.Equals(Runner.LocalPlayer)).First().Value.GetComponent<PlayerMock>();
        return Runner.GetPlayerObject(Runner.LocalPlayer).GetComponent<PlayerCombatSystem>();
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

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        SceneManager.LoadScene(inventorySceneIndex);
    }
}
