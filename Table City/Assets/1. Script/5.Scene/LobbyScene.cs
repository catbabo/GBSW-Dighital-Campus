using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : SceneBase
{
    private SetRoomCode roomCode;
    private SetNickName nickName;
    private Transform playerPoint;

    public override void Init()
    {
        _scene = gameObject;
        _type = Define.Scene.Lobby;
        _name = "Lobby";

        InitUI();
        InitEvent();
    }

    protected override void InitUI()
    {
        roomCode = transform.Find("RoomCode").GetComponent<SetRoomCode>();
        nickName = transform.Find("NickName").GetComponent<SetNickName>();

        roomCode.Init();
        nickName.Init();
    }

    private void InitEvent()
    {
        Managers.Event.AddMatchRoomButton(MatchRoomButton);
    }

    public override void StartLoad() { OnLoad(); }

    protected override void OnLoad()
    {
        if(Managers.player == null) { SpawnPlayer(); }
    }

    private void SpawnPlayer()
    {
        Debug.Log("jjscjj");
        Managers.player.Destroy();
        Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
        Transform playerPoint = spawnPoint.Find("Spawn_Player").Find("Point_A");
        GameObject source = Resources.Load<GameObject>("0. Player/Player_Prefab");
        GameObject go = Instantiate(source, playerPoint.position, playerPoint.rotation);
        Managers.player = go.GetComponent<PlayerController>();
    }

    private void MatchRoomButton()
    {
        if (!Managers.Network.IsCanCreateRoom())
        {
            Managers.UI.ShowPopup("Warning", "Nothing Enter");
        }
        else
        {
            Managers.Scene.LoadScene(Define.Scene.Room);
        }
    }
}