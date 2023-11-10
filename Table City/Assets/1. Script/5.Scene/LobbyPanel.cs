using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanel : PanelBase
{
    private SetRoomCode roomCode;
    private SetNickName nickName;
    private Transform playerPoint;

    public override void Init()
    {
        _panel = gameObject;
        _type = Define.Panel.Lobby;

        InitUI();
        InitEvent();
        OnShow();
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
        if (Managers.Scene.IsInitable(Define.Scene.UnNetwork, _type))
            Managers.Event.AddMatchRoomButton(MatchRoomButton);
    }

    public override void OnShow() { }

    private void MatchRoomButton()
    {
        if (!Managers.Network.IsCanCreateRoom())
        {
            Managers.UI.ShowPopup("Warning", "Nothing Enter");
        }
        else
        {
            Managers.Scene.LoadScene(Define.Scene.InNetwork);
        }
    }
}