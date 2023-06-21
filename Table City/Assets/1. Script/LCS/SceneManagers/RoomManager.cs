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
	// 1. A��ġ�� B��ġ�� ��� �� ����
	// 2. ��ŸƮ �ڿ��� �����Ͽ� ��ġ ����
	// 3. 1���� 2���� ��ģ ���ϴ� ��ġ�� ���ϰ� ��ŸƮ �ڿ��� ���ϴ� ��ġ���� ���� ( ������ �ڿ� ä���� ������ ��ġ�� �����̱� ������ �Ұ��� �Ұ��̶� �Ǵ� )

	// �÷��̾� ��ȯ ��ġ A, B
	public Transform _PlayerPointA { get; private set; }
	public Transform _PlayerPointB { get; private set; }
	
	// �÷��̾� �۾��� ��ȯ ��ġ A, B
	public Transform _WorkbenchPointA { get; private set; }
	public Transform _WorkbenchPointB { get; private set; }
	#endregion

	private PhotonView _pv;

	private void Start()
	{
		_pv = gameObject.GetComponent<PhotonView>();
		InitSapwnPoint();
	}

	// ��ȯ ��ġ �ʱ�ȭ
	private void InitSapwnPoint()
	{
		Transform spawnPoint = GameObject.Find("#SpawnPoint").transform;

		_PlayerPointA = spawnPoint.Find("Spawn_Player").Find("PointA");
		_PlayerPointB = spawnPoint.Find("Spawn_Player").Find("PointB");

		_WorkbenchPointA = spawnPoint.Find("Spawn_Workbench").Find("PointA");
		_WorkbenchPointB = spawnPoint.Find("Spawn_Workbench").Find("PointB");

		NetworkManager.Net.SpawnPlayer();
	}

	// ������Ʈ ��ȯ
	public void SpawnOBJ(prefabType _type, string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle)
	{
		if(prefabType.effect == _type)
		{
			_pv.RPC("SpawnEffect", RpcTarget.All,_objName, _spawnPoint, _spawnAngle);
		}
	}

	// ����Ʈ ��ȯ
	[PunRPC]
	private void SpawnEffect(string _objName, Vector3 _spawnPoint, Quaternion _spawnAngle)
	{
		Managers.instantiate.UsePoolingObject(prefabType.effect + _objName, _spawnPoint, _spawnAngle);
	}
}
