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
	// 1. A위치와 B위치를 골라서 방 입장 * 현재 적용중 *
	// 2. 스타트 자원을 선택하여 위치 선정
	// 3. 1번과 2번을 합친 원하는 위치를 정하고 스타트 자원을 원하는 위치에서 시작 ( 하지만 자원 채집과 공장의 위치는 고정이기 때문에 불가능 할것이라 판단 )

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

	/// <summary> 박스 컨트롤러 </summary>
	private InputBoxController _inputBoxController;

	/// <summary> 트럭 이동할 위치 </summary>
	public Transform _targetTrans { get; private set; }

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

	/// <summary> 오브젝트 소환 동기화 </summary>
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

	/// <summary> 아이템 데이터 동기화 </summary>
	/// <param name="_inputBoxController">이동할 데이터가 들어가 있는 박스 컨트롤러</param>
	/// <param name="_factoryType">데이터를 이동할 공장 타입</param>
	public void SyncItemData(InputBoxController _inputBoxC, Define.AssetData _factoryType)
	{
		_inputBoxController = _inputBoxC;
		_pv.RPC("SetItemData", RpcTarget.All, _factoryType);
	}

	public void SyncTransform(Transform _trans) { _pv.RPC("SetTargetTransform", RpcTarget.All, _trans); }

	public Transform GetTargetTransform() { return _targetTrans; }

	/// <summary> 이펙트 동기화 </summary>
	/// <param name="_objName">생성할 오브젝트 이름</param>
	/// <param name="_spawnPoint">생성할 오브젝트 위치</param>
	/// <param name="_spawnAngle">생성할 오브젝트 각도</param>
	[PunRPC]
	private void SpawnEffect(string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle) { GameObject _object = Managers.instantiate.UsePoolingObject(Define.prefabType.effect + _objName, _spawnPoint, _spawnAngle); }

	/// <summary> 아이템 데이터 동기화 </summary>
	/// <param name="_factoryType">데이터를 이동할 공장 타입</param>
	[PunRPC]
	private void SetItemData(Define.AssetData _factoryType) { _inputBoxController.SendItem(_factoryType); }

	[PunRPC]
	private void SetTargetTransform(Transform _trans) { _targetTrans = _trans; }
	
}
