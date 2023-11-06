using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameScene : SceneBase
{
    private PhotonView _pv;

    public GameObject _Object_PlayerA = null;
    public GameObject _Object_PlayerB = null;

    private string _bgmName = "bgm6";

    [SerializeField]
    private Slider[] slider;
    [SerializeField]
    private TextMeshProUGUI[] sliderText;
    [SerializeField]
    private TextMeshProUGUI[] valueView;

    [SerializeField]
    private GameObject _obejcts;

    private bool _isEndGame = false;

    public override void Init()
    {
        _scene = gameObject;
        _type = Define.Scene.InGame;
        _name = "InGame";
        _pv = gameObject.GetComponent<PhotonView>();

        InitUI();
        InitEvent();
    }

    protected override void InitUI()
    {

    }

    private void InitEvent()
    {

    }

    public override void StartLoad() { OnLoad(); }

    protected override void OnLoad()
    {
        _isEndGame = false;
        Managers.Sound.BgmPlay(_bgmName);

        _obejcts.SetActive(true);

        if(Managers.Network.IsSolo())
            SpawnWorkBenchSolo();
        else
            SpawnWorkBenchMulty();

        StartCoroutine(EndingBarCycle());
    }

    private void SpawnWorkBenchMulty()
    {
        Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
        Transform playerPoint = spawnPoint.Find("Spawn_Player");
        Transform workbenchPoint = spawnPoint.Find("Spawn_Workbench");
        string workbenchName;

        if (Managers.Network.IsPlayerTeamA())
        {
            workbenchName = "0. Player/PlayerA_Workbench";
            workbenchPoint = transform.Find("Point_A");
        }
        else
        {
            workbenchName = "0. Player/PlayerB_Workbench";
            workbenchPoint = transform.Find("Point_B");
        }
        Managers.Instance.SpawnObject(workbenchName, workbenchPoint);

        if (Managers.Network.IsPlayerTeamA())
        {
            playerPoint = transform.Find("Point_A");
        }
        else
        {
            playerPoint = transform.Find("Point_B");
        }
        Managers.player.SetPos(playerPoint.position);
    }


    private void SpawnWorkBenchSolo()
    {
        Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
        Transform playerPoint = spawnPoint.Find("Spawn_Player");
        Transform workbenchPoint = spawnPoint.Find("Spawn_Workbench");
        string workbenchName = "0. Player/PlayerC_Workbench";
        workbenchPoint = workbenchPoint.Find("Point_A");

        Managers.Instance.SpawnObject(workbenchName, workbenchPoint);

        playerPoint = playerPoint.Find("Point_A");
        Managers.player.SetPos(playerPoint.position);
    }

    public void SetPlayerObject(GameObject _player, bool _pointA)
    {
        if (_pointA) { _Object_PlayerA = _player; }
        else { _Object_PlayerB = _player; }
    }

    public void SyncSpeedUp(Define.AssetData _factoryType)
    {
        _pv.RPC("FactroySpeedUp", RpcTarget.All, _factoryType);
    }

    [PunRPC]
    private void FactroySpeedUp(Define.AssetData _factroyType)
    {
        Managers.Game.factoryScript[_factroyType].speedUpState = true;
    }

    public void SpeedUp(Define.AssetData factoryType)
    {
        SyncSpeedUp(factoryType);
        Managers.Sound.SfxPlay("sharara");
    }

    private IEnumerator EndingBarCycle()
    {
        while(!_isEndGame)
        {
            int allEndingValue = 0;
            for (int i = 0; i < Managers.Game.endingValues.Length; i++)
            {
                allEndingValue += Managers.Game.endingValues[i];
            }

            for (int i = 0; i < slider.Length; i++)
            {
                slider[i].value = (float)allEndingValue / 100;
                sliderText[i].text = "¿£µù : " + allEndingValue + "%";
                valueView[i].text = $"" +
                    $"<color=\"red\">{Managers.Game.endingValues[0]}</color>/" +
                    $"<color=\"yellow\">{Managers.Game.endingValues[1]}</color>/" +
                    $"<color=\"green\">{Managers.Game.endingValues[2]}</color>/" +
                    $"<color=\"blue\">{Managers.Game.endingValues[3]}</color>";
            }
            yield return null;
        }
    }

    public override void LeftScene()
    {
        _obejcts.SetActive(false);
        _isEndGame = true;
    }
}
