using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

	public TMP_InputField _NickName;
	public GameObject canvas;

	private void Awake()
	{
		PhotonNetwork.SendRate = 60;
		PhotonNetwork.SerializationRate = 30;
	}

	// ���� ����
	public void Connect() => PhotonNetwork.ConnectUsingSettings();

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.LocalPlayer.NickName = _NickName.text;
		PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
	}

	// ĳ���� ��ȯ
	public void SpawnCharater()
	{
		PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
	}

	// ������ ����
	public override void OnJoinedRoom()
	{
		canvas.SetActive(false);

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
		canvas.SetActive(true);
	}
}
