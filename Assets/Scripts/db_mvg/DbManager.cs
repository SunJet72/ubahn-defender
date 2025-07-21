using UnityEngine;
using System;
using System.Threading.Tasks;
using SpacetimeDB;
using SpacetimeDB.Types;
using System.Collections.Generic;
using System.Linq;

public class DbManager
{
    private string player_id;

    public static DbManager Instance { get; private set; }

    private TaskCompletionSource<bool> connectionReady = new TaskCompletionSource<bool>();

    public DbManager(string player_name)
    {
        player_id = player_name;
        Application.targetFrameRate = 60;

        var builder = DbConnection.Builder()
            .OnConnect(HandleConnect)
            .OnConnectError(HandleConnectError)
            .OnDisconnect(HandleDisconnect)
            .WithUri(SERVER_URL)
            .WithModuleName(MODULE_NAME);

        if (PlayerPrefs.HasKey(AuthToken.Token))
        {
            builder = builder.WithToken(AuthToken.Token);
        }

        Conn = builder.Build();
        Instance = this;
    }

    const string SERVER_URL = "https://ubahn.sunjet-project.de";
    //const string SERVER_URL = "http://127.0.0.1:3000";
    const string MODULE_NAME = "spacetime-db-service";

    public static Identity LocalIdentity { get; private set; }
    public static DbConnection Conn { get; private set; }
    public event Action<Train> OnTrainUpdated;

    private void HandleConnect(DbConnection conn, Identity identity, string token)
    {
        Debug.Log("[GameManager] Connected to SpacetimeDB.");
        AuthToken.SaveToken(token);
        LocalIdentity = identity;

        Conn.SubscriptionBuilder()
            .OnApplied(HandleSubscriptionApplied)
            .SubscribeToAllTables();

        Conn.Db.Train.OnUpdate += (EventContext ctx, Train oldTrain, Train newTrain) =>
        {
            Debug.Log($"[Train Updated] ID: {newTrain.Id}");
            OnTrainUpdated?.Invoke(newTrain);
        };

        Conn.Reducers.Login(player_id);
    }

    private void HandleSubscriptionApplied(SubscriptionEventContext ctx)
    {
        Debug.Log("Subscription applied!");
        connectionReady.TrySetResult(true);
    }

    private void HandleConnectError(Exception ex)
    {
        Debug.LogError($"[GameManager] Connection error: {ex}");
    }

    private void HandleDisconnect(DbConnection conn, Exception ex)
    {
        Debug.LogWarning("[GameManager] Disconnected.");
        if (ex != null)
        {
            Debug.LogException(ex);
        }
    }

    private async Task WaitUntilReady()
    {
        await connectionReady.Task;
    }

    ////////////////////////////////////////////////////////
    /// ASYNC METHODS

    public async Task enter_the_train(string from, string to)
    {
        await WaitUntilReady();
        uint station_from_money = Conn.Db.Station.MvgId.Find(from).Money;
        Conn.Reducers.EnterTheTrain(from + to, from, to, player_id, station_from_money);
    }

    public async Task leave_the_train(string from, string to)
    {
        await WaitUntilReady();
        Conn.Reducers.LeaveTheTrain(from + to, player_id);
    }

    public async Task<uint> get_armor_id()
    {
        await WaitUntilReady();
        var player = Conn.Db.Player.PlayerId.Find(player_id);
        return player?.ArmorId ?? 0;
    }

    public async Task set_armor(uint armor_id)
    {
        await WaitUntilReady();
        Conn.Reducers.EquipArmor(armor_id, player_id);
    }

    public async Task<uint> get_player_money()
    {
        await WaitUntilReady();
        var player = Conn.Db.Player.PlayerId.Find(player_id);
        return player?.Money ?? 0;
    }

    public async Task<List<SpacetimeDB.Types.Station>> get_stations()
    {
        await WaitUntilReady();
        var localStations = Conn.Db.Station.Iter().ToList();
        return localStations;
    }

    public async Task set_player_money(uint _money)
    {
        await WaitUntilReady();
        Conn.Reducers.SetPlayerMoney(player_id, _money);
    }

    public async Task<uint> get_weapon_id()
    {
        await WaitUntilReady();
        var player = Conn.Db.Player.PlayerId.Find(player_id);
        return player?.WeaponId ?? 0;
    }

    public async Task set_weapon(uint weapon_id)
    {
        await WaitUntilReady();
        Conn.Reducers.EquipWeapon(weapon_id, player_id);
    }

    public async Task<List<Item>> get_inventory()
    {
        await WaitUntilReady();
        var player = Conn.Db.Player.PlayerId.Find(player_id);
        return player?.Items ?? new List<Item>();
    }

    public async Task set_inventory(List<Item> items)
    {
        await WaitUntilReady();
        Conn.Reducers.SetInventoryToPlayer(player_id, items);
    }

    public async Task<List<Item>> get_consumables()
    {
        await WaitUntilReady();
        var player = Conn.Db.Player.PlayerId.Find(player_id);
        return player?.Consumables ?? new List<Item>();
    }

    public async Task set_consumables(List<Item> items)
    {
        await WaitUntilReady();
        Conn.Reducers.SetConsumablesToPlayer(player_id, items);
    }

    public async Task<string> get_player_class()
    {
        await WaitUntilReady();
        var player = Conn.Db.Player.PlayerId.Find(player_id);
        return player?.ClassName ?? "Unknown";
    }
    public async Task set_player_class(string class_name)
    {
        await WaitUntilReady();
        Conn.Reducers.SetPlayerClass(player_id, class_name);
    }
    public async Task set_train_started(string from, string to, bool _started)
    {
        await WaitUntilReady();
        Conn.Reducers.SetTrainStarted(from, to, _started);
    }
    public async Task add_prize_and_leave_the_train(string train_id, float ratio)
    {
        await WaitUntilReady();
        uint player_money_value = await get_player_money();
        uint train_money_value = Conn.Db.Train.Id.Find(train_id).Money;
        player_money_value += (uint)(train_money_value * ratio);
        await set_player_money(player_money_value);
        await leave_the_train(train_id, "");
    }
}
