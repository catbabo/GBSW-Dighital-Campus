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
	// 1. A위치와 B위치를 골라서 방 입장
	// 2. 스타트 자원을 선택하여 위치 선정
	// 3. 1번과 2번을 합친 원하는 위치를 정하고 스타트 자원을 원하는 위치에서 시작 ( 하지만 자원 채집과 공장의 위치는 고정이기 때문에 불가능 할것이라 판단 )

	// 방 주인의 캐릭터 소환 위치
	public Transform MasterPoint { get; private set; }
	// 방 손님의 캐릭터 소환 위치
	public Transform CommonPoint { get; private set; }
	#endregion

	private void Start()
	{
		SetSapwnPoint();
	}

	// 플레이어 소환 위치 설정
	private void SetSapwnPoint()
	{
		Transform spawnPoint = GameObject.Find("SpawnPoint").transform;
		MasterPoint = spawnPoint.Find("MasterPoint");
		CommonPoint = spawnPoint.Find("CommonPoint");

		NetworkManager.Net.SpawnPlayer();
	}
}
