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
	// 1. A��ġ�� B��ġ�� ��� �� ���� * ���� ������ *
	// 2. ��ŸƮ �ڿ��� �����Ͽ� ��ġ ����
	// 3. 1���� 2���� ��ģ ���ϴ� ��ġ�� ���ϰ� ��ŸƮ �ڿ��� ���ϴ� ��ġ���� ���� ( ������ �ڿ� ä���� ������ ��ġ�� �����̱� ������ �Ұ��� �Ұ��̶� �Ǵ� )

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

	/// <summary> �ڽ� ��Ʈ�ѷ� </summary>
	private InputBoxController _inputBoxController;

	/// <summary> Ʈ�� �̵��� ��ġ </summary>
	public Transform _targetTrans { get; private set; }

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

	/// <summary> ������Ʈ ��ȯ ����ȭ </summary>
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

	/// <summary> ������ ������ ����ȭ </summary>
	/// <param name="_inputBoxController">�̵��� �����Ͱ� �� �ִ� �ڽ� ��Ʈ�ѷ�</param>
	/// <param name="_factoryType">�����͸� �̵��� ���� Ÿ��</param>
	public void SyncItemData(InputBoxController _inputBoxC, Define.AssetData _factoryType)
	{
		_inputBoxController = _inputBoxC;
		_pv.RPC("SetItemData", RpcTarget.All, _factoryType);
	}

	public void SyncTransform(Transform _trans) { _pv.RPC("SetTargetTransform", RpcTarget.All, _trans); }

	public Transform GetTargetTransform() { return _targetTrans; }

	/// <summary> ����Ʈ ����ȭ </summary>
	/// <param name="_objName">������ ������Ʈ �̸�</param>
	/// <param name="_spawnPoint">������ ������Ʈ ��ġ</param>
	/// <param name="_spawnAngle">������ ������Ʈ ����</param>
	[PunRPC]
	private void SpawnEffect(string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle) { GameObject _object = Managers.instantiate.UsePoolingObject(Define.prefabType.effect + _objName, _spawnPoint, _spawnAngle); }

	/// <summary> ������ ������ ����ȭ </summary>
	/// <param name="_factoryType">�����͸� �̵��� ���� Ÿ��</param>
	[PunRPC]
	private void SetItemData(Define.AssetData _factoryType) { _inputBoxController.SendItem(_factoryType); }

	[PunRPC]
	private void SetTargetTransform(Transform _trans) { _targetTrans = _trans; }
	
}
