using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ApplicationController : MonoBehaviour
{
    [SerializeField]
    private bool debugMode;

    private void Awake()
    {
        Init();
        DontDestroyOnLoad(gameObject);

        OnManagerInit();
    }

    private void Init()
    {
        Managers.Root = Util.AddOrGetComponent<Managers>(gameObject);
        Managers.Root.Init();
    }

    private void OnManagerInit()
    {
        _behaviour = Define.PlayerBehaviourState.StartApplication;
        InitEvent();
        SpawnLocalPlayer();

        GotoStartScene();
    }

    private void SpawnLocalPlayer()
    {
        Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
        Transform playerPoint = spawnPoint.Find("Spawn_Player").Find("Point_A");
        GameObject source = Resources.Load<GameObject>("0. Player/Player_Prefab");
        GameObject go = Instantiate(source, playerPoint.position, playerPoint.rotation);
        Managers.Spawn(go);
    }

    private void GotoStartScene()
    {
        Managers.Scene.LoadScene(Define.Scene.Title);
    }

    [SerializeField]
    private Define.PlayerBehaviourState _behaviour;

    private void Update()
    {
        if (!debugMode)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Managers.Event.ExcuteCancleButton();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            switch (_behaviour)
            {
                case Define.PlayerBehaviourState.StartApplication:
                    Managers.Event.ExcuteGameStartButton();
                    break;
                case Define.PlayerBehaviourState.Lobby:
                    Managers.Event.ExcuteMatchRoomButton();
                    break;
                case Define.PlayerBehaviourState.InRoom:
                    Managers.Event.ExcuteReadyButton(false);
                    break;
                case Define.PlayerBehaviourState.Ready:
                    Managers.Event.ExcuteReadyButton(true);
                    break;
            }
        }

        // L을 누르면 방 떠나기
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (_behaviour == Define.PlayerBehaviourState.InRoom ||
                _behaviour == Define.PlayerBehaviourState.Ready ||
                _behaviour == Define.PlayerBehaviourState.SelectJob)
            {
                Managers.Event.ExcuteLeaveButton();
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Managers.Event.ExcuteJobButton(true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Managers.Event.ExcuteJobButton(false);
        }
    }

    private void InitEvent()
    {
        Managers.Event.AddGameStartButton(GameStartButton);
        Managers.Event.AddMatchRoomButton(MatchRoom);
        Managers.Event.AddLeaveButton(LeaveButton);
        Managers.Event.AddReadyButton(ReadyButton);
        Managers.Event.AddAllReady(SelectJob);
    }

    private void GameStartButton()
    {
        Debug.Log("Start Game");
        _behaviour = Define.PlayerBehaviourState.Lobby;
    }

    private void MatchRoom()
    {
        if (Managers.Network.IsCanCreateRoom())
        {
            Debug.Log("Start Match");
            _behaviour = Define.PlayerBehaviourState.InRoom;
        }
        else
        {
            Debug.Log("Can't Start Match");
        }
    }
    private void LeaveButton()
    {
        Debug.Log("LeaveRoom");
        _behaviour = Define.PlayerBehaviourState.Lobby;
    }

    private void ReadyButton(bool trigger)
    {
        if(!trigger)
        {
            _behaviour = Define.PlayerBehaviourState.Ready;
            Debug.Log("Ready");
        }
    }

    private void SelectJob(bool isSolo)
    {
        _behaviour = Define.PlayerBehaviourState.SelectJob;
    }

}
