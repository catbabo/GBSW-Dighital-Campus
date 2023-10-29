using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayRoomScene : MonoBehaviourPunCallbacks
{
	private PhotonView _pv;

	public GameObject _Object_PlayerA { get; private set; } = null;
	public GameObject _Object_PlayerB { get; private set; } = null;

    private string _bgmName = "bgm6";

    private void Start()
    {
        Managers.Sound.BgmPlay(_bgmName);
        
		_pv = gameObject.GetComponent<PhotonView>();
		SpawnPlayer();
	}

	private void SpawnPlayer()
    {
        Transform spawnPoint = GameObject.Find("#SpawnPoint").transform;
		Transform playerPoint, workbenchPoint;
        string workbenchName;

        if (Managers.Network.IsPlayerTeamA())
        {
            playerPoint = spawnPoint.Find("Spawn_Player").Find("Point_A");
            workbenchName = "0. Player/PlayerA_Workbench";
			workbenchPoint = spawnPoint.Find("Spawn_Workbench").Find("Point_A");
        }
        else
        {
			playerPoint = spawnPoint.Find("Spawn_Player").Find("Point_B");
            workbenchName = "0. Player/PlayerB_Workbench";
            workbenchPoint = spawnPoint.Find("Spawn_Workbench").Find("Point_B");
        }

        Managers.Instance.SpawnObject("0. Player/Player_Prefab", playerPoint);
        Managers.Instance.SpawnObject(workbenchName, workbenchPoint);
	}

    public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log(otherPlayer.NickName + " ³ª°¨.");
        Managers.Network.LeaveRoom();
        Managers.Network.SetForceOut(true);
		PhotonNetwork.LoadLevel("MainLobby");
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
}
