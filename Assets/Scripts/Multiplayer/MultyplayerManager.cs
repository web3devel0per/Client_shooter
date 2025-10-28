using Colyseus;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MultyplayerManager : ColyseusManager<MultyplayerManager>
{

    [SerializeField] private PlayerCharecter _player;
    [SerializeField] private EnemyController _enemy;

    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

    private ColyseusRoom<State> _room;
    protected override void Awake()
    {
        base.Awake();

        Instance.InitializeClient();
        Connect();
       
    }

    private async void Connect()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"speed",  _player._speed},
            {"hp",  _player.maxHealth}
        };

        _room = await Instance.client.JoinOrCreate<State>("state_handler", data);

        _room.OnStateChange += OnChange;
        _room.OnMessage<string>("Shoot", ApplyShoot);
    }

    private void ApplyShoot(string jsonShootInfo)
    {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);

        if (_enemies.ContainsKey(shootInfo.key) == false)
        {
            Debug.LogError("����� ���, � �� ������� ��������");
            return;
        }

        _enemies[shootInfo.key].Shoot(shootInfo);
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (isFirstState == false) return;

        var player = state.players[_room.SessionId];
        

        state.players.ForEach((key, player) =>
        {
            if (key == _room.SessionId) CreatePlayer(player);
            else CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;
    }

    private void RemoveEnemy(string key, Player value)
    {
        if (_enemies.ContainsKey(key) == false) return;

        var enemy = _enemies[key];
        enemy.Destroy();

        _enemies.Remove(key);
    }

    private void CreateEnemy(string key, Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);
        var enemy = Instantiate(_enemy, position, Quaternion.identity);
        enemy.Init(key, player);
        _enemies.Add(key, enemy);
    }

    private void CreatePlayer(Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);
        player.OnChange += Instantiate(_player, position, Quaternion.identity).Onchange;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _room.Leave();
    }

   public void SendMessageToServer(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }

    public void SendMessageToServer(string key, string data)
    {
        _room.Send(key, data);
    }

    public string GetSessionId() => _room.SessionId;

}
