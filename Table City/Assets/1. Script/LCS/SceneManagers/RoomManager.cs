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

	#region Syncron
	/// <summary> ������Ʈ ��ȯ ����ȭ ���� </summary>
	/// <param name="_type">��ȯ�� ������Ʈ�� Ÿ��</param>
	/// <param name="_objName">��ȯ�� ������Ʈ�� �̸�</param>
	/// <param name="_spawnPoint">��ȯ�� ������Ʈ�� ��ġ</param>
	/// <param name="_spawnAngle">��ȯ�� ������Ʈ�� ����</param>
	public void SyncSpawnObejct(Define.prefabType _type, string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle)
	{
		if(Define.prefabType.effect == _type)
		{
			_pv.RPC("SpawnEffect", RpcTarget.All,_objName, _spawnPoint, _spawnAngle);
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
	private void SpawnEffect(string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle, int _factroyNum = -1)
	{ 
		GameObject _object = Managers.instantiate.UsePoolingObject(Define.prefabType.effect + _objName, _spawnPoint, _spawnAngle);
		if(_objName == "truck") { _object.GetComponent<Throw>().SetTargetPosition(AssetManager._asset.GetTargetPosition(_factroyNum)); }
	}
	#endregion
}
