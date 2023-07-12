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
	/// <summary> �÷��̾� ��ȯ ��ġ A </summary>
	public Transform _PlayerPointA { get; private set; }
	/// <summary> �÷��̾� ��ȯ ��ġ B </summary>
	public Transform _PlayerPointB { get; private set; }
	
	/// <summary> �÷��̾� �۾��� ��ȯ ��ġ A </summary>
	public Transform _WorkbenchPointA { get; private set; }
	/// <summary> �÷��̾� �۾��� ��ȯ ��ġ B </summary>
	public Transform _WorkbenchPointB { get; private set; }
	#endregion

	/// <summary> ���� �� </summary>
	private PhotonView _pv;

	public GameObject _Object_PlayerA { get; private set; } = null;
	public GameObject _Object_PlayerB { get; private set; } = null;

	private void Start()
	{
		_pv = gameObject.GetComponent<PhotonView>();
		InitSapwnPoint();
	}

	/// <summary> ��ȯ ��ġ �ʱ�ȭ </summary>
	private void InitSapwnPoint()
	{
		Transform spawnPoint = GameObject.Find("#SpawnPoint").transform;

		_PlayerPointA = spawnPoint.Find("Spawn_Player").Find("Point_A");
		_PlayerPointB = spawnPoint.Find("Spawn_Player").Find("Point_B");

		_WorkbenchPointA = spawnPoint.Find("Spawn_Workbench").Find("Point_A");
		_WorkbenchPointB = spawnPoint.Find("Spawn_Workbench").Find("Point_B");

		NetworkManager.Net.SpawnPlayer();
	}

	// �÷��̾ �濡�� �����ٸ� ����
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log(otherPlayer.NickName + " ����.");
		NetworkManager.Net.LeaveRoom();
		NetworkManager.Net.SetForceOut(true);
		PhotonNetwork.LoadLevel("MainLobby");
	}

	/// <summary> ���� �÷��̾� ������Ʈ �������� </summary>
	/// <param name="_player">�÷��̾� ������Ʈ</param>
	/// <param name="_pointA">�÷��̾��� ������</param>
	public void SetPlayerObject(GameObject _player, bool _pointA)
	{
		if (_pointA) { _Object_PlayerA = _player; }
		else { _Object_PlayerB = _player; }
	}

	/// <summary> ���� ���ǵ�� ����ȭ ���� </summary>
	/// <param name="_factoryType">����ȭ �� ���� ������</param>
	public void SyncSpeedUp(Define.AssetData _factoryType)
	{
		_pv.RPC("FactorySpeedUp", RpcTarget.All, _factoryType);
	}
	/// <summary> ���� ���ǵ� �� </summary>
	/// <param name="_factroyData">���ǵ带 �ø� ���� Ÿ��</param>
	[PunRPC]
	private void FactroySpeedUp(Define.AssetData _factroyType)
	{
		Managers.system.factoryScript[_factroyType].speedUpState = true;
	}



	#region Syncron
	/// <summary> ������Ʈ ��ȯ ����ȭ ���� </summary>
	/// <param name="_type">��ȯ�� ������Ʈ�� Ÿ��</param>
	/// <param name="_objName">��ȯ�� ������Ʈ�� �̸�</param>
	/// <param name="_spawnPoint">��ȯ�� ������Ʈ�� ��ġ</param>
	/// <param name="_spawnAngle">��ȯ�� ������Ʈ�� ����</param>
	public void SyncSpawnObejct(Define.prefabType _type, string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle, Define.AssetData _assetType)
	{
		if(Define.prefabType.effect == _type)
		{
			_pv.RPC("SpawnEffect", RpcTarget.All,_objName, _spawnPoint, _spawnAngle, _assetType);
		}
	}
	#endregion

	#region PunRPC
	/// <summary> ����Ʈ ����ȭ </summary>
	/// <param name="_objName">������ ������Ʈ �̸�</param>
	/// <param name="_spawnPoint">������ ������Ʈ ��ġ</param>
	/// <param name="_spawnAngle">������ ������Ʈ ����</param>
	/// <param name="_factroyNum">��ǥ ���� ��ȣ</param>
	[PunRPC]
	private void SpawnEffect(string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle, Define.AssetData _assetType)
	{ 
		GameObject _object = Managers.instantiate.UsePoolingObject(Define.prefabType.effect + _objName, _spawnPoint, _spawnAngle);
		if(_objName == "truck") { _object.GetComponent<Throw>().SetTargetPosition(AssetManager._asset.GetTargetPosition((int)_assetType)); }
	}
	#endregion
}
