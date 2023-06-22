using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{

	// ���� ����
	private string _gameVersion = "1";

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

	#region PlayerServerInfo
	// �� �� �̸�
	public string _roomCode { get; private set; } = "12345";

	// �г���
	public string _nickName { get; private set; } = "Admin";

	// �÷��̾ A ����Ʈ�� ������ ����
	// true : �÷��̾� A ����Ʈ���� ��ȯ , false : �÷��̾� B ����Ʈ���� ��ȯ
	private bool _pointA;
	#endregion

	/// <summary>�����ڸ�� ��� ���� (true : �����ڸ�� ��� , false : �����ڸ�� ����)</summary>
	private bool _onDevelopMode = false;

	/// <summary>��Ʈ��ũ ���� �ʱ�ȭ</summary>
	private void InitNetworkSetting()
	{
		PhotonNetwork.GameVersion = this._gameVersion; // ���� ���� ���� ( ������ ���� ��������� ��Ī�� ������ )
		PhotonNetwork.SendRate = 60; // �ʴ� ��Ű���� �����ϴ� Ƚ��
		PhotonNetwork.SerializationRate = 30; // �ʴ� OnPhotonSerialize�� �����ϴ� Ƚ��
		PhotonNetwork.AutomaticallySyncScene = true; // PhotonNetwork.LoadLevel�� ����Ͽ��� �� ��� �����ڸ� ������ ������ �̵��ϰ� �ϴ����� ����
	}

	private void Update()
	{
		// D, M�� ���ÿ� ������ �����ڸ�� ��� �Ǵ� ����
		if(Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.M)) { _onDevelopMode = !_onDevelopMode; print("������ ��� : " + _onDevelopMode); }
		if (_onDevelopMode) { DevelopMode(); }
	}

	#region SetPlayerServerInfo
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
	// �κ� ����
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
	// �� ���� ���� ������ ���� ( �� �̸�, ���� ���� �ο���)
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
	// ������Ʈ ��ȯ ( ������Ʈ ���� ��ġ (Resources ���� ��ȯ��), ��ȯ�� ��ġ, ������ �������� ��ȯ�� ��ġ(��ȯ ��ġ�� ��� ���ٸ� ����� ��))
	public void SpawnObject(string _objectName, Transform _point)
	{
		GameObject _object = PhotonNetwork.Instantiate(_objectName, _point.position, _point.rotation);
	}

	// �÷��̾� ��ȯ
	public void SpawnPlayer()
	{
		SpawnObject("0. Player/Player_Prefab", _pointA ? RoomManager.room._PlayerPointA : RoomManager.room._PlayerPointB);
		SpawnObject("0. Player/Player_Workbench", _pointA ? RoomManager.room._WorkbenchPointA : RoomManager.room._WorkbenchPointB);
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

	// �÷��̾ ������ ��ġ ����
	public void SetPlayerSpawnPoint(bool _point) => _pointA = _point;

	// �濡 ���� �÷��̾��� ���� ����
	// public void SetJoinRoomPlayerCount(TMP_Text _text) => _text.text = "Player : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

	// ���� ���� ���
	// �濡 �� �ִٸ� �� �̸�, �� �ο���, �� �ִ� �ο���, �濡 �ִ� �÷��̾� ��� ���
	// �׷��� ������ ������ �ο� ��, �� ����, ��� �濡 �ִ� �ο� ��, �κ� �ִ����� ����, ������ �ƴ����� ����
	public void Info()
	{
		if (PhotonNetwork.InRoom)
		{
			Debug.Log("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
			Debug.Log("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
			Debug.Log("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

			string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
			Debug.Log(playerStr);
		}
		else
		{
			Debug.Log("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
			Debug.Log("�� ���� : " + PhotonNetwork.CountOfRooms);
			Debug.Log("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
			Debug.Log("�κ� �ִ���? : " + PhotonNetwork.InLobby);
			Debug.Log("����ƴ���? : " + PhotonNetwork.IsConnected);
		}
	}

	// ������ ���
	// ��κ� ���ڸ��� ���ͼ� ������
	// Ű�ڵ� : 'C'onnect, lo'B'by, 'I'nfo, 'L'eave , 'J'oin, 'D'isconnect
	private void DevelopMode()
	{

		// B�� ������ �κ� ����
		if (Input.GetKeyDown(KeyCode.B)) JoinLobby();

		// C�� ������ ������ ������ ����
		if (Input.GetKeyDown(KeyCode.C)) Connect();

		// D�� ������ ���� ���� ����
		if (Input.GetKeyDown(KeyCode.D)) DisConnect();

		// I�� ������ ���� ���� ȣ��
		if (Input.GetKeyDown(KeyCode.I)) Info();

		// J�� ������ �� ����
		if (Input.GetKeyDown(KeyCode.J)) JoinOrCreate(_roomCode, 2);

		// L�� ������ �� ������
		if (Input.GetKeyDown(KeyCode.L)) LeaveRoom();

	}

}
