using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager room = null;

	private void Awake()
	{
		if(room == null)
		{
			room = this;
		}
	}

	#region SpawnPoint + New Idea
	// New Idea
	// 1. A��ġ�� B��ġ�� ��� �� ����
	// 2. ��ŸƮ �ڿ��� �����Ͽ� ��ġ ����
	// 3. 1���� 2���� ��ģ ���ϴ� ��ġ�� ���ϰ� ��ŸƮ �ڿ��� ���ϴ� ��ġ���� ���� ( ������ �ڿ� ä���� ������ ��ġ�� �����̱� ������ �Ұ��� �Ұ��̶� �Ǵ� )

	// �� ������ ĳ���� ��ȯ ��ġ
	public Transform _PlayerPointA { get; private set; }
	// �� �մ��� ĳ���� ��ȯ ��ġ
	public Transform _PlayerPointB { get; private set; }
	#endregion

	private void Start()
	{
		SetSapwnPoint();
	}

	// �÷��̾� ��ȯ ��ġ ����
	private void SetSapwnPoint()
	{
		Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
		_PlayerPointA = spawnPoint.Find("Point_A").Find("Player");
		_PlayerPointB = spawnPoint.Find("Point_B").Find("Player");

		NetworkManager.Net.SpawnPlayer();
	}
}
