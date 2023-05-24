using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

	public Transform[] _SpawnPoint;

	private void Awake()
	{
		PhotonNetwork.SendRate = 60;
		PhotonNetwork.SerializationRate = 30;
	}

	private void Start()
	{
		Connect();
	}

	// 서버 연결
	public void Connect() => PhotonNetwork.ConnectUsingSettings();

	public override void OnConnectedToMaster()
	{
		//PhotonNetwork.LocalPlayer.NickName = _NickName.text;
		PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
	}

	// 캐릭터 소환
	public void SpawnCharater()
	{

		// 캐릭터 한명이라도 나간다면 방폭

		GameObject _player = PhotonNetwork.Instantiate("0. Player/PlayerPrefab",
							 PhotonNetwork.IsMasterClient ? _SpawnPoint[0].position : _SpawnPoint[1].position, 
							 PhotonNetwork.IsMasterClient ? _SpawnPoint[0].rotation : _SpawnPoint[1].rotation);

		PhotonView _pv = _player.GetComponent<PhotonView>();
		PhotonNetwork.LocalPlayer.NickName = _pv.ViewID.ToString();
	}

	// 서버룸 조인
	public override void OnJoinedRoom()
	{
		SpawnCharater();
	}

	// 서버 연결 해제
	public void DisConnect()
	{
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
	}

	// 서버 연결 해제시 실행
	public override void OnDisconnected(DisconnectCause cause)
	{
		
	}
}
