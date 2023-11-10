using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanel : PanelBase
{
    public override void Init()
    {
        _panel = gameObject;
        _type = Define.Panel.Title;
        InitUI();
        InitEvent();
    }

    private void InitEvent()
    {
        if(Managers.Scene.IsInitable(Define.Scene.UnNetwork, _type))
            Managers.Event.AddGameStartButton(OnGameStart);
    }

    public void OnGameStart()
    {
        Managers.Scene.ShowPanel(Define.Panel.Lobby);
    }

    public override void OnShow() { }

    public override void LeftPanel()
    {
        base.LeftPanel();
    }
}
