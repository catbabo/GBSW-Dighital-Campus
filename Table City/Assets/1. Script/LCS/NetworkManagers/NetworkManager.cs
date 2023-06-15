using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


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
	private string _gameVersion = "1";

	// �� �̸�
	public string _roomCode { get; private set; } = "12345";

	// �г���
	public string _nickName { get; private set; } = "Admin";

	// ���� ����Ʈ
	private bool _PointA;

	private void Update()
	{
		DevelopMode();
	}

	// ��Ʈ��ũ ���� �ʱ�ȭ
	private void InitNetworkSetting()
	{
		PhotonNetwork.GameVersion = this._gameVersion;
		PhotonNetwork.SendRate = 60;
		PhotonNetwork.SerializationRate = 30;
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	#region SetInfo
	// �÷��̾� �г��� ����
	public void SetNickName(string _name) => _nickName = _name;

	// ������ �� �̸� ����
	public void SetRoomCode(string _code) => _roomCode = _code;
	#endregion

	#region Connect
	// ���� ����
	public void Connect() => PhotonNetwork.ConnectUsingSettings();

	// ������ ������ ���� �ϸ� ����
	public override void OnConnectedToMaster()
	{
		print("������ ���� ���� ����!");

		PhotonNetwork.LocalPlayer.NickName = _nickName;
	}
	#endregion

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

		JoinOrCreate(_roomCode, 2);
	}
	#endregion

	#region Room
	// �� ���� �Ǵ� ���� ( �� �̸�, ���� ���� �ο���)
	public void JoinOrCreate(string _name, int _maxPlayer)
				=> PhotonNetwork.JoinOrCreateRoom(_name, new RoomOptions { MaxPlayers = _maxPlayer }, null); // ( �� �̸�, �� �ɼ�, �κ� Ÿ�� )

	// ���� ��������� ����
	public override void OnCreatedRoom()
	{
		print("�� ����� �Ϸ�.");
	}

	// ������� �濡 ���� ����
	public override void OnJoinedRoom()
	{
		print("�� ���� ����!");
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

	#region Spawn
	// ������Ʈ ��ȯ ( ������Ʈ �̸� (Resources ���� ��ȯ��), ��ȯ�� ��ġ, ������ �������� ��ȯ�� ��ġ(��ȯ ��ġ�� ��� ���ٸ� ����� ��))
	public void SpawnObject(string _objectName, Transform _point)
	{
		GameObject _object = PhotonNetwork.Instantiate(_objectName, _point.position, _point.rotation);
	}

	// �÷��̾� ��ȯ
	public void SpawnPlayer()
	{
		SpawnObject("0. Player/PlayerPrefab", _PointA ? RoomManager.room._PlayerPointA : RoomManager.room._PlayerPointB);
	}
	#endregion

	#region Disconnect
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
	#endregion

	// �÷��̾� ��ȯ ��ġ ��
	public void SetPlayerSpawnPoint(bool _point)
	{
		_PointA = _point;
	}

	// �濡 ���� �÷��̾��� ���� ����
	public void SetJoinRoomPlayerCount(TMP_Text _text) => _text.text = "Player : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

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
		if (Input.GetKeyDown(KeyCode.J)) JoinOrCreate(_roomCode, 2);

		// D�� ������ ���� ���� ����
		if (Input.GetKeyDown(KeyCode.D)) DisConnect();
	}

}
