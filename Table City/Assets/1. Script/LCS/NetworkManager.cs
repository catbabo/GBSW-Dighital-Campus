using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class NetworkManager : MonoBehaviourPunCallbacks
{
	#region Singleton
	public static NetworkManager Net = null;
	private void Awake()
	{
		if (Net == null)
		{
			Net = this;

			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
		InitNetworkSetting();
	}
	#endregion

	// ���� ����
	//private string _gameVersion = "1";

	// �� �̸�
	public string _roomCode { get; private set; }

	// �г���
	public string _nickName { get; private set; }

	private void Update()
	{
		DevelopMode();
	}

	// ��Ʈ��ũ ���� �ʱ�ȭ
	private void InitNetworkSetting()
	{
		//PhotonNetwork.GameVersion = this._gameVersion;
		PhotonNetwork.SendRate = 60;
		PhotonNetwork.SerializationRate = 30;
	}

	// ������ ���
	private void DevelopMode()
	{
		// A�� ������ "12345" �濡 ����
		if (Input.GetKeyDown(KeyCode.A))
		{
			_roomCode = "12345";
			_nickName = "asdf";
			Connect();
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			_roomCode = "12345";
			_nickName = "fdas";
			Connect();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			print("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
			print("�� ���� : " + PhotonNetwork.CountOfRooms);
			string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
			print(playerStr);
		}
	}

	// ���� ����
	public void Connect() => PhotonNetwork.ConnectUsingSettings();

	// ������ ������ ���� �ϸ� ����
	public override void OnConnectedToMaster()
	{
		print("������ ���� ���� ����!");
		PhotonNetwork.LocalPlayer.NickName = _nickName;
		PhotonNetwork.LoadLevel("PlayRoom");
		JoinOrCreate(_roomCode);
	}

	#region PlayerInfo
	// �÷��̾� �г��� ����
	public void _SetNickName(string _name) => _nickName = _name;

	// ������ �� �̸� ����
	public void _SetRoomCode(string _code) => _roomCode = _code;
	#endregion

	// �� ���� �Ǵ� ���� ( �� �̸�, �� �ɼ� (����� ���� �ο��� 2�� �ִ�� ����), �κ� Ÿ��)
	public void JoinOrCreate(string _name) 
				=> PhotonNetwork.JoinOrCreateRoom(_name, new RoomOptions { MaxPlayers = 2 }, null);

	// ������Ʈ ��ȯ ( ������Ʈ �̸� (Resources ���� ��ȯ��), ��ȯ�� ��ġ, ������ �������� ��ȯ�� ��ġ(��ȯ ��ġ�� ��� ���ٸ� ����� ��))
	public void SpawnObject(string objectName, Transform CommonPoint, Transform MarsterPoint = null)
	{
		if(MarsterPoint == null) { MarsterPoint = CommonPoint; }
		GameObject _object = PhotonNetwork.Instantiate(objectName,
							 PhotonNetwork.IsMasterClient ? MarsterPoint.position : CommonPoint.position, 
							 PhotonNetwork.IsMasterClient ? MarsterPoint.rotation : CommonPoint.rotation);
	}

	// ���� ��������� ����
	public override void OnCreatedRoom()
	{
		print("�� ����� �Ϸ�.");
	}

	// ������� �濡 ���� ����
	public override void OnJoinedRoom()
	{
		print("�� ���� ����!");
		SpawnPlayer();
	}

	// �÷��̾� ��ȯ
	private void SpawnPlayer()
	{
		SpawnObject("0. Player/PlayerPrefab", RoomManager.room.CommonPoint, RoomManager.room.MasterPoint);
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
		print("���� ���� ����");
	}

}
