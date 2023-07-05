using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
	#region Singleton
	public static RoomManager room = null;

	private void Awake()
	{
		if(room == null)
		{
			room = this;
		}
	}
	#endregion

	#region SpawnPoint
	/// <summary> 플레이어 소환 위치 A </summary>
	public Transform _PlayerPointA { get; private set; }
	/// <summary> 플레이어 소환 위치 B </summary>
	public Transform _PlayerPointB { get; private set; }
	
	/// <summary> 플레이어 작업대 소환 위치 A </summary>
	public Transform _WorkbenchPointA { get; private set; }
	/// <summary> 플레이어 작업대 소환 위치 B </summary>
	public Transform _WorkbenchPointB { get; private set; }
	#endregion

	/// <summary> 포톤 뷰 </summary>
	private PhotonView _pv;

	private void Start()
	{
		_pv = gameObject.GetComponent<PhotonView>();
		InitSapwnPoint();
	}

	/// <summary> 소환 위치 초기화 </summary>
	private void InitSapwnPoint()
	{
		Transform spawnPoint = GameObject.Find("#SpawnPoint").transform;

		_PlayerPointA = spawnPoint.Find("Spawn_Player").Find("Point_A");
		_PlayerPointB = spawnPoint.Find("Spawn_Player").Find("Point_B");

		_WorkbenchPointA = spawnPoint.Find("Spawn_Workbench").Find("Point_A");
		_WorkbenchPointB = spawnPoint.Find("Spawn_Workbench").Find("Point_B");

		NetworkManager.Net.SpawnPlayer();
	}

	// 플레이어가 방에서 나간다면 실행
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log(otherPlayer.NickName + " 나감.");
		NetworkManager.Net.LeaveRoom();
		NetworkManager.Net.SetForceOut(true);
		PhotonNetwork.LoadLevel("MainLobby");
	}

	#region Syncron
	/// <summary> 오브젝트 소환 동기화 실행 </summary>
	/// <param name="_type">소환할 오브젝트의 타입</param>
	/// <param name="_objName">소환할 오브젝트의 이름</param>
	/// <param name="_spawnPoint">소환할 오브젝트의 위치</param>
	/// <param name="_spawnAngle">소환할 오브잭트의 각도</param>
	public void SyncSpawnObejct(Define.prefabType _type, string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle)
	{
		if(Define.prefabType.effect == _type)
		{
			_pv.RPC("SpawnEffect", RpcTarget.All,_objName, _spawnPoint, _spawnAngle);
		}
	}
	#endregion

	#region PunRPC
	/// <summary> 이펙트 동기화 </summary>
	/// <param name="_objName">생성할 오브젝트 이름</param>
	/// <param name="_spawnPoint">생성할 오브젝트 위치</param>
	/// <param name="_spawnAngle">생성할 오브젝트 각도</param>
	/// <param name="_factroyNum">목표 공장 번호</param>
	[PunRPC]
	private void SpawnEffect(string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle, int _factroyNum = -1)
	{ 
		GameObject _object = Managers.instantiate.UsePoolingObject(Define.prefabType.effect + _objName, _spawnPoint, _spawnAngle);
		if(_objName == "truck") { _object.GetComponent<Throw>().SetTargetPosition(AssetManager._asset.GetTargetPosition(_factroyNum)); }
	}
	#endregion
}
