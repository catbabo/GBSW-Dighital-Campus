using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Transform[] PlayerPoint;

	private void Start()
	{
		NetworkManager.Net.SpawnObject("0. Player/PlayerPrefab", PlayerPoint[0], PlayerPoint[1]);
	}
}
