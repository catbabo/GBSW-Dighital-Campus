using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnNetworkScene : SceneBase
{
    private string _bgmName = "bgm1";
    [SerializeField]
    private GameObject _popup;

    private void Start() { OnLoad(); }

    protected override void OnLoad()
    {
        _scene = gameObject;
        _name  = "UnNetwork";
        _type = Define.Scene.UnNetwork;
        Managers.Scene.OnLoad(this);

        Init();

        Managers.Sound.BgmPlay(_bgmName);
        SpawnLocalPlayer();
        
        ShowPanel(Define.Panel.Title);
    }

    public override void Init()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            RegistrationPanel(panels[i]);
        }

        Managers.UI.SetPopup(_popup);
    }

    protected override void InitUI()
    {

    }

    private void SpawnLocalPlayer()
    {
        Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
        Transform playerPoint = spawnPoint.Find("Spawn_Player").Find("Point_A");
        GameObject source = Resources.Load<GameObject>("0. Player/Player_Prefab");
        GameObject go = Instantiate(source, playerPoint.position, playerPoint.rotation);
        //Managers.Spawn(go);
    }

    public override void LeftScene() { }
}
