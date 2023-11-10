using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScene : SceneBase
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        _scene = gameObject;
        _type = Define.Scene.Load;
        _name = "Load";
        InitPanel();
        InitUI();
        OnLoad();
    }

    protected override void InitPanel()
    {
        panels[0].gameObject.SetActive(true);
    }

    protected override void InitUI()
    {

    }

    protected override void OnLoad()
    {
        Managers.Scene.OnLoad(this);
        SpawnPlayer();
        panels[0].Init();
    }

    private void SpawnPlayer()
    {
        //Managers.player.Destroy();
        Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
        Transform playerPoint = spawnPoint.Find("Spawn_Player").Find("Point_A");
        GameObject source = Resources.Load<GameObject>("0. Player/Player_Prefab");
        GameObject go = Instantiate(source, playerPoint.position, playerPoint.rotation);
        //Managers.player = go.GetComponent<PlayerController>();
    }

    public override void LeftScene()
    {

    }
}
