using OculusSampleFramework;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Root;
    public static UIManager UI;
    public static GameManager Game;
    public static InstantiateManager Instance;
    public static SoundManager Sound;
    public static RoomManager Room;
    public static AssetManager Asset;
    public static NetworkManager Network;

    private T Init<T>() where T : ManagerBase
    {
        T manager = Util.GetOrAddComponent<T>(gameObject);
        manager.Init();

        return manager;
    }
    
    private T InitPun<T>() where T : PunManagerBase
    {
        T manager = Util.GetOrAddComponent<T>(gameObject);
        manager.Init();

        return manager;
    }

    public void Init()
    {
        Game = Init<GameManager>();
        UI = Init<UIManager>();
        Instance = Init<InstantiateManager>();
        Sound = Init<SoundManager>();
        Room = InitPun<RoomManager>();
        Asset = InitPun<AssetManager>();
        Network = InitPun<NetworkManager>();
    }
}
