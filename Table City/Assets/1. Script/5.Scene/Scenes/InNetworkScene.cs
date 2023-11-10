using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InNetworkScene : SceneBase
{
    private string _bgmName = "bgm1";
    [SerializeField]
    private GameObject _popup;

    private void Start() { OnLoad(); }

    protected override void OnLoad()
    {
        _scene = gameObject;
        _name = "InNetwork";
        _type = Define.Scene.InNetwork;
        Managers.Scene.OnLoad(this);

        Init();

        Managers.Sound.BgmPlay(_bgmName);

        ShowPanel(Define.Panel.Room);
    }

    public override void Init()
    {
        for (int i = 0; i < panels.Count; i++)
            RegistrationPanel(panels[i]);

        Managers.UI.SetPopup(_popup);
    }

    protected override void InitUI()
    {

    }

    public override void LeftScene() { }

}
