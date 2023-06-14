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
	public string _roomCode { get; private set; } = "12345";

	// �г���
	public string _nickName { get; private set; } = "Admin";

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
		// M�� ������ ������ ������ ����
		if (Input.GetKeyDown(KeyCode.M)) Connect();

		// B�� ������ �κ� ����
		if (Input.GetKeyDown(KeyCode.B)) JoinLobby();

		// I�� ������ ���� ���� ȣ��
		if (Input.GetKeyDown(KeyCode.I)) Info();

		// L�� ������ �� ������
		if (Input.GetKeyDown(KeyCode.L)) LeaveRoom();

		// R�� ������ �� ����
		if (Input.GetKeyDown(KeyCode.J)) JoinOrCreate(_roomCode);

		// D�� ������ ���� ���� ����
		if (Input.GetKeyDown(KeyCode.D)) DisConnect();
	}

	// ���� ����
	public void Connect() => PhotonNetwork.ConnectUsingSettings();

	// ������ ������ ���� �ϸ� ����
	public override void OnConnectedToMaster()
	{
		print("������ ���� ���� ����!");
		PhotonNetwork.LocalPlayer.NickName = _nickName;
		PhotonNetwork.LoadLevel("PlayRoom");
	}

	#region Lobby
	// �κ� �����ϱ�.
	public void JoinLobby()
	{
		PhotonNetwork.JoinLobby();
	}
	
	// �κ� ���� �ϸ� ����
	public override void OnJoinedLobby()
	{
		print("�κ� ���� �Ϸ�.");
	}
	#endregion

	#region PlayerInfo
	// �÷��̾� �г��� ����
	public void _SetNickName(string _name) => _nickName = _name;

	// ������ �� �̸� ����
	public void _SetRoomCode(string _code) => _roomCode = _code;
	#endregion

	#region Room
	// �� ���� �Ǵ� ���� ( �� �̸�, �� �ɼ� (����� ���� �ο��� 2�� �ִ�� ����), �κ� Ÿ��)
	public void JoinOrCreate(string _name) 
				=> PhotonNetwork.JoinOrCreateRoom(_name, new RoomOptions { MaxPlayers = 2 }, null);

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

	// �� ����
	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	// �� ����� ����
	public override void OnLeftRoom()
	{
		print("�� ���� ����!");
	}
	#endregion

	// ������Ʈ ��ȯ ( ������Ʈ �̸� (Resources ���� ��ȯ��), ��ȯ�� ��ġ, ������ �������� ��ȯ�� ��ġ(��ȯ ��ġ�� ��� ���ٸ� ����� ��))
	public void SpawnObject(string objectName, Transform CommonPoint, Transform MarsterPoint = null)
	{
		if(MarsterPoint == null) { MarsterPoint = CommonPoint; }
		GameObject _object = PhotonNetwork.Instantiate(objectName,
							 PhotonNetwork.IsMasterClient ? MarsterPoint.position : CommonPoint.position, 
							 PhotonNetwork.IsMasterClient ? MarsterPoint.rotation : CommonPoint.rotation);
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

	// ���� ����
	public void Info()
	{
		if (PhotonNetwork.InRoom)
		{
			print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
			print("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
			print("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

			string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
			print(playerStr);
		}
		else
		{
			print("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
			print("�� ���� : " + PhotonNetwork.CountOfRooms);
			print("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
			print("�κ� �ִ���? : " + PhotonNetwork.InLobby);
			print("����ƴ���? : " + PhotonNetwork.IsConnected);
		}
	}
}
