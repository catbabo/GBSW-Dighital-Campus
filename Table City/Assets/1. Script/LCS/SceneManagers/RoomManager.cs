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

	#region SpawnPoint + New Idea
	// New Idea
	// 1. A위치와 B위치를 골라서 방 입장
	// 2. 스타트 자원을 선택하여 위치 선정
	// 3. 1번과 2번을 합친 원하는 위치를 정하고 스타트 자원을 원하는 위치에서 시작 ( 하지만 자원 채집과 공장의 위치는 고정이기 때문에 불가능 할것이라 판단 )

	// 플레이어 소환 위치 A, B
	public Transform _PlayerPointA { get; private set; }
	public Transform _PlayerPointB { get; private set; }
	
	// 플레이어 작업대 소환 위치 A, B
	public Transform _WorkbenchPointA { get; private set; }
	public Transform _WorkbenchPointB { get; private set; }
	#endregion

	private PhotonView _pv;

	private void Start()
	{
		_pv = gameObject.GetComponent<PhotonView>();
		InitSapwnPoint();
	}

	// 소환 위치 초기화
	private void InitSapwnPoint()
	{
		Transform spawnPoint = GameObject.Find("#SpawnPoint").transform;

		_PlayerPointA = spawnPoint.Find("Spawn_Player").Find("PointA");
		_PlayerPointB = spawnPoint.Find("Spawn_Player").Find("PointB");

		_WorkbenchPointA = spawnPoint.Find("Spawn_Workbench").Find("PointA");
		_WorkbenchPointB = spawnPoint.Find("Spawn_Workbench").Find("PointB");

		NetworkManager.Net.SpawnPlayer();
	}

	// 오브젝트 소환
	public void SpawnOBJ(prefabType _type, string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle)
	{
		if(prefabType.effect == _type)
		{
			_pv.RPC("SpawnEffect", RpcTarget.All,_objName, _spawnPoint, _spawnAngle);
		}
	}

	// 이펙트 소환
	[PunRPC]
	private void SpawnEffect(string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle)
	{
		Managers.instantiate.UsePoolingObject(prefabType.effect + _objName, _spawnPoint, _spawnAngle);
	}
}
