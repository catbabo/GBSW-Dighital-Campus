using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static GameManager Game { get; set; }
    public static UIManager UI { get; private set; }
    public static InstanceManager Instance { get; private set; }
    public static SoundManager Sound { get; private set; }
    public static NetworkManager Network { get; private set; }


    private void Awake()
    {
        if(Game == null)
        {
            Game = gameObject.AddComponent<GameManager>();
            UI = gameObject.AddComponent<UIManager>();
            Instance = gameObject.AddComponent<InstanceManager>();
            Instance.Init();

            Sound = gameObject.AddComponent<SoundManager>();
            Sound.Init();

            Network = gameObject.AddComponent<NetworkManager>();
            Network.Init();

        }
        else
        {
            if(Game.gameObject != this.gameObject)
                Destroy(this.gameObject);
        }
    }
}
