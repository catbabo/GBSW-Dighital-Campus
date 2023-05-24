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

	// ���� ����
	public void Connect() => PhotonNetwork.ConnectUsingSettings();

	public override void OnConnectedToMaster()
	{
		//PhotonNetwork.LocalPlayer.NickName = _NickName.text;
		PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
	}

	// ĳ���� ��ȯ
	public void SpawnCharater()
	{

		// ĳ���� �Ѹ��̶� �����ٸ� ����

		GameObject _player = PhotonNetwork.Instantiate("0. Player/PlayerPrefab",
							 PhotonNetwork.IsMasterClient ? _SpawnPoint[0].position : _SpawnPoint[1].position, 
							 PhotonNetwork.IsMasterClient ? _SpawnPoint[0].rotation : _SpawnPoint[1].rotation);

		PhotonView _pv = _player.GetComponent<PhotonView>();
		PhotonNetwork.LocalPlayer.NickName = _pv.ViewID.ToString();
	}

	// ������ ����
	public override void OnJoinedRoom()
	{
		SpawnCharater();
	}

	// ���� ���� ����
	public void DisConnect()
	{
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
	}

	// ���� ���� ������ ����
	public override void OnDisconnected(DisconnectCause cause)
	{
		
	}
}
