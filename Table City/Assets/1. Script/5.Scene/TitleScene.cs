using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : SceneBase
{
    private string _bgmName = "bgm1";

    public override void Init()
    {
        _scene = gameObject;
        _type = Define.Scene.Title;
        _name = "Title";
        InitUI();
        InitEvent();
    }

    private void InitEvent()
    {
        Managers.Event.AddOnGameStart(OnGameStart);
    }

    public void OnGameStart()
    {
        Managers.Scene.LoadScene(Define.Scene.Lobby);
    }

    public override void StartLoad()
    {
        OnLoad();
    }

    protected override void OnLoad()
    {
        Managers.Sound.BgmPlay(_bgmName);
    }
}
