using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Root;
    public static GameManager Game;
    public static UIManager UI;
    public static AssetManager Asset;
    public static InstanceManager Instance;
    public static SoundManager Sound;
    public static NetworkManager Network;
    public static SceneManager Scene;
    public static EventManager Event;

    private T Init<T>() where T : ManagerBase
    {
        T manager = null;
        manager = Util.AddOrGetComponent<T>(this.gameObject);
        if (manager != null)
        {
            manager.Init();
        }

        return manager;
    }

    private T InitPun<T>() where T : PunManagerBase
    {
        T manager = null;
        manager = Util.AddOrGetComponent<T>(gameObject);
        if (manager != null)
        {
            manager.Init();
        }

        return manager;
    }

    public void Init()
    {
        Event = Init<EventManager>();
        Network = InitPun<NetworkManager>();
        Scene = Init<SceneManager>();
        Game = Init<GameManager>();
        UI = Init<UIManager>();
        Instance = Init<InstanceManager>();
        Sound = Init<SoundManager>();
        Asset = InitPun<AssetManager>();
    }
}
