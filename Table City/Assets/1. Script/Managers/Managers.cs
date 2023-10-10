using OculusSampleFramework;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static FindManager _find;
    public static UIManager _ui;
    public static GameManager _game;
    public static InstantiateManager _inst;
    public static SoundManager _sound;
    public static RoomManager _room;
    public static AssetManager _asset;
    public static LobbyManager _lobby;
    public static NetworkManager _network;

    private T Init<T>() where T : ManagerBase
    {
        T manager = GetComponent<T>();
        if (manager == null)
        {
            manager = gameObject.AddComponent<T>();
        }
        manager.Init();

        return manager;
    }
    
    private T InitPun<T>() where T : PunManagerBase
    {
        T manager = GetComponent<T>();
        if (manager == null)
        {
            manager = gameObject.AddComponent<T>();
        }
        manager.Init();

        return manager;
    }

    private void Awake()
    {
        _find = Init<FindManager>();
        _ui = InitPun<UIManager>();
        _game = Init<GameManager>();
        _inst = Init<InstantiateManager>();
        _sound = Init<SoundManager>();
        _room = InitPun<RoomManager>();
        _asset = InitPun<AssetManager>();
        _lobby = InitPun<LobbyManager>();
        _network = InitPun<NetworkManager>();

        DontDestroyOnLoad(this.gameObject);
    }
}
