using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

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
        GotoStartScene();
    }

    private void GotoStartScene()
    {
        Managers.Scene.LoadScene(Define.Scene.Title);
    }

    private Define.PlayerBehaviourState _behaviour;

    private void Update()
    {
        if (!debugMode)
            return;

        if(Input.GetKeyDown(KeyCode.N))
        {
            switch(_behaviour)
            {
                case Define.PlayerBehaviourState.StartApplication:
                    Managers.Event.ExcuteOnGameStart();
                    break;
                case Define.PlayerBehaviourState.Title:
                    Managers.Event.ExcuteMatchRoomButton();
                    break;
            }
        }


        // B�� ������ �κ� ����
        if (Input.GetKeyDown(KeyCode.B)) Managers.Network.JoinLobby();

        // C�� ������ ������ ������ ����
        if (Input.GetKeyDown(KeyCode.C)) Managers.Network.Connect();

        // D�� ������ ���� ���� ����
        if (Input.GetKeyDown(KeyCode.D)) Managers.Network.DisConnect();

        // I�� ������ ���� ���� ȣ��
        if (Input.GetKeyDown(KeyCode.I)) Managers.Network.Info();

        // J�� ������ �� ����
        if (Input.GetKeyDown(KeyCode.J)) Managers.Network.JoinOrCreate();

        // L�� ������ �� ������
        if (Input.GetKeyDown(KeyCode.L)) Managers.Network.LeaveRoom();

        // S�� ������ �⺻ ����
        if (Input.GetKeyDown(KeyCode.S)) Managers.Network.EnterRoomSolo();

        // T�� ������ ȥ�ڼ� ����
        if (Input.GetKeyDown(KeyCode.T)) PhotonNetwork.LoadLevel("PlayRoom");
    }

    private void InitEvent()
    {
        Managers.Event.AddOnGameStart(OnGameStart);
        Managers.Event.AddMatchRoomButton(MatchRoom);
    }

    private void OnGameStart()
    {
        _behaviour = Define.PlayerBehaviourState.Title;
    }

    private void MatchRoom()
    {
        if(Managers.Network.IsCanCreateRoom())
        {
            Debug.Log("Start Match");
            _behaviour = Define.PlayerBehaviourState.CreateLobby;
        }
        else
        {
            Debug.Log("Can't Start Match");
        }
    }
}
